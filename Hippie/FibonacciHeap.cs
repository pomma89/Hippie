// FibonacciHeap.cs
//
// Author: Alessio Parma <alessio.parma@gmail.com>
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
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class FibonacciHeap<TVal, TPr> : TreeHeap<TVal, TPr>, IRawHeap<TVal, TPr>
    {
        private const short EmptyMark = 0;
        private const short FullMark = 2;
        private readonly Tree[] _mergeBuffer;
        private readonly SinglyLinkedList<Tree> _treePool;

        internal FibonacciHeap(IComparer<TPr> cmp)
            : base(cmp)
        {
            _mergeBuffer = new Tree[(int) Math.Ceiling(Math.Log(int.MaxValue, 2)) * 2];
            _treePool = new SinglyLinkedList<Tree>();
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
        }

        public void CopyTo(IHeapHandle<TVal, TPr>[] array, int idx)
        {
            this.CommonCopyTo(array, idx, h => h);
        }

        public IEnumerator<IHeapHandle<TVal, TPr>> GetEnumerator()
        {
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var tp in _treePool)
            {
                foreach (var t in tp.BreadthFirstVisit())
                {
                    yield return t.Handle;
                }
            }
            // ReSharper restore LoopCanBeConvertedToQuery
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
            var other = otherHeap as FibonacciHeap<TVal, TPr>;
            if (other == null)
            {
                this.CommonMerge(otherHeap);
                return;
            }
            other.Version.Id = Version.Id; // Updates all other nodes version
            _treePool.Append(other._treePool);
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
            RemoveTree(treeHandle.Tree);
            return true;
        }

        public IHeapHandle<TVal, TPr> RemoveMin()
        {
            var min = MinTree;
            RemoveTree(min);
            return min.Handle;
        }

        public IEnumerable<IReadOnlyTree<TVal, TPr>> ToReadOnlyForest()
        {
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var tp in _treePool)
            {
                yield return tp.ToReadOnlyTree();
            }
            // ReSharper restore LoopCanBeConvertedToQuery
        }

        public override string ToString()
        {
            return this.CommonToString();
        }

        protected override void MoveDown(TreeHandle handle)
        {
            var tree = handle.Tree;
            var initialTree = tree;
            var child = tree.MinChild(Cmp);
            while (child != null && Cmp(child.Priority, tree.Priority) < 0)
            {
                tree.SwapRootWith(child);
                tree = child;
                child = tree.MinChild(Cmp);
            }
            if (ReferenceEquals(MinTree, initialTree))
            {
                MergeSameOrderTrees();
            }
        }

        protected override void MoveUp(TreeHandle handle)
        {
            var tree = handle.Tree;
            FixMin(tree);
            var parent = tree.Parent;
            if (parent == null || Cmp(parent.Priority, tree.Priority) <= 0)
            {
                return;
            }
            while (true)
            {
                parent.Children.Remove(tree);
                AppendToPool(tree);
                if (parent.Parent != null)
                {
                    parent.Mark++;
                }
                if (parent.Mark != FullMark)
                {
                    return;
                }
                parent.Mark = EmptyMark;
                tree = parent;
                parent = tree.Parent;
            }
        }

        private void AppendToPool(Tree tree)
        {
            tree.Parent = null;
            tree.Mark = EmptyMark;
            _treePool.AddLast(tree);
        }

        private void AppendToPool(SinglyLinkedList<Tree> trees)
        {
            foreach (var tree in trees)
            {
                tree.Parent = null;
                tree.Mark = EmptyMark;
            }
            _treePool.Append(trees);
        }

        private void FixMin(Tree tree)
        {
            if (MinTree == null || Cmp(tree.Priority, MinTree.Priority) < 0)
            {
                MinTree = tree;
            }
        }

        private void MergeSameOrderTrees()
        {
            if (_treePool.Count <= 1)
            {
                if (_treePool.Count == 0)
                {
                    return;
                }
                MinTree = _treePool.First;
                return;
            }

            var maxIndex = 0;
            foreach (var tree in _treePool)
            {
                var merged = tree;
                var mergedIndex = merged.Children.Count;
                Tree aux;
                while ((aux = _mergeBuffer[mergedIndex]) != null)
                {
                    merged = Meld(merged, aux);
                    _mergeBuffer[mergedIndex] = null;
                    mergedIndex = merged.Children.Count;
                }
                _mergeBuffer[mergedIndex] = merged;
                if (mergedIndex > maxIndex)
                {
                    maxIndex = mergedIndex;
                }
            }

            _treePool.Clear();
            MinTree = null;
            for (var i = 0; i <= maxIndex; ++i)
            {
                var tree = _mergeBuffer[i];
                if (tree == null)
                {
                    continue;
                }
                _treePool.AddLast(tree);
                FixMin(tree);
                _mergeBuffer[i] = null;
            }
        }

        private void RemoveTree(Tree tree)
        {
            for (var p = tree.Parent; p != null; tree = p, p = p.Parent)
            {
                tree.SwapRootWith(p);
            }
            _treePool.Remove(tree);
            AppendToPool(tree.Children);
            if (ReferenceEquals(tree, MinTree))
            {
                MergeSameOrderTrees();
            }
            Count--;
            tree.Handle.Version = null;
        }
    }
}