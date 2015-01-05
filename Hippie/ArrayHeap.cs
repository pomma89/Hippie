// ArrayHeap.cs
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
    using System.Diagnostics.Contracts;
    using Core;
    using PommaLabs;

    public sealed class ArrayHeap<TVal, TPr> : RawHeap<TVal, TPr, ArrayHeap<TVal, TPr>.ArrayHandle>, IRawHeap<TVal, TPr>
    {
        /// <summary>
        ///   The index from which handles are stored.
        /// </summary>
        private const int MinIndex = 0;

        /// <summary>
        ///   The minimum size of the array containing the handles.
        /// </summary>
        private const int MinSize = 4;

        /// <summary>
        ///   The factor used to increment the size of the array containing the handles.
        /// </summary>
        private const int ResizeFactor = 2;

        /// <summary>
        ///   The maximum number of children each node can have.
        /// </summary>
        private readonly byte _cc;

        /// <summary>
        ///   The array into which handles are stored.
        /// </summary>
        private ArrayHandle[] _handles;

        internal ArrayHeap(Byte cc, IComparer<TPr> cmp)
            : base(cmp)
        {
            Contract.Requires<ArgumentOutOfRangeException>(cc >= 2, ErrorMessages.WrongChildCount);
            _cc = cc;
            _handles = new ArrayHandle[MinSize];
        }

        public IHeapHandle<TVal, TPr> Min
        {
            get { return _handles[MinIndex]; }
        }

        public IHeapHandle<TVal, TPr> Add(TVal value, TPr priority)
        {
            var handle = new ArrayHandle(value, priority);
            Add(handle);
            return handle;
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
            Count = 0;
            _handles = new ArrayHandle[MinSize];
        }

        public void CopyTo(IHeapHandle<TVal, TPr>[] array, int idx)
        {
            this.CommonCopyTo(array, idx, h => h);
        }

        public IEnumerator<IHeapHandle<TVal, TPr>> GetEnumerator()
        {
            // ReSharper disable LoopCanBeConvertedToQuery ReSharper disable ForCanBeConvertedToForeach
            for (var i = MinIndex; i < Count; ++i)
            {
                yield return _handles[i];
            }
            // ReSharper restore ForCanBeConvertedToForeach ReSharper restore LoopCanBeConvertedToQuery
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
            var other = otherHeap as ArrayHeap<TVal, TPr>;
            if (other != null)
            {
                var otherHandles = other._handles;
                for (var i = MinIndex; i < other.Count; ++i)
                {
                    Add(otherHandles[i]);
                }
                other.Clear();
                return;
            }
            var other2 = otherHeap as ArrayHeap<TVal2, TPr2>;
            if (other2 != null)
            {
                var other2Handles = other2._handles;
                for (var i = MinIndex; i < other2.Count; ++i)
                {
                    Add(ArrayHandle.New(other2Handles[i]));
                }
                other2.Clear();
                return;
            }
            while (otherHeap.Count != 0)
            {
                Add(ArrayHandle.New(otherHeap.RemoveMin()));
            }
        }

        public bool Remove(IHeapHandle<TVal, TPr> handle)
        {
            var arrayHandle = GetHandle(handle as ArrayHandle);
            if (arrayHandle != null)
            {
                RemoveAt(arrayHandle.Index);
                return true;
            }
            return false;
        }

        public IHeapHandle<TVal, TPr> RemoveMin()
        {
            var min = Min;
            RemoveAt(MinIndex);
            return min;
        }

        public IEnumerable<IReadOnlyTree<TVal, TPr>> ToReadOnlyForest()
        {
            if (Count == 0)
            {
                yield break;
            }
            var queue = new Queue<GPair<ArrayHandle, ReadOnlyTree<TVal, TPr>>>();
            queue.Enqueue(GPair.Create(_handles[MinIndex], (ReadOnlyTree<TVal, TPr>) null));
            ReadOnlyTree<TVal, TPr> root = null;
            while (queue.Count > 0)
            {
                var vi = queue.Dequeue();
                var it = vi.First;
                var t = new ReadOnlyTree<TVal, TPr>(it.Value, it.Priority, vi.Second);
                if (it.Index == MinIndex)
                {
                    root = t;
                }
                var start = _cc * it.Index + 1;
                for (var i = 0; i < _cc && i + start < Count; ++i)
                {
                    queue.Enqueue(GPair.Create(_handles[start + i], t));
                }
            }
            yield return root;
        }

        public override string ToString()
        {
            return this.CommonToString();
        }

        protected override ArrayHandle GetHandle(ArrayHandle handle)
        {
            if (handle == null)
            {
                return null;
            }
            var i = handle.Index;
            if (i >= Count || !ReferenceEquals(handle, _handles[i]))
            {
                return null;
            }
            return handle;
        }

        protected override void MoveDown(ArrayHandle handle)
        {
            var i = handle.Index;

            for (int j = _cc * i + 1, k = j + _cc; j < Count; j = _cc * i + 1, k = j + _cc)
            {
                var mc = j; // Min child index
                for (++j; j < Count && j < k; ++j)
                {
                    if (Cmp(_handles[j].Priority, _handles[mc].Priority) < 0)
                    {
                        mc = j;
                    }
                }
                if (Cmp(handle.Priority, _handles[mc].Priority) <= 0)
                {
                    break;
                }
                PlaceAt(_handles[mc], i);
                i = mc;
            }
            PlaceAt(handle, i);
        }

        protected override void MoveUp(ArrayHandle handle)
        {
            var i = handle.Index;
            while (true)
            {
                if (i == MinIndex)
                {
                    break;
                }
                var j = (i - 1) / _cc;
                var parent = _handles[j];
                if (Cmp(handle.Priority, parent.Priority) >= 0)
                {
                    break;
                }
                PlaceAt(parent, i);
                i = j;
            }
            PlaceAt(handle, i);
        }

        private void Add(ArrayHandle handle)
        {
            if (Count == _handles.Length)
            {
                Array.Resize(ref _handles, Count * ResizeFactor);
            }
            PlaceAt(handle, Count++);
            MoveUp(handle);
            CheckHandlesSize();
        }

        [Conditional("DEBUG")]
        private void CheckHandlesSize()
        {
            if (Count <= MinSize)
            {
                Debug.Assert(_handles.Length == MinSize);
                return;
            }
            var expectedLog = Math.Log(_handles.Length, ResizeFactor);
            var foundLog = Math.Ceiling(Math.Log(Count, ResizeFactor));
            Debug.Assert(expectedLog.Equals(foundLog));
        }

        private void PlaceAt(ArrayHandle handle, int index)
        {
            Debug.Assert(index <= Count);
            handle.Index = index;
            _handles[index] = handle;
        }

        private void RemoveAt(int index)
        {
            var last = _handles[--Count];
            // If we have an empty heap, or we are deleting the last element, we should not swap any
            // element. In the first case, because there is nothing to swap; in the second case,
            // because it is useless and the capacity of the array might have gotten too little.
            if (Count != 0 && Count != index)
            {
                last.Index = index;
                _handles[index] = last;
                MoveDown(last);
            }
            if (Count >= MinSize && Count * ResizeFactor == _handles.Length)
            {
                Array.Resize(ref _handles, Count);
            }
            CheckHandlesSize();
        }

        public sealed class ArrayHandle : IHeapHandle<TVal, TPr>, IItem
        {
            public int Index;

            public ArrayHandle(TVal val, TPr pr)
            {
                Value = val;
                Priority = pr;
            }

            public TVal Value { get; set; }

            public TPr Priority { get; set; }

            public static ArrayHandle New<TVal2, TPr2>(IHeapHandle<TVal2, TPr2> handle)
                where TVal2 : TVal
                where TPr2 : TPr
            {
                return new ArrayHandle(handle.Value, handle.Priority);
            }

            public override string ToString()
            {
                return string.Format("[Value: {0}; Priority: {1}]", Value, Priority);
            }
        }
    }
}