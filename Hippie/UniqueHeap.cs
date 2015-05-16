// UniqueHeap.cs
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
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace DIBRIS.Hippie
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Core;

    public sealed class UniqueHeap<TVal, TPr> : IHeap<TVal, TPr>
    {
        private readonly Func<TPr, TPr, int> _cmp;
        private readonly IEqualityComparer<TVal> _eqCmp;
        private readonly Dictionary<TVal, IHeapHandle<TVal, TPr>> _map;
        private readonly IRawHeap<TVal, TPr> _wrappedHeap;

        internal UniqueHeap(IRawHeap<TVal, TPr> heapToWrap, IEqualityComparer<TVal> eqCmp)
        {
            _cmp = heapToWrap.Comparer.Compare;
            _eqCmp = eqCmp ?? EqualityComparer<TVal>.Default;
            _wrappedHeap = heapToWrap;
            _map = new Dictionary<TVal, IHeapHandle<TVal, TPr>>(_eqCmp);
        }

        public IComparer<TPr> Comparer
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

        public IHeapHandle<TVal, TPr> Min
        {
            get { return _wrappedHeap.Min; }
        }

        public TPr this[TVal val]
        {
            get { return _map[val].Priority; }
            set { _wrappedHeap.UpdatePriorityOf(_map[val], value); }
        }

        public void Add(TVal val, TPr priority)
        {
            _map.Add(val, _wrappedHeap.Add(val, priority));
        }

        public void Add(IHeapHandle<TVal, TPr> handle)
        {
            _map.Add(handle.Value, _wrappedHeap.Add(handle.Value, handle.Priority));
        }

        public void Clear()
        {
            _wrappedHeap.Clear();
            _map.Clear();
        }

        public bool Contains(TVal value)
        {
            return _map.ContainsKey(value);
        }

        public bool Contains(TVal value, TPr priority)
        {
            if (ReferenceEquals(priority, null))
            {
                // There should be no handle with null priority.
                return false;
            }
            IHeapHandle<TVal, TPr> handle;
            if (_map.TryGetValue(value, out handle))
            {
                return _cmp(handle.Priority, priority) == 0;
            }
            return false;
        }

        public bool Contains(IHeapHandle<TVal, TPr> handle)
        {
            if (ReferenceEquals(handle, null) || ReferenceEquals(handle.Priority, null))
            {
                // Handle should not be null and there should be no handle with null priority.
                return false;
            }
            IHeapHandle<TVal, TPr> tmp;
            if (_map.TryGetValue(handle.Value, out tmp))
            {
                return _cmp(handle.Priority, tmp.Priority) == 0;
            }
            return false;
        }

        public void CopyTo(IHeapHandle<TVal, TPr>[] array, int arrayIndex)
        {
            _wrappedHeap.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IHeapHandle<TVal, TPr>> GetEnumerator()
        {
            return _wrappedHeap.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _wrappedHeap.GetEnumerator();
        }

        public void Merge<TVal2, TPr2>(IThinHeap<TVal2, TPr2> other)
            where TVal2 : TVal
            where TPr2 : TPr
        {
            if (ReferenceEquals(this, other) || other.Count == 0)
            {
                return;
            }
            // Handles are cached to avoid enumerating other heap two times (it may be very costly,
            // in particular for tree heaps).
            var tmp = new List<IHeapHandle<TVal2, TPr2>>(other.Count);
            foreach (var h in other)
            {
                if (_map.ContainsKey(h.Value))
                {
                    throw new ArgumentException(ErrorMessages.MergeConflict);
                }
                tmp.Add(h);
            }
            foreach (var h in tmp)
            {
                _map.Add(h.Value, _wrappedHeap.Add(h.Value, h.Priority));
            }
            other.Clear();
        }

        public TPr PriorityOf(TVal value)
        {
            return _map[value].Priority;
        }

        public IHeapHandle<TVal, TPr> Remove(TVal value)
        {
            var handle = _map[value];
            _map.Remove(value);
            var hasRemoved = _wrappedHeap.Remove(handle);
            Debug.Assert(hasRemoved);
            return handle;
        }

        public bool Remove(IHeapHandle<TVal, TPr> handle)
        {
            IHeapHandle<TVal, TPr> tmp;
            if (_map.TryGetValue(handle.Value, out tmp))
            {
                if (_cmp(handle.Priority, tmp.Priority) == 0)
                {
                    _map.Remove(handle.Value);
                    var hasRemoved = _wrappedHeap.Remove(tmp);
                    Debug.Assert(hasRemoved);
                    return true;
                }
                return false;
            }
            return false;
        }

        public IHeapHandle<TVal, TPr> RemoveMin()
        {
            var min = _wrappedHeap.RemoveMin();
            _map.Remove(min.Value);
            return min;
        }

        public IEnumerable<IReadOnlyTree<TVal, TPr>> ToReadOnlyForest()
        {
            return _wrappedHeap.CommonToForest<TVal, TPr, TVal, TPr>(TransformTree);
        }

        public TPr Update(TVal value, TVal newValue, TPr newPriority)
        {
            var oldPriority = UpdatePriorityOf(value, newPriority);
            UpdateValue(value, newValue);
            return oldPriority;
        }

        public TPr UpdatePriorityOf(TVal value, TPr newPriority)
        {
            return _wrappedHeap.UpdatePriorityOf(_map[value], newPriority);
        }

        public void UpdateValue(TVal value, TVal newValue)
        {
            if (EqualityComparer.Equals(value, newValue))
            {
                return;
            }
            var handle = _map[value];
            _wrappedHeap.UpdateValue(handle, newValue);
            _map.Remove(value);
            _map.Add(newValue, handle);
        }

        public override string ToString()
        {
            return _wrappedHeap.ToString();
        }

        private static ReadOnlyTree<TVal, TPr> TransformTree(IReadOnlyTree<TVal, TPr> tree, ReadOnlyTree<TVal, TPr> parent)
        {
            return new ReadOnlyTree<TVal, TPr>(tree.Value, tree.Priority, parent);
        }
    }
}