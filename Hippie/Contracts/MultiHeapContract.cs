// MultiHeapContract.cs
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

    [ContractClassFor(typeof(IHeap<>))]
    internal abstract class MultiHeapContract<T> : IHeap<T>
    {
        public IComparer<T> Comparer
        {
            get
            {
                Contract.Ensures(Contract.Result<IComparer<T>>() != null);
                Contract.Ensures(Count == Contract.OldValue(Count));
                return Comparer<T>.Default;
            }
        }

        public IEqualityComparer<T> EqualityComparer
        {
            get
            {
                Contract.Ensures(Contract.Result<IEqualityComparer<T>>() != null);
                Contract.Ensures(Count == Contract.OldValue(Count));
                return EqualityComparer<T>.Default;
            }
        }

        public T Min
        {
            get
            {
                Contract.Requires<InvalidOperationException>(Count > 0, ErrorMessages.NoMin);
                Contract.Ensures(Count == Contract.OldValue(Count));
                Contract.Ensures(Contains(Contract.Result<T>()));
                // ReSharper disable AssignNullToNotNullAttribute
                return default(T);
                // ReSharper restore AssignNullToNotNullAttribute
            }
        }

        public void Merge<T2>(IHeap<T2> other) where T2 : T
        {
            Contract.Requires<ArgumentNullException>(other != null, ErrorMessages.NullOther);
            Contract.Requires<ArgumentException>(ReferenceEquals(Comparer, other.Comparer),
                                                 ErrorMessages.DifferentComparers);
            Contract.Ensures(ReferenceEquals(this, other) ||
                             Count == Contract.OldValue(Count) + Contract.OldValue(other.Count));
            Contract.Ensures(ReferenceEquals(this, other) || other.Count == 0);
        }

        public T RemoveMin()
        {
            Contract.Requires<InvalidOperationException>(Count > 0, ErrorMessages.NoMin);
            Contract.Ensures(Count == Contract.OldValue(Count) - 1);
            Contract.Ensures(EqualityComparer.Equals(Contract.Result<T>(), Contract.OldValue(Min)));
            Contract.Ensures(Comparer.Compare(Contract.Result<T>(), Contract.OldValue(Min)) == 0);
            // ReSharper disable AssignNullToNotNullAttribute
            return default(T);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        public IEnumerable<IReadOnlyTree<T>> ToReadOnlyForest()
        {
            Contract.Ensures(Contract.Result<IEnumerable<IReadOnlyTree<T>>>() != null);
            // ReSharper disable AssignNullToNotNullAttribute
            return default(IEnumerable<IReadOnlyTree<T>>);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #region ICollection Members

        public abstract int Count { get; }

        public abstract bool IsReadOnly { get; }

        public abstract void Add(T item);

        public abstract void Clear();

        public abstract bool Contains(T item);

        public abstract void CopyTo(T[] array, int arrayIndex);

        public abstract IEnumerator<T> GetEnumerator();

        public abstract bool Remove(T item);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion ICollection Members
    }
}