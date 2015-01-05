// ThinHeapContract.cs
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

namespace DIBRIS.Hippie.Contracts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Core;
    using DIBRIS.Hippie;

    [ContractClassFor(typeof(IThinHeap<,>))]
    internal abstract class ThinHeapContract<TVal, TPr> : IThinHeap<TVal, TPr>
    {
        public IComparer<TPr> Comparer
        {
            get
            {
                Contract.Ensures(Contract.Result<IComparer<TPr>>() != null);
                Contract.Ensures(Count == Contract.OldValue(Count));
                return Comparer<TPr>.Default;
            }
        }

        public IEqualityComparer<TVal> EqualityComparer
        {
            get
            {
                Contract.Ensures(Contract.Result<IEqualityComparer<TVal>>() != null);
                Contract.Ensures(Count == Contract.OldValue(Count));
                return EqualityComparer<TVal>.Default;
            }
        }

        public IHeapHandle<TVal, TPr> Min
        {
            get
            {
                Contract.Requires<InvalidOperationException>(Count > 0, ErrorMessages.NoMin);
                Contract.Ensures(Contract.Result<IHeapHandle<TVal, TPr>>() != null);
                Contract.Ensures(Count == Contract.OldValue(Count));
                Contract.Ensures(Contains(Contract.Result<IHeapHandle<TVal, TPr>>()));
                // ReSharper disable AssignNullToNotNullAttribute
                return default(IHeapHandle<TVal, TPr>);
                // ReSharper restore AssignNullToNotNullAttribute
            }
        }

        public void Add(TVal val, TPr priority)
        {
            // Priority should never be null to avoid exceptions while using comparers.
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(priority, null), ErrorMessages.NullPriority);
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
        }

        public void Merge<TVal2, TPr2>(IThinHeap<TVal2, TPr2> other)
            where TVal2 : TVal
            where TPr2 : TPr
        {
            Contract.Requires<ArgumentNullException>(other != null, ErrorMessages.NullOther);
            Contract.Requires<ArgumentException>(ReferenceEquals(Comparer, other.Comparer),
                                                 ErrorMessages.DifferentComparers);
            Contract.Ensures(ReferenceEquals(this, other) ||
                             Count == Contract.OldValue(Count) + Contract.OldValue(other.Count));
            Contract.Ensures(ReferenceEquals(this, other) || other.Count == 0);
        }

        public IHeapHandle<TVal, TPr> RemoveMin()
        {
            Contract.Requires<InvalidOperationException>(Count > 0, ErrorMessages.NoMin);
            Contract.Ensures(Count == Contract.OldValue(Count) - 1);
            Contract.Ensures(Contract.Result<IHeapHandle<TVal, TPr>>() != null);
            Contract.Ensures(!Contains(Contract.Result<IHeapHandle<TVal, TPr>>()));
            Contract.Ensures(EqualityComparer.Equals(Contract.Result<IHeapHandle<TVal, TPr>>().Value,
                                                     Contract.OldValue(Min.Value)));
            Contract.Ensures(
                Comparer.Compare(Contract.Result<IHeapHandle<TVal, TPr>>().Priority, Contract.OldValue(Min.Priority)) ==
                0);
            // ReSharper disable AssignNullToNotNullAttribute
            return default(IHeapHandle<TVal, TPr>);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        public IEnumerable<IReadOnlyTree<TVal, TPr>> ToReadOnlyForest()
        {
            Contract.Ensures(Contract.Result<IEnumerable<IReadOnlyTree<TVal, TPr>>>() != null);
            // ReSharper disable AssignNullToNotNullAttribute
            return default(IEnumerable<IReadOnlyTree<TVal, TPr>>);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #region ICollection Members

        public abstract int Count { get; }

        public abstract bool IsReadOnly { get; }

        public abstract void Add(IHeapHandle<TVal, TPr> item);

        public abstract void Clear();

        public abstract bool Contains(IHeapHandle<TVal, TPr> item);

        public abstract void CopyTo(IHeapHandle<TVal, TPr>[] array, int arrayIndex);

        public abstract IEnumerator<IHeapHandle<TVal, TPr>> GetEnumerator();

        public abstract bool Remove(IHeapHandle<TVal, TPr> item);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion ICollection Members
    }

    [ContractClassFor(typeof(IStableThinHeap<,>))]
    internal abstract class StableThinHeapContract<TVal, TPr> : IStableThinHeap<TVal, TPr>
    {
        IHeapHandle<TVal, IVersionedPriority<TPr>> IStableThinHeap<TVal, TPr>.Min
        {
            get
            {
                Contract.Requires<InvalidOperationException>(Count > 0, ErrorMessages.NoMin);
                Contract.Ensures(Contract.Result<IHeapHandle<TVal, IVersionedPriority<TPr>>>() != null);
                Contract.Ensures(Count == Contract.OldValue(Count));
                Contract.Ensures(Contains(Contract.Result<IHeapHandle<TVal, IVersionedPriority<TPr>>>()));
                // ReSharper disable AssignNullToNotNullAttribute
                return default(IHeapHandle<TVal, IVersionedPriority<TPr>>);
                // ReSharper restore AssignNullToNotNullAttribute
            }
        }

        public long NextVersion
        {
            get
            {
                Contract.Ensures(Count == Contract.OldValue(Count));
                return default(long);
            }
        }

        public void Add(TVal value, TPr priority, long version)
        {
            // Priority should never be null to avoid exceptions while using comparers.
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(priority, null), ErrorMessages.NullPriority);
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            Contract.Ensures(NextVersion == Contract.OldValue(NextVersion));
        }

        public bool Contains(IHeapHandle<TVal, IVersionedPriority<TPr>> handle)
        {
            Contract.Ensures(Count == Contract.OldValue(Count));
            return default(bool);
        }

        IHeapHandle<TVal, IVersionedPriority<TPr>> IStableThinHeap<TVal, TPr>.RemoveMin()
        {
            Contract.Requires<InvalidOperationException>(Count > 0, ErrorMessages.NoMin);
            Contract.Ensures(Count == Contract.OldValue(Count) - 1);
            Contract.Ensures(Contract.Result<IHeapHandle<TVal, IVersionedPriority<TPr>>>() != null);
            Contract.Ensures(!Contains(Contract.Result<IHeapHandle<TVal, IVersionedPriority<TPr>>>()));
            Contract.Ensures(EqualityComparer.Equals(
                Contract.Result<IHeapHandle<TVal, IVersionedPriority<TPr>>>().Value, Contract.OldValue(Min.Value)));
            Contract.Ensures(
                Comparer.Compare(Contract.Result<IHeapHandle<TVal, IVersionedPriority<TPr>>>().Priority.Value,
                                 Contract.OldValue(Min.Priority)) == 0);
            // ReSharper disable AssignNullToNotNullAttribute
            return default(IHeapHandle<TVal, IVersionedPriority<TPr>>);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #region IThinHeap Members

        public abstract int Count { get; }

        public abstract bool IsReadOnly { get; }

        public abstract void Add(IHeapHandle<TVal, TPr> item);

        public abstract void Clear();

        public abstract bool Contains(IHeapHandle<TVal, TPr> item);

        public abstract void CopyTo(IHeapHandle<TVal, TPr>[] array, int arrayIndex);

        public abstract IEnumerator<IHeapHandle<TVal, TPr>> GetEnumerator();

        public abstract bool Remove(IHeapHandle<TVal, TPr> item);

        public abstract IComparer<TPr> Comparer { get; }

        public abstract IEqualityComparer<TVal> EqualityComparer { get; }

        public abstract IHeapHandle<TVal, TPr> Min { get; }

        public abstract void Add(TVal val, TPr priority);

        public abstract void Merge<TVal2, TPr2>(IThinHeap<TVal2, TPr2> other)
            where TVal2 : TVal
            where TPr2 : TPr;

        public abstract IHeapHandle<TVal, TPr> RemoveMin();

        public abstract IEnumerable<IReadOnlyTree<TVal, TPr>> ToReadOnlyForest();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IThinHeap Members
    }
}