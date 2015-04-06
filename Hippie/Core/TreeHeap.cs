// TreeHeap.cs
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

namespace DIBRIS.Hippie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Collections;

    public abstract class TreeHeap<TVal, TPr> : RawHeap<TVal, TPr, TreeHeap<TVal, TPr>.TreeHandle>
    {
        internal Tree MinTree;

        /// <summary>
        ///   Used to give a "version" to each node. When the heap is cleared, the version is
        ///   updated, so that all nodes, even if they have a valid hint (reference to node), are
        ///   marked as invalid because of the old version they have.
        /// </summary>
        protected TreeVersion Version = new TreeVersion();

        internal TreeHeap(IComparer<TPr> cmp)
            : base(cmp)
        {
        }

        public IHeapHandle<TVal, TPr> Min
        {
            get { return MinTree.Handle; }
        }

        protected void ClearBase()
        {
            Version = new TreeVersion();
            Count = 0;
            MinTree = null;
        }

        protected override sealed TreeHandle GetHandle(TreeHandle handle)
        {
            return (handle == null || handle.Version == null || handle.Version.Id != Version.Id) ? null : handle;
        }

        internal Tree Meld(Tree t1, Tree t2)
        {
            // It is a mistake to meld the same trees.
            Debug.Assert(!ReferenceEquals(t1, t2));

            if (Cmp(t1.Priority, t2.Priority) < 0)
            {
                t2.Parent = t1;
                t1.Children.AddLast(t2);
                return t1;
            }
            t1.Parent = t2;
            t2.Children.AddLast(t1);
            return t2;
        }

        internal Tree NullMeld(Tree t1, Tree t2)
        {
            return (t1 == null) ? t2 : (t2 == null) ? t1 : Meld(t1, t2);
        }

        internal sealed class Tree
        {
            public readonly SinglyLinkedList<Tree> Children = new SinglyLinkedList<Tree>();
            public TreeHandle Handle;
            public short Mark; // Used by Fibonacci heap
            public Tree Parent;

            public Tree(TVal val, TPr pr, TreeVersion v)
            {
                Handle = new TreeHandle(val, pr, v, this);
            }

            public TPr Priority
            {
                get { return Handle.Priority; }
            }

            public IEnumerable<Tree> BreadthFirstVisit()
            {
                var queue = new Queue<Tree>();
                queue.Enqueue(this);
                while (queue.Count != 0)
                {
                    var tree = queue.Dequeue();
                    yield return tree;
                    foreach (var child in tree.Children)
                    {
                        queue.Enqueue(child);
                    }
                }
            }

            public Tree MinChild(Func<TPr, TPr, int> cmp)
            {
                if (Children.Count == 0)
                {
                    return null;
                }
                var min = Children.First;
                foreach (var child in Children)
                {
                    if (cmp(child.Priority, min.Priority) < 0)
                    {
                        min = child;
                    }
                }
                return min;
            }

            public void SwapRootWith(Tree other)
            {
                var tmp = other.Handle;
                Handle.Tree = other;
                other.Handle = Handle;
                tmp.Tree = this;
                Handle = tmp;
            }

            public IReadOnlyTree<TVal, TPr> ToReadOnlyTree()
            {
                var queue = new Queue<GPair<Tree, ReadOnlyTree<TVal, TPr>>>();
                queue.Enqueue(GPair.Create(this, (ReadOnlyTree<TVal, TPr>) null));
                ReadOnlyTree<TVal, TPr> root = null;
                while (queue.Count > 0)
                {
                    var vi = queue.Dequeue();
                    var it = vi.First;
                    var t = new ReadOnlyTree<TVal, TPr>(it.Handle.Value, it.Priority, vi.Second);
                    if (root == null)
                    {
                        root = t;
                    }
                    foreach (var c in it.Children)
                    {
                        queue.Enqueue(GPair.Create(c, t));
                    }
                }
                return root;
            }

            public override string ToString()
            {
                return string.Format("[Item: {0}; Children: {1}]", base.ToString(), Children);
            }
        }

        public sealed class TreeHandle : IHeapHandle<TVal, TPr>, IItem
        {
            internal Tree Tree;
            internal TreeVersion Version;

            internal TreeHandle(TVal val, TPr pr, TreeVersion version, Tree tree)
            {
                Value = val;
                Priority = pr;
                Version = version;
                Tree = tree;
            }

            public TVal Value { get; set; }

            public TPr Priority { get; set; }

            public override string ToString()
            {
                return string.Format("[Value: {0}; Priority: {1}]", Value, Priority);
            }
        }

        public sealed class TreeVersion
        {
            public object Id = new object();

            public override string ToString()
            {
                return string.Format("[Id: {0}]", Id);
            }
        }
    }
}