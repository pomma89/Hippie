// ThinHeap.cs
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
    using System.Linq;
    using Core;
    using PommaLabs.CodeServices.Common;

    public sealed class ThinHeap<TVal, TPr> : IThinHeap<TVal, TPr>
    {
        /// <summary>
        ///   The index from which handles are stored.
        /// </summary>
        private const int MinIndex = 0;

        /// <summary>
        ///   The minimum size of the array containing the handles.
        /// </summary>
        private const int MinSize = 8;

        /// <summary>
        ///   The factor used to increment the size of the array containing the handles.
        /// </summary>
        private const int IncreaseFactor = 2;

        /// <summary>
        ///   The factor used to decrement the size of the array containing the handles.
        /// </summary>
        private const int DecreaseFactor = IncreaseFactor * IncreaseFactor;

        private readonly Func<TPr, TPr, int> _cmp;

        /// <summary>
        ///   The comparer used to compare the priority of items.
        /// </summary>
        private readonly IComparer<TPr> _comparer;

        /// <summary>
        ///   The comparer used to compare the equality of items.
        /// </summary>
        private readonly IEqualityComparer<TVal> _equalityComparer;

        /// <summary>
        ///   The array into which handles are stored.
        /// </summary>
        private Item[] _items;

        internal ThinHeap(IComparer<TPr> comparer, IEqualityComparer<TVal> equalityComparer)
        {
            _comparer = comparer;
            _cmp = comparer.Compare;
            _equalityComparer = equalityComparer;
            _items = new Item[MinSize];
        }

        public IComparer<TPr> Comparer
        {
            get { return _comparer; }
        }

        public int Count { get; private set; }

        public IEqualityComparer<TVal> EqualityComparer
        {
            get { return _equalityComparer; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IHeapHandle<TVal, TPr> Min
        {
            get { return _items[MinIndex]; }
        }

        public void Add(TVal value, TPr priority)
        {
            Add(new Item(value, priority));
        }

        public void Add(IHeapHandle<TVal, TPr> handle)
        {
            Add(handle.Value, handle.Priority);
        }

        public void Clear()
        {
            Count = 0;
            _items = new Item[MinSize];
        }

        public bool Contains(IHeapHandle<TVal, TPr> item)
        {
            return this.Any(i => ReferenceEquals(i, item));
        }

        public void CopyTo(IHeapHandle<TVal, TPr>[] array, int arrayIndex)
        {
            this.CommonCopyTo(array, arrayIndex, h => h);
        }

        public IEnumerator<IHeapHandle<TVal, TPr>> GetEnumerator()
        {
            // ReSharper disable LoopCanBeConvertedToQuery ReSharper disable ForCanBeConvertedToForeach
            for (var i = MinIndex; i < Count; ++i)
            {
                yield return _items[i];
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
            var other = otherHeap as ThinHeap<TVal, TPr>;
            if (other != null)
            {
                var otherItems = other._items;
                for (var i = MinIndex; i < other.Count; ++i)
                {
                    Add(otherItems[i]);
                }
                other.Clear();
                return;
            }
            var other2 = otherHeap as ThinHeap<TVal2, TPr2>;
            if (other2 != null)
            {
                var other2Items = other2._items;
                for (var i = MinIndex; i < other2.Count; ++i)
                {
                    var otherItem = other2Items[i];
                    Add(new Item(otherItem.Value, otherItem.Priority));
                }
                other2.Clear();
                return;
            }
            while (otherHeap.Count != 0)
            {
                var otherMin = otherHeap.RemoveMin();
                Add(new Item(otherMin.Value, otherMin.Priority));
            }
        }

        public bool Remove(IHeapHandle<TVal, TPr> item)
        {
            throw new NotImplementedException(ErrorMessages.InefficientMethod);
        }

        public IHeapHandle<TVal, TPr> RemoveMin()
        {
            var min = _items[MinIndex];
            var last = _items[--Count];

            // If we have an empty heap, we should not swap any element.
            if (Count == 0)
            {
                return min;
            }

            var idx = 0;
            int j;
            while ((j = 2 * idx + 1) < Count)
            {
                var mc = _items[j]; // Min child
                if (j + 1 < Count && _cmp(_items[j + 1].Priority, mc.Priority) < 0)
                {
                    mc = _items[++j];
                }
                if (_comparer.Compare(last.Priority, mc.Priority) <= 0)
                {
                    break;
                }
                _items[idx] = mc;
                idx = j;
            }
            _items[idx] = last;

            if (Count >= MinSize && Count * DecreaseFactor == _items.Length)
            {
                Array.Resize(ref _items, Count);
            }
            CheckHandlesSize();

            return min;
        }

        public IEnumerable<IReadOnlyTree<TVal, TPr>> ToReadOnlyForest()
        {
            if (Count == 0)
            {
                yield break;
            }
            var queue = new Queue<KeyValuePair<IndexedItem, ReadOnlyTree<TVal, TPr>>>();
            var indexedItem = new IndexedItem(_items[MinIndex], MinIndex);
            queue.Enqueue(KeyValuePair.Create(indexedItem, (ReadOnlyTree<TVal, TPr>) null));
            ReadOnlyTree<TVal, TPr> root = null;
            while (queue.Count > 0)
            {
                var vi = queue.Dequeue();
                var it = vi.Key;
                var t = new ReadOnlyTree<TVal, TPr>(it.Value, it.Priority, vi.Value);
                if (root == null)
                {
                    root = t;
                }
                var start = 2 * it.Index + 1;
                for (var i = 0; i < 2 && i + start < Count; ++i)
                {
                    var idx = start + i;
                    indexedItem = new IndexedItem(_items[idx], idx);
                    queue.Enqueue(KeyValuePair.Create(indexedItem, t));
                }
            }
            yield return root;
        }

        private void Add(Item item)
        {
            if (Count == _items.Length)
            {
                Array.Resize(ref _items, Count * IncreaseFactor);
            }
            var idx = Count++;
            CheckHandlesSize();

            while (true)
            {
                if (idx == MinIndex)
                {
                    break;
                }
                var j = (idx - 1) >> 1;
                var parent = _items[j];
                if (_cmp(item.Priority, parent.Priority) >= 0)
                {
                    break;
                }
                _items[idx] = parent;
                idx = j;
            }
            _items[idx] = item;
        }

        [Conditional("DEBUG")]
        private void CheckHandlesSize()
        {
            Debug.Assert(DecreaseFactor == IncreaseFactor * IncreaseFactor);
            if (Count <= MinSize)
            {
                Debug.Assert(_items.Length == MinSize || _items.Length == MinSize * 2);
                return;
            }
            var expectedLog = Math.Log(_items.Length, IncreaseFactor);
            var foundLog = Math.Ceiling(Math.Log(Count, IncreaseFactor));
            Debug.Assert(expectedLog.Equals(foundLog) || expectedLog.Equals(foundLog + 1));
        }

        private sealed class IndexedItem : IHeapHandle<TVal, TPr>
        {
            public readonly Int32 Index;
            private readonly Item _item;

            public IndexedItem(Item item, Int32 index)
            {
                _item = item;
                Index = index;
            }

            public TPr Priority
            {
                get { return _item.Priority; }
            }

            public TVal Value
            {
                get { return _item.Value; }
            }
        }

        private sealed class Item : IHeapHandle<TVal, TPr>
        {
            public readonly TPr Priority;
            public readonly TVal Value;

            public Item(TVal value, TPr priority)
            {
                Value = value;
                Priority = priority;
            }

            TPr IHeapHandle<TVal, TPr>.Priority
            {
                get { return Priority; }
            }

            TVal IHeapHandle<TVal, TPr>.Value
            {
                get { return Value; }
            }

            public override string ToString()
            {
                return string.Format("[Value: {0}; Priority: {1}]", Value, Priority);
            }
        }
    }

    public sealed class StableThinHeap<TVal, TPr> : IStableThinHeap<TVal, TPr>
    {
        private readonly IComparer<TPr> _comparer;
        private readonly IThinHeap<TVal, IVersionedPriority<TPr>> _wrappedHeap;

        internal StableThinHeap(IComparer<TPr> comparer, IEqualityComparer<TVal> equalityComparer, long initialVersion)
        {
            _comparer = comparer;
            var stableCmp = new StableComparer<TPr>(comparer.Compare);
            _wrappedHeap = new ThinHeap<TVal, IVersionedPriority<TPr>>(stableCmp, equalityComparer);
            NextVersion = initialVersion;
        }

        public long NextVersion { get; private set; }

        public IComparer<TPr> Comparer
        {
            get { return _comparer; }
        }

        public Int32 Count
        {
            get { return _wrappedHeap.Count; }
        }

        public IEqualityComparer<TVal> EqualityComparer
        {
            get { return _wrappedHeap.EqualityComparer; }
        }

        public Boolean IsReadOnly
        {
            get { return false; }
        }

        public IHeapHandle<TVal, IVersionedPriority<TPr>> Min
        {
            get { return _wrappedHeap.Min; }
        }

        IHeapHandle<TVal, TPr> IThinHeap<TVal, TPr>.Min
        {
            get { return new StableHandle<TVal, TPr>(_wrappedHeap.Min); }
        }

        public void Add(TVal value, TPr priority, long version)
        {
            _wrappedHeap.Add(value, new VersionedPriority<TPr>(priority, version));
        }

        public void Add(IHeapHandle<TVal, TPr> handle)
        {
            Add(handle.Value, handle.Priority, NextVersion++);
        }

        public void Add(TVal value, TPr priority)
        {
            Add(value, priority, NextVersion++);
        }

        public void Clear()
        {
            _wrappedHeap.Clear();
        }

        public bool Contains(IHeapHandle<TVal, TPr> item)
        {
            Debug.Assert(item is StableHandle<TVal, TPr>);
            var wrappedHandle = (item as StableHandle<TVal, TPr>).Handle;
            return _wrappedHeap.Any(i => ReferenceEquals(i, wrappedHandle));
        }

        public bool Contains(IHeapHandle<TVal, IVersionedPriority<TPr>> item)
        {
            return _wrappedHeap.Any(i => ReferenceEquals(i, item));
        }

        public void CopyTo(IHeapHandle<TVal, TPr>[] array, int arrayIndex)
        {
            this.CommonCopyTo(array, arrayIndex, h => h);
        }

        public IEnumerator<IHeapHandle<TVal, TPr>> GetEnumerator()
        {
            // ReSharper disable LoopCanBeConvertedToQuery ReSharper disable ForCanBeConvertedToForeach
            foreach (var handle in _wrappedHeap)
            {
                yield return new StableHandle<TVal, TPr>(handle);
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
            while (otherHeap.Count > 0)
            {
                var otherMin = otherHeap.RemoveMin();
                _wrappedHeap.Add(otherMin.Value, new VersionedPriority<TPr>(otherMin.Priority, NextVersion++));
            }
        }

        public bool Remove(IHeapHandle<TVal, TPr> item)
        {
            throw new NotImplementedException(ErrorMessages.InefficientMethod);
        }

        public IHeapHandle<TVal, IVersionedPriority<TPr>> RemoveMin()
        {
            return _wrappedHeap.RemoveMin();
        }

        IHeapHandle<TVal, TPr> IThinHeap<TVal, TPr>.RemoveMin()
        {
            return new StableHandle<TVal, TPr>(_wrappedHeap.RemoveMin());
        }

        public IEnumerable<IReadOnlyTree<TVal, TPr>> ToReadOnlyForest()
        {
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var t in _wrappedHeap.ToReadOnlyForest())
            {
                var tt = t.BreadthFirstVisit<ReadOnlyTree<TVal, TPr>>(TransformTree, null);
                yield return tt.ToList()[0];
            }
            // ReSharper restore LoopCanBeConvertedToQuery
        }

        private static ReadOnlyTree<TVal, TPr> TransformTree(IReadOnlyTree<TVal, IVersionedPriority<TPr>> tree,
                                                     ReadOnlyTree<TVal, TPr> parent)
        {
            return new ReadOnlyTree<TVal, TPr>(tree.Value, tree.Priority.Value, parent);
        }
    }

    internal sealed class StableComparer<TPr> : IComparer<IVersionedPriority<TPr>>
    {
        private readonly Func<TPr, TPr, int> _originalCmp;

        public StableComparer(Func<TPr, TPr, int> originalCmp)
        {
            _originalCmp = originalCmp;
        }

        public int Compare(IVersionedPriority<TPr> x, IVersionedPriority<TPr> y)
        {
            var cmp = _originalCmp(x.Value, y.Value);
            return (cmp != 0) ? cmp : x.Version.CompareTo(y.Version);
        }
    }

    internal sealed class StableHandle<TVal, TPr> : IHeapHandle<TVal, TPr>
    {
        public readonly IHeapHandle<TVal, IVersionedPriority<TPr>> Handle;

        public StableHandle(IHeapHandle<TVal, IVersionedPriority<TPr>> handle)
        {
            Handle = handle;
        }

        public TVal Value
        {
            get { return Handle.Value; }
        }

        public TPr Priority
        {
            get { return Handle.Priority.Value; }
        }

        public override string ToString()
        {
            return string.Format("[Value: {0}; Priority: {1}]", Value, Priority);
        }
    }

    internal sealed class VersionedPriority<TPr> : IVersionedPriority<TPr>
    {
        private readonly TPr _value;
        private readonly long _version;

        public VersionedPriority(TPr value, long version)
        {
            _value = value;
            _version = version;
        }

        public TPr Value
        {
            get { return _value; }
        }

        public long Version
        {
            get { return _version; }
        }

        public override string ToString()
        {
            return string.Format("[Value: {0}; Version: {1}]", _value, _version);
        }
    }
}