// 
// StableHeap.cs
//  
// Author:
//       Alessio Parma <alessio.parma@gmail.com>
// 
// Copyright (c) 2012-2014 Alessio Parma <alessio.parma@gmail.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Hippie
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Core;

    public sealed class StableHeap<TVal, TPr> : IStableRawHeap<TVal, TPr>
    {
        readonly IComparer<TPr> _comparer;
        readonly IRawHeap<TVal, IVersionedPriority<TPr>> _wrappedHeap;

        internal StableHeap(IRawHeap<TVal, IVersionedPriority<TPr>> wrappedHeap, IComparer<TPr> comparer,
                            long initialVersion)
        {
            _wrappedHeap = wrappedHeap;
            _comparer = comparer;
            NextVersion = initialVersion;
        }

        public IComparer<TPr> Comparer
        {
            get { return _comparer; }
        }

        public int Count
        {
            get { return _wrappedHeap.Count; }
        }

        public IEqualityComparer<TVal> EqualityComparer
        {
            get { return _wrappedHeap.EqualityComparer; }
        }

        public bool IsReadOnly
        {
            get { return _wrappedHeap.IsReadOnly; }
        }

        public IHeapHandle<TVal, IVersionedPriority<TPr>> Min
        {
            get { return _wrappedHeap.Min; }
        }

        public long NextVersion { get; private set; }

        public TPr this[IHeapHandle<TVal, IVersionedPriority<TPr>> handle]
        {
            set { _wrappedHeap[handle] = new VersionedPriority<TPr>(value, NextVersion++); }
        }

        public IHeapHandle<TVal, IVersionedPriority<TPr>> Add(TVal val, TPr priority)
        {
            return _wrappedHeap.Add(val, new VersionedPriority<TPr>(priority, NextVersion++));
        }

        public IHeapHandle<TVal, IVersionedPriority<TPr>> Add(TVal val, TPr priority, long version)
        {
            return _wrappedHeap.Add(val, new VersionedPriority<TPr>(priority, version));
        }

        void IThinHeap<TVal, TPr>.Add(TVal val, TPr priority)
        {
            _wrappedHeap.Add(val, new VersionedPriority<TPr>(priority, NextVersion++));
        }

        void IStableThinHeap<TVal, TPr>.Add(TVal val, TPr priority, long version)
        {
            _wrappedHeap.Add(val, new VersionedPriority<TPr>(priority, version));
        }

        public void Add(IHeapHandle<TVal, TPr> item)
        {
            _wrappedHeap.Add(item.Value, new VersionedPriority<TPr>(item.Priority, NextVersion++));
        }

        public void Clear()
        {
            _wrappedHeap.Clear();
        }

        public Boolean Contains(IHeapHandle<TVal, IVersionedPriority<TPr>> handle)
        {
            return _wrappedHeap.Contains(handle);
        }

        public IEnumerator<IHeapHandle<TVal, IVersionedPriority<TPr>>> GetEnumerator()
        {
            return _wrappedHeap.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Merge<TVal2, TPr2>(IThinHeap<TVal2, TPr2> other) where TVal2 : TVal where TPr2 : TPr
        {
            this.CommonMerge(other);
        }

        public Boolean Remove(IHeapHandle<TVal, IVersionedPriority<TPr>> handle)
        {
            return _wrappedHeap.Remove(handle);
        }

        public IHeapHandle<TVal, IVersionedPriority<TPr>> RemoveMin()
        {
            return _wrappedHeap.RemoveMin();
        }

        public IVersionedPriority<TPr> UpdatePriorityOf(IHeapHandle<TVal, IVersionedPriority<TPr>> handle,
                                                        TPr newPriority)
        {
            var newStablePriority = new VersionedPriority<TPr>(newPriority, NextVersion++);
            return _wrappedHeap.UpdatePriorityOf(handle, newStablePriority);
        }

        public IVersionedPriority<TPr> UpdatePriorityOf(IHeapHandle<TVal, IVersionedPriority<TPr>> handle,
                                                        TPr newPriority, long newVersion)
        {
            var newStablePriority = new VersionedPriority<TPr>(newPriority, newVersion);
            return _wrappedHeap.UpdatePriorityOf(handle, newStablePriority);
        }

        public TVal UpdateValue(IHeapHandle<TVal, IVersionedPriority<TPr>> handle, TVal newValue)
        {
            return _wrappedHeap.UpdateValue(handle, newValue);
        }

        public IEnumerable<IReadOnlyTree<TVal, TPr>> ToReadOnlyForest()
        {
            return _wrappedHeap.CommonToForest<TVal, IVersionedPriority<TPr>, TVal, TPr>(TransformTree);
        }

        public override string ToString()
        {
            return _wrappedHeap.ToString();
        }

        static ReadOnlyTree<TVal, TPr> TransformTree(IReadOnlyTree<TVal, IVersionedPriority<TPr>> tree,
                                                     ReadOnlyTree<TVal, TPr> parent)
        {
            return new ReadOnlyTree<TVal, TPr>(tree.Value, tree.Priority.Value, parent);
        }

        #region IRawHeap Members

        IHeapHandle<TVal, TPr> IThinHeap<TVal, TPr>.Min
        {
            get { return new StableHandle<TVal, TPr>(_wrappedHeap.Min); }
        }

        public TPr this[IHeapHandle<TVal, TPr> handle]
        {
            set { _wrappedHeap[UnwrapHandle(handle)] = new VersionedPriority<TPr>(value, NextVersion++); }
        }

        IHeapHandle<TVal, TPr> IRawHeap<TVal, TPr>.Add(TVal val, TPr priority)
        {
            var stablePriority = new VersionedPriority<TPr>(priority, NextVersion++);
            return new StableHandle<TVal, TPr>(_wrappedHeap.Add(val, stablePriority));
        }

        public bool Contains(IHeapHandle<TVal, TPr> handle)
        {
            return _wrappedHeap.Contains(UnwrapHandle(handle));
        }

        public void CopyTo(IHeapHandle<TVal, TPr>[] array, int idx)
        {
            _wrappedHeap.CommonCopyTo(array, idx, h => new StableHandle<TVal, TPr>(h));
        }

        IEnumerator<IHeapHandle<TVal, TPr>> IEnumerable<IHeapHandle<TVal, TPr>>.GetEnumerator()
        {
// ReSharper disable LoopCanBeConvertedToQuery
            foreach (var handle in _wrappedHeap) {
                yield return new StableHandle<TVal, TPr>(handle);
            }
// ReSharper restore LoopCanBeConvertedToQuery
        }

        public Boolean Remove(IHeapHandle<TVal, TPr> handle)
        {
            return _wrappedHeap.Remove(UnwrapHandle(handle));
        }

        IHeapHandle<TVal, TPr> IThinHeap<TVal, TPr>.RemoveMin()
        {
            return new StableHandle<TVal, TPr>(_wrappedHeap.RemoveMin());
        }

        public TPr UpdatePriorityOf(IHeapHandle<TVal, TPr> handle, TPr newPriority)
        {
            var newStablePriority = new VersionedPriority<TPr>(newPriority, NextVersion++);
            return _wrappedHeap.UpdatePriorityOf(UnwrapHandle(handle), newStablePriority).Value;
        }

        public TVal UpdateValue(IHeapHandle<TVal, TPr> handle, TVal newValue)
        {
            return _wrappedHeap.UpdateValue(UnwrapHandle(handle), newValue);
        }

        static IHeapHandle<TVal, IVersionedPriority<TPr>> UnwrapHandle(IHeapHandle<TVal, TPr> wrapper)
        {
            Contract.Requires<ArgumentException>(wrapper is StableHandle<TVal, TPr>);
            Debug.Assert(wrapper is StableHandle<TVal, TPr>, "To keep R# quiet :)");
            return (wrapper as StableHandle<TVal, TPr>).Handle;
        }

        #endregion
    }
}