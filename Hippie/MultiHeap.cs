// MultiHeap.cs
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Core;
    using PommaLabs.Collections;

    public sealed class MultiHeap<TVal> : IHeap<TVal>
    {
        private readonly IEqualityComparer<TVal> _eqCmp;
        private readonly Dictionary<TVal, LinkedStack<IHeapHandle<TVal, TVal>>> _map;
        private readonly IRawHeap<TVal, TVal> _wrappedHeap;

        internal MultiHeap(IRawHeap<TVal, TVal> heapToWrap, IEqualityComparer<TVal> eqCmp)
        {
            _wrappedHeap = heapToWrap;
            _eqCmp = eqCmp ?? EqualityComparer<TVal>.Default;
            _map = new Dictionary<TVal, LinkedStack<IHeapHandle<TVal, TVal>>>(_eqCmp);
        }

        public IComparer<TVal> Comparer
        {
            get { return _wrappedHeap.Comparer; }
        }

        public int Count
        {
            get { return _wrappedHeap.Count; }
        }

        public IEqualityComparer<TVal> EqualityComparer
        {
            get { return _eqCmp; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public TVal Min
        {
            get { return _wrappedHeap.Min.Value; }
        }

        public void Add(TVal val)
        {
            var handle = _wrappedHeap.Add(val, val);
            LinkedStack<IHeapHandle<TVal, TVal>> stack;
            if (_map.TryGetValue(val, out stack))
            {
                stack.Push(handle);
            }
            else
            {
                stack = new LinkedStack<IHeapHandle<TVal, TVal>>();
                stack.Push(handle);
                _map.Add(val, stack);
            }
        }

        public void Clear()
        {
            _map.Clear();
            _wrappedHeap.Clear();
        }

        public bool Contains(TVal val)
        {
            return _map.ContainsKey(val);
        }

        public void CopyTo(TVal[] array, int arrayIndex)
        {
            _wrappedHeap.CommonCopyTo(array, arrayIndex, h => h.Value);
        }

        public IEnumerator<TVal> GetEnumerator()
        {
            return _wrappedHeap.Select(p => p.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _wrappedHeap.GetEnumerator();
        }

        public void Merge<TVal2>(IHeap<TVal2> other) where TVal2 : TVal
        {
            if (ReferenceEquals(this, other) || other.Count == 0)
            {
                return;
            }
            foreach (var h in other)
            {
                var handle = _wrappedHeap.Add(h, h);
                LinkedStack<IHeapHandle<TVal, TVal>> stack;
                if (_map.TryGetValue(h, out stack))
                {
                    stack.Push(handle);
                }
                else
                {
                    stack = new LinkedStack<IHeapHandle<TVal, TVal>>();
                    stack.Push(handle);
                    _map.Add(h, stack);
                }
            }
            other.Clear();
        }

        public bool Remove(TVal val)
        {
            LinkedStack<IHeapHandle<TVal, TVal>> stack;
            if (_map.TryGetValue(val, out stack))
            {
                var hasRemoved = _wrappedHeap.Remove(stack.Pop());
                Debug.Assert(hasRemoved);
                if (stack.Count == 0)
                {
                    _map.Remove(val);
                }
                return true;
            }
            return false;
        }

        public TVal RemoveMin()
        {
            var item = _wrappedHeap.RemoveMin().Value;
            var stack = _map[item];
            stack.Pop();
            if (stack.Count == 0)
            {
                _map.Remove(item);
            }
            return item;
        }

        public IEnumerable<IReadOnlyTree<TVal>> ToReadOnlyForest()
        {
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var t in _wrappedHeap.ToReadOnlyForest())
            {
                var tt = t.BreadthFirstVisit<ReadOnlyTree<TVal>>(TransformTree, null);
                yield return tt.ToList()[0];
            }
            // ReSharper restore LoopCanBeConvertedToQuery
        }

        public override string ToString()
        {
            return _wrappedHeap.ToString();
        }

        private static ReadOnlyTree<TVal> TransformTree(IReadOnlyTree<TVal, TVal> tree, ReadOnlyTree<TVal> parent)
        {
            return new ReadOnlyTree<TVal>(tree.Priority, parent);
        }
    }
}