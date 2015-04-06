// ThinHeapContract.cs
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

namespace DIBRIS.Hippie.Contracts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Core;
    using DIBRIS.Hippie;

    [ContractClassFor(typeof(IRawHeap<,>))]
    internal abstract class RawHeapContract<TV, TP> : IRawHeap<TV, TP>
    {
        public TP this[IHeapHandle<TV, TP> handle]
        {
            set
            {
                // Priority should never be null to avoid exceptions while using comparers.
                Contract.Requires<ArgumentNullException>(!ReferenceEquals(value, null), ErrorMessages.NullPriority);
                Contract.Requires<ArgumentException>(Contains(handle), ErrorMessages.NoHandle);
                Contract.Ensures(Count == Contract.OldValue(Count));
                Contract.Ensures(Contains(handle));
                Contract.Ensures(Comparer.Compare(handle.Priority, value) == 0);
            }
        }

        IHeapHandle<TV, TP> IRawHeap<TV, TP>.Add(TV val, TP priority)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(priority, null), ErrorMessages.NullPriority);
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            Contract.Ensures(Contract.Result<IHeapHandle<TV, TP>>() != null);
            Contract.Ensures(Contains(Contract.Result<IHeapHandle<TV, TP>>()));
            Contract.Ensures(EqualityComparer.Equals(Contract.Result<IHeapHandle<TV, TP>>().Value, val));
            Contract.Ensures(Comparer.Compare(Contract.Result<IHeapHandle<TV, TP>>().Priority, priority) == 0);
            // ReSharper disable AssignNullToNotNullAttribute
            return default(IHeapHandle<TV, TP>);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        public TP UpdatePriorityOf(IHeapHandle<TV, TP> handle, TP newPriority)
        {
            // Priority should never be null to avoid exceptions while using comparers.
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(newPriority, null), ErrorMessages.NullPriority);
            Contract.Requires<ArgumentException>(Contains(handle), ErrorMessages.NoHandle);
            Contract.Ensures(Count == Contract.OldValue(Count));
            Contract.Ensures(!ReferenceEquals(Contract.Result<TP>(), null));
            Contract.Ensures(Contains(handle));
            Contract.Ensures(Comparer.Compare(handle.Priority, newPriority) == 0);
            return default(TP);
        }

        public TV UpdateValue(IHeapHandle<TV, TP> handle, TV newValue)
        {
            Contract.Requires<ArgumentException>(Contains(handle), ErrorMessages.NoHandle);
            Contract.Ensures(Count == Contract.OldValue(Count));
            Contract.Ensures(Contains(handle));
            Contract.Ensures(EqualityComparer.Equals(handle.Value, newValue));
            return default(TV);
        }

        #region IThinHeap Members

        public abstract int Count { get; }

        public abstract bool IsReadOnly { get; }

        public abstract void Add(IHeapHandle<TV, TP> item);

        public abstract void Clear();

        public abstract bool Contains(IHeapHandle<TV, TP> item);

        public abstract void CopyTo(IHeapHandle<TV, TP>[] array, int arrayIndex);

        public abstract IEnumerator<IHeapHandle<TV, TP>> GetEnumerator();

        public abstract bool Remove(IHeapHandle<TV, TP> item);

        public abstract IComparer<TP> Comparer { get; }

        public abstract IEqualityComparer<TV> EqualityComparer { get; }

        public abstract IHeapHandle<TV, TP> Min { get; }

        public abstract void Add(TV val, TP priority);

        public abstract void Merge<TVal2, TPr2>(IThinHeap<TVal2, TPr2> other)
            where TVal2 : TV
            where TPr2 : TP;

        public abstract IHeapHandle<TV, TP> RemoveMin();

        public abstract IEnumerable<IReadOnlyTree<TV, TP>> ToReadOnlyForest();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IThinHeap Members
    }

    [ContractClassFor(typeof(IStableRawHeap<,>))]
    internal abstract class StableRawHeapContract<TV, TP> : IStableRawHeap<TV, TP>
    {
        TP IStableRawHeap<TV, TP>.this[IHeapHandle<TV, IVersionedPriority<TP>> handle]
        {
            set
            {
                // Priority should never be null to avoid exceptions while using comparers.
                Contract.Requires<ArgumentNullException>(!ReferenceEquals(value, null), ErrorMessages.NullPriority);
                Contract.Requires<ArgumentException>(Contains(handle));
                Contract.Ensures(Count == Contract.OldValue(Count));
                Contract.Ensures(Contains(handle));
                Contract.Ensures(Comparer.Compare(handle.Priority.Value, value) == 0);
            }
        }

        IHeapHandle<TV, IVersionedPriority<TP>> IStableRawHeap<TV, TP>.Add(TV val, TP priority, long version)
        {
            // Priority should never be null to avoid exceptions while using comparers.
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(priority, null), ErrorMessages.NullPriority);
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            Contract.Ensures(Contract.Result<IHeapHandle<TV, IVersionedPriority<TP>>>() != null);
            Contract.Ensures(Contains(Contract.Result<IHeapHandle<TV, IVersionedPriority<TP>>>()));
            Contract.Ensures(EqualityComparer.Equals(Contract.Result<IHeapHandle<TV, IVersionedPriority<TP>>>().Value,
                                                     val));
            Contract.Ensures(
                Comparer.Compare(Contract.Result<IHeapHandle<TV, IVersionedPriority<TP>>>().Priority.Value, priority) ==
                0);
            Contract.Ensures(Contract.Result<IHeapHandle<TV, IVersionedPriority<TP>>>().Priority.Version == version);
            // ReSharper disable AssignNullToNotNullAttribute
            return default(IHeapHandle<TV, IVersionedPriority<TP>>);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        IHeapHandle<TV, IVersionedPriority<TP>> IStableRawHeap<TV, TP>.Add(TV val, TP priority)
        {
            // Priority should never be null to avoid exceptions while using comparers.
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(priority, null), ErrorMessages.NullPriority);
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            Contract.Ensures(Contract.Result<IHeapHandle<TV, IVersionedPriority<TP>>>() != null);
            Contract.Ensures(Contains(Contract.Result<IHeapHandle<TV, IVersionedPriority<TP>>>()));
            Contract.Ensures(EqualityComparer.Equals(Contract.Result<IHeapHandle<TV, IVersionedPriority<TP>>>().Value,
                                                     val));
            Contract.Ensures(
                Comparer.Compare(Contract.Result<IHeapHandle<TV, IVersionedPriority<TP>>>().Priority.Value, priority) ==
                0);
            Contract.Ensures(Contract.Result<IHeapHandle<TV, IVersionedPriority<TP>>>().Priority.Version ==
                             Contract.OldValue(NextVersion));
            // ReSharper disable AssignNullToNotNullAttribute
            return default(IHeapHandle<TV, IVersionedPriority<TP>>);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        public bool Contains(IHeapHandle<TV, IVersionedPriority<TP>> handle)
        {
            Contract.Ensures(Count == Contract.OldValue(Count));
            return default(bool);
        }

        IEnumerator<IHeapHandle<TV, IVersionedPriority<TP>>> IStableRawHeap<TV, TP>.GetEnumerator()
        {
            Contract.Ensures(Contract.Result<IEnumerator<IHeapHandle<TV, IVersionedPriority<TP>>>>() != null);
            // ReSharper disable AssignNullToNotNullAttribute
            return default(IEnumerator<IHeapHandle<TV, IVersionedPriority<TP>>>);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        public bool Remove(IHeapHandle<TV, IVersionedPriority<TP>> handle)
        {
            Contract.Ensures(Count ==
                             (Contract.Result<bool>() ? Contract.OldValue(Count) - 1 : Contract.OldValue(Count)));
            Contract.Ensures(!Contains(handle));
            return default(bool);
        }

        public IVersionedPriority<TP> UpdatePriorityOf(IHeapHandle<TV, IVersionedPriority<TP>> handle, TP newPriority)
        {
            // Priority should never be null to avoid exceptions while using comparers.
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(newPriority, null), ErrorMessages.NullPriority);
            Contract.Requires<ArgumentException>(Contains(handle), ErrorMessages.NoHandle);
            Contract.Ensures(Count == Contract.OldValue(Count));
            Contract.Ensures(Contract.Result<IVersionedPriority<TP>>() != null);
            Contract.Ensures(Contains(handle));
            Contract.Ensures(Comparer.Compare(handle.Priority.Value, newPriority) == 0);
            Contract.Ensures(handle.Priority.Version == Contract.OldValue(NextVersion));
            // ReSharper disable AssignNullToNotNullAttribute
            return default(IVersionedPriority<TP>);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        public IVersionedPriority<TP> UpdatePriorityOf(IHeapHandle<TV, IVersionedPriority<TP>> handle, TP newPriority,
                                                       long newVersion)
        {
            // Priority should never be null to avoid exceptions while using comparers.
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(newPriority, null), ErrorMessages.NullPriority);
            Contract.Requires<ArgumentException>(Contains(handle), ErrorMessages.NoHandle);
            Contract.Ensures(Count == Contract.OldValue(Count));
            Contract.Ensures(Contract.Result<IVersionedPriority<TP>>() != null);
            Contract.Ensures(Contains(handle));
            Contract.Ensures(Comparer.Compare(handle.Priority.Value, newPriority) == 0);
            Contract.Ensures(handle.Priority.Version == newVersion);
            // ReSharper disable AssignNullToNotNullAttribute
            return default(IVersionedPriority<TP>);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        public TV UpdateValue(IHeapHandle<TV, IVersionedPriority<TP>> handle, TV newValue)
        {
            Contract.Requires<ArgumentException>(Contains(handle), ErrorMessages.NoHandle);
            Contract.Ensures(Count == Contract.OldValue(Count));
            Contract.Ensures(Contains(handle));
            Contract.Ensures(EqualityComparer.Equals(handle.Value, newValue));
            return default(TV);
        }

        #region IRawHeap and IStableThinHeap Members

        IEnumerator<IHeapHandle<TV, TP>> IEnumerable<IHeapHandle<TV, TP>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public abstract int Count { get; }

        public abstract bool IsReadOnly { get; }

        public abstract void Add(IHeapHandle<TV, TP> item);

        public abstract void Clear();

        public abstract bool Contains(IHeapHandle<TV, TP> item);

        public abstract void CopyTo(IHeapHandle<TV, TP>[] array, int arrayIndex);

        public abstract bool Remove(IHeapHandle<TV, TP> item);

        public abstract IComparer<TP> Comparer { get; }

        public abstract IEqualityComparer<TV> EqualityComparer { get; }

        IHeapHandle<TV, IVersionedPriority<TP>> IStableThinHeap<TV, TP>.Min
        {
            get { throw new NotImplementedException(); }
        }

        public abstract long NextVersion { get; }

        IHeapHandle<TV, TP> IThinHeap<TV, TP>.Min
        {
            get { throw new NotImplementedException(); }
        }

        TP IRawHeap<TV, TP>.this[IHeapHandle<TV, TP> handle]
        {
            set { throw new NotImplementedException(); }
        }

        IHeapHandle<TV, IVersionedPriority<TP>> IStableThinHeap<TV, TP>.RemoveMin()
        {
            throw new NotImplementedException();
        }

        public abstract void Add(TV val, TP priority);

        public abstract void Merge<TVal2, TPr2>(IThinHeap<TVal2, TPr2> other)
            where TVal2 : TV
            where TPr2 : TP;

        public abstract IHeapHandle<TV, TP> RemoveMin();

        public abstract IEnumerable<IReadOnlyTree<TV, TP>> ToReadOnlyForest();

        public abstract void Add(TV value, TP priority, long version);

        IHeapHandle<TV, TP> IRawHeap<TV, TP>.Add(TV val, TP priority)
        {
            throw new NotImplementedException();
        }

        public abstract TP UpdatePriorityOf(IHeapHandle<TV, TP> handle, TP newPriority);

        public abstract TV UpdateValue(IHeapHandle<TV, TP> handle, TV newValue);

        #endregion IRawHeap and IStableThinHeap Members
    }
}