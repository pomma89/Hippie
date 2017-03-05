// File name: PairingHeap.cs
//
// Author(s): Alessio Parma <alessio.parma@gmail.com>
//
// Copyright (c) 2012-2016 Alessio Parma <alessio.parma@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace DIBRIS.Hippie
{
    using Core;
    using PommaLabs.CodeServices.Common.Collections;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public sealed class PairingHeap<TVal, TPr> : TreeHeap<TVal, TPr>, IRawHeap<TVal, TPr>
    {
        private readonly LinkedQueue<Tree> _auxQueue = new LinkedQueue<Tree>();
        private readonly LinkedStack<Tree> _auxStack = new LinkedStack<Tree>();
        private readonly SinglyLinkedList<Tree> _treePool = new SinglyLinkedList<Tree>();
        private Tree _mainTree;

        internal PairingHeap(IComparer<TPr> cmp)
            : base(cmp)
        {
        }

        private Tree MainTree
        {
            set
            {
                _mainTree = value;
                if (_mainTree != null)
                {
                    _mainTree.Parent = null;
                }
            }
        }

        public IHeapHandle<TVal, TPr> Add(TVal value, TPr priority)
        {
            var tree = new Tree(value, priority, Version);
            _treePool.AddLast(tree);
            FixMin(tree);
            Count++;
            return tree.Handle;
        }

        public void Add(IHeapHandle<TVal, TPr> handle)
        {
            Add(handle.Value, handle.Priority);
        }

        void IThinHeap<TVal, TPr>.Add(TVal val, TPr priority)
        {
            Add(val, priority);
        }

        public void Clear()
        {
            ClearBase();
            _treePool.Clear();
            _mainTree = null;
        }

        public void CopyTo(IHeapHandle<TVal, TPr>[] array, int idx)
        {
            this.CommonCopyTo(array, idx, h => h);
        }

        public IEnumerator<IHeapHandle<TVal, TPr>> GetEnumerator()
        {
            if (Count == 0)
            {
                yield break;
            }
            foreach (var tree in _treePool.SelectMany(t => t.BreadthFirstVisit()))
            {
                yield return tree.Handle;
            }
            if (_mainTree == null)
            {
                yield break;
            }
            foreach (var tree in _mainTree.BreadthFirstVisit())
            {
                yield return tree.Handle;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Merge<TVal2, TPr2>(IThinHeap<TVal2, TPr2> otherHeap)
            where TVal2 : TVal
            where TPr2 : TPr
        {
            if (ReferenceEquals(this, otherHeap) || otherHeap.Count == 0)
            {
                return;
            }
            var other = otherHeap as PairingHeap<TVal, TPr>;
            if (other == null)
            {
                this.CommonMerge(otherHeap);
                return;
            }
            other.Version.Id = Version.Id; // Updates all other nodes version
            _treePool.Append(other._treePool);
            if (other._mainTree != null)
            {
                _treePool.AddLast(other._mainTree);
            }
            FixMin(other.MinTree);
            Count += other.Count;
            other.Clear();
        }

        public bool Remove(IHeapHandle<TVal, TPr> handle)
        {
            var treeHandle = GetHandle(handle as TreeHandle);
            if (treeHandle == null)
            {
                return false;
            }
            var tree = treeHandle.Tree;
            // We put this instuction here because the object referenced by tree may be changed by
            // the instructions below.
            tree.Handle.Version = null;
            if (ReferenceEquals(tree, MinTree))
            {
                // Given tree is the minimum, so we can use the same procedure used within RemoveMin,
                // with a small difference. See below...
                var min = NullMeld(MultiPassPairing(), _mainTree);
                MainTree = MinTree = TwoPassPairing(min.Children);
                // If the new min is not the old min (this can happen when two or more nodes have the
                // same priority), we need to ensure given value is really the value we are going to
                // delete. To do so, we swap values between tree and min.
                if (!ReferenceEquals(tree, min))
                {
                    tree.SwapRootWith(min);
                }
            }
            else
            {
                var pairedChildren = TwoPassPairing(tree.Children);
                if (ReferenceEquals(tree, _mainTree))
                {
                    MainTree = pairedChildren;
                }
                else if (tree.Parent == null)
                {
                    _treePool.Remove(tree);
                    if (pairedChildren != null)
                    {
                        pairedChildren.Parent = null;
                        _treePool.AddLast(pairedChildren);
                    }
                }
                else
                {
                    var parent = tree.Parent;
                    var grandParent = parent.Parent;
                    parent.Children.Remove(tree);
                    var meld = NullMeld(parent, pairedChildren);
                    if (ReferenceEquals(parent, _mainTree))
                    {
                        MainTree = meld;
                    }
                    else if (grandParent == null)
                    {
                        _treePool.Remove(parent);
                        meld.Parent = null;
                        _treePool.AddLast(meld);
                    }
                    else
                    {
                        grandParent.Children.Remove(parent);
                        meld.Parent = grandParent;
                        grandParent.Children.AddLast(meld);
                    }
                }
            }
            Count--;
            return true;
        }

        public IHeapHandle<TVal, TPr> RemoveMin()
        {
            var oldMin = MinTree;
            var min = NullMeld(MultiPassPairing(), _mainTree);
            if (!ReferenceEquals(min, oldMin))
            {
                min.SwapRootWith(oldMin);
            }
            MainTree = MinTree = TwoPassPairing(min.Children);
            Count--;
            min.Handle.Version = null;
            return min.Handle;
        }

        public IEnumerable<IReadOnlyTree<TVal, TPr>> ToReadOnlyForest()
        {
            if (Count == 0)
            {
                yield break;
            }
            foreach (var tree in _treePool)
            {
                yield return tree.ToReadOnlyTree();
            }
            if (_mainTree != null)
            {
                yield return _mainTree.ToReadOnlyTree();
            }
        }

        public override string ToString()
        {
            return this.CommonToString();
        }

        protected override void MoveDown(TreeHandle handle)
        {
            var tree = handle.Tree;
            if (tree.Children.Count > 0)
            {
                foreach (var child in tree.Children)
                {
                    child.Parent = null;
                }
                _treePool.Append(tree.Children);
            }
            if (!ReferenceEquals(tree, MinTree))
            {
                return;
            }
            MainTree = MinTree = NullMeld(MultiPassPairing(), _mainTree);
        }

        protected override void MoveUp(TreeHandle handle)
        {
            var tree = handle.Tree;
            var parent = tree.Parent;
            if (parent != null)
            {
                parent.Children.Remove(tree);
                tree.Parent = null;
                _treePool.AddLast(tree);
            }
            FixMin(tree);
        }

        private void FixMin(Tree tree)
        {
            if (MinTree != null && Cmp(MinTree.Priority, tree.Priority) < 0)
            {
                return;
            }
            Debug.Assert(tree.Parent == null);
            MinTree = tree;
        }

        private Tree MultiPassPairing()
        {
            if (_treePool.Count == 0)
            {
                return null;
            }
            foreach (var tree in _treePool)
            {
                _auxQueue.Enqueue(tree);
            }
            _treePool.Clear();
            while (_auxQueue.Count >= 2)
            {
                _auxQueue.Enqueue(Meld(_auxQueue.Dequeue(), _auxQueue.Dequeue()));
            }
            return _auxQueue.Dequeue();
        }

        private Tree TwoPassPairing(ICollection<Tree> trees)
        {
            if (trees.Count == 0)
            {
                return null;
            }
            var en = trees.GetEnumerator();
            while (en.MoveNext())
            {
                var tmp = en.Current;
                _auxStack.Push(en.MoveNext() ? Meld(tmp, en.Current) : tmp);
            }
            var merged = _auxStack.Pop();
            while (_auxStack.Count != 0)
            {
                merged = Meld(merged, _auxStack.Pop());
            }
            return merged;
        }
    }
}