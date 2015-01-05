// BinomialHeap.cs
// 
// Author: Alessio Parma <alessio.parma@gmail.com>
// 
// Copyright (c) 2012-2014 Alessio Parma <alessio.parma@gmail.com>
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
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace DIBRIS.Hippie
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Core;

    public sealed class BinomialHeap<TVal, TPr> : TreeHeap<TVal, TPr>, IRawHeap<TVal, TPr>
    {
        private readonly Tree[] _addBuffer = new Tree[1];
        private readonly Tree[] _trees;
        private int _maxTreeIndex;

        internal BinomialHeap(IComparer<TPr> cmp)
            : base(cmp)
        {
            _trees = new Tree[(int) Math.Ceiling(Math.Log(int.MaxValue, 2))];
        }

        public IHeapHandle<TVal, TPr> Add(TVal value, TPr priority)
        {
            var tree = new Tree(value, priority, Version);
            // We have to momentarily put the new tree in an enumerable array, in order to be able
            // to use the MergeTrees procedure.
            _addBuffer[0] = tree;
            MergeTrees(_addBuffer, 1, 1);
            // It's better to clear the buffer, in order to avoid to keep unnecessary references.
            _addBuffer[0] = null;
            // Only roots can became heap minimum.
            if (tree.Parent == null)
            {
                FixMin(tree);
            }
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
            for (var i = 0; i < _trees.Length; ++i)
            {
                _trees[i] = null;
            }
            _maxTreeIndex = 0;
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
            for (var i = 0; i <= _maxTreeIndex; ++i)
            {
                var tree = _trees[i];
                if (tree == null)
                {
                    continue;
                }
                foreach (var t in tree.BreadthFirstVisit())
                {
                    yield return t.Handle;
                }
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
            var other = otherHeap as BinomialHeap<TVal, TPr>;
            if (other == null)
            {
                this.CommonMerge(otherHeap);
                return;
            }
            other.Version.Id = Version.Id; // Updates all other nodes version
            MergeTrees(other._trees, other._trees.Length, other.Count);
            // Only roots can became heap minimum.
            if (other.MinTree.Parent == null)
            {
                FixMin(other.MinTree);
            }
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
            for (var p = tree.Parent; p != null; tree = p, p = p.Parent)
            {
                tree.SwapRootWith(p);
            }
            foreach (var child in tree.Children)
            {
                child.Parent = null;
            }
            var treeOrder = tree.Children.Count;
            _trees[treeOrder] = null;
            MergeTrees(tree.Children, treeOrder, -1);
            if (ReferenceEquals(tree, MinTree))
            {
                FixMin();
            }
            tree.Handle.Version = null;
            return true;
        }

        public IHeapHandle<TVal, TPr> RemoveMin()
        {
            var min = MinTree;
            foreach (var child in min.Children)
            {
                child.Parent = null;
            }
            var treeOrder = min.Children.Count;
            _trees[treeOrder] = null;
            MergeTrees(min.Children, treeOrder, -1);
            FixMin();
            min.Handle.Version = null;
            return min.Handle;
        }

        public IEnumerable<IReadOnlyTree<TVal, TPr>> ToReadOnlyForest()
        {
            if (Count == 0)
            {
                yield break;
            }
            for (var i = 0; i <= _maxTreeIndex; ++i)
            {
                var tree = _trees[i];
                if (tree != null)
                {
                    yield return tree.ToReadOnlyTree();
                }
            }
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
                FixMin();
            }
        }

        protected override void MoveUp(TreeHandle handle)
        {
            var tree = handle.Tree;
            var parent = tree.Parent;
            while (parent != null && Cmp(tree.Priority, parent.Priority) < 0)
            {
                tree.SwapRootWith(parent);
                tree = parent;
                parent = parent.Parent;
            }
            if (parent == null)
            {
                FixMin(tree);
            }
        }

        private void FixMin()
        {
            MinTree = null;
            for (var i = 0; i <= _maxTreeIndex; ++i)
            {
                if (_trees[i] != null)
                {
                    FixMin(_trees[i]);
                }
            }
        }

        private void FixMin(Tree tree)
        {
            Debug.Assert(tree.Parent == null);
            if (MinTree == null || Cmp(tree.Priority, MinTree.Priority) <= 0)
            {
                MinTree = tree;
            }
        }

        /// <summary>
        ///   </summary>
        /// <param name="otherTrees"></param>
        /// <param name="otherTreesCount"></param>
        /// <param name="otherPairCount"></param>
        /// <remarks>This is a performance critical method.</remarks>
        private void MergeTrees(IEnumerable<Tree> otherTrees, int otherTreesCount, int otherPairCount)
        {
            // If Count is zero, we have nothing to do. Moreover, we do not want to do Math.Log(0)...
            if ((Count += otherPairCount) == 0)
            {
                return;
            }
            _maxTreeIndex = (int) Math.Log(Count, 2);

            Tree carry = null;
            var en = otherTrees.GetEnumerator();
            for (var i = 0; i < otherTreesCount; ++i)
            {
                en.MoveNext();
                var merged = NullMeld(en.Current, _trees[i]);
                if (merged == null || merged.Children.Count == i + 1)
                {
                    _trees[i] = carry;
                    carry = merged;
                }
                else if (carry == null)
                {
                    _trees[i] = merged;
                }
                else
                {
                    _trees[i] = null;
                    carry = Meld(merged, carry);
                }
            }

            if (carry == null)
            {
                return;
            }
            while (true)
            {
                var i = carry.Children.Count;
                carry = NullMeld(_trees[i], carry);
                if (carry.Children.Count == i)
                {
                    _trees[i] = carry;
                    break;
                }
                _trees[i] = null;
            }
        }
    }
}