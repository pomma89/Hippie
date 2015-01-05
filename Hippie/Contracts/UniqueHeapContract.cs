// UniqueHeapContract.cs
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

    [ContractClassFor(typeof(IHeap<,>))]
    internal abstract class UniqueHeapContract<TVal, TPr> : IHeap<TVal, TPr>
    {
        public TPr this[TVal val]
        {
            get
            {
                Contract.Requires<ArgumentException>(Contains(val), ErrorMessages.NoValue);
                Contract.Ensures(Count == Contract.OldValue(Count));
                Contract.Ensures(!ReferenceEquals(Contract.Result<TPr>(), null));
                Contract.Ensures(Contains(val, Contract.Result<TPr>()));
                return default(TPr);
            }
            set
            {
                // Priority should never be null to avoid exceptions while using comparers.
                Contract.Requires<ArgumentNullException>(!ReferenceEquals(value, null), ErrorMessages.NullPriority);
                Contract.Requires<ArgumentException>(Contains(val), ErrorMessages.NoValue);
                Contract.Ensures(Count == Contract.OldValue(Count));
                Contract.Ensures(Contains(val, value));
                Contract.Ensures(Comparer.Compare(this[val], value) == 0);
            }
        }

        public bool Contains(TVal value)
        {
            Contract.Ensures(Count == Contract.OldValue(Count));
            return default(bool);
        }

        public bool Contains(TVal value, TPr priority)
        {
            Contract.Ensures(Count == Contract.OldValue(Count));
            return default(bool);
        }

        public TPr PriorityOf(TVal value)
        {
            Contract.Requires<ArgumentException>(Contains(value), ErrorMessages.NoValue);
            Contract.Ensures(Count == Contract.OldValue(Count));
            Contract.Ensures(!ReferenceEquals(Contract.Result<TPr>(), null));
            Contract.Ensures(Contains(value, Contract.Result<TPr>()));
            return default(TPr);
        }

        public IHeapHandle<TVal, TPr> Remove(TVal value)
        {
            Contract.Requires<ArgumentException>(Contains(value), ErrorMessages.NoValue);
            Contract.Ensures(Count == Contract.OldValue(Count) - 1);
            Contract.Ensures(Contract.Result<IHeapHandle<TVal, TPr>>() != null);
            Contract.Ensures(!Contains(value) && !Contains(value, Contract.Result<IHeapHandle<TVal, TPr>>().Priority));
            // ReSharper disable AssignNullToNotNullAttribute
            return default(IHeapHandle<TVal, TPr>);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        public TPr Update(TVal value, TVal newValue, TPr newPriority)
        {
            // Differently from raw heaps, here value should never be null, in order to avoid
            // exceptions while using equality comparers.
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(value, null), ErrorMessages.NullValue);
            // Priority should never be null to avoid exceptions while using comparers.
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(newValue, null), ErrorMessages.NullPriority);
            Contract.Requires<ArgumentException>(Contains(value), ErrorMessages.NoValue);
            Contract.Requires<ArgumentException>(EqualityComparer.Equals(value, newValue) || !Contains(newValue),
                                                 ErrorMessages.ValueContained);
            Contract.Ensures(Count == Contract.OldValue(Count));
            Contract.Ensures(!ReferenceEquals(Contract.Result<TPr>(), null));
            Contract.Ensures(Contains(newValue) && Contains(newValue, newPriority));
            Contract.Ensures(EqualityComparer.Equals(value, newValue) || !Contains(value));
            Contract.Ensures(EqualityComparer.Equals(value, newValue) || !Contains(value, Contract.Result<TPr>()));
            return default(TPr);
        }

        public TPr UpdatePriorityOf(TVal value, TPr newPriority)
        {
            // Priority should never be null to avoid exceptions while using comparers.
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(newPriority, null), ErrorMessages.NullPriority);
            Contract.Requires<ArgumentException>(Contains(value), ErrorMessages.NoValue);
            Contract.Ensures(Count == Contract.OldValue(Count));
            Contract.Ensures(Contains(value, newPriority));
            Contract.Ensures(Comparer.Compare(this[value], newPriority) == 0);
            Contract.Ensures(Comparer.Compare(Contract.Result<TPr>(), newPriority) == 0 ||
                             !Contains(value, Contract.Result<TPr>()));
            return default(TPr);
        }

        public void UpdateValue(TVal value, TVal newValue)
        {
            // Differently from raw heaps, here value should never be null, in order to avoid
            // exceptions while using equality comparers.
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(newValue, null), ErrorMessages.NullValue);
            Contract.Requires<ArgumentException>(Contains(value), ErrorMessages.NoValue);
            Contract.Requires<ArgumentException>(EqualityComparer.Equals(value, newValue) || !Contains(newValue),
                                                 ErrorMessages.ValueContained);
            Contract.Ensures(Count == Contract.OldValue(Count));
            Contract.Ensures(Contains(newValue));
            Contract.Ensures(EqualityComparer.Equals(value, newValue) || !Contains(value));
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