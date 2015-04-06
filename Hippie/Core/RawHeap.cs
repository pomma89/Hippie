// RawHeap.cs
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

namespace DIBRIS.Hippie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using DIBRIS.Hippie;

    public abstract class RawHeap<TVal, TPr, TItem> where TItem : class, RawHeap<TVal, TPr, TItem>.IItem
    {
        protected readonly Func<TPr, TPr, int> Cmp;
        private readonly IComparer<TPr> _cmp;

        internal RawHeap(IComparer<TPr> cmp)
        {
            Contract.Requires<ArgumentNullException>(cmp != null, ErrorMessages.NullComparer);
            _cmp = cmp;
            Cmp = cmp.Compare;
        }

        public IComparer<TPr> Comparer
        {
            get { return _cmp; }
        }

        public int Count { get; protected set; }

        public IEqualityComparer<TVal> EqualityComparer
        {
            get { return EqualityComparer<TVal>.Default; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public TPr this[IHeapHandle<TVal, TPr> handle]
        {
            set { UpdatePriorityOf(handle, value); }
        }

        public bool Contains(IHeapHandle<TVal, TPr> handle)
        {
            return (GetHandle(handle as TItem) != null);
        }

        public TPr UpdatePriorityOf(IHeapHandle<TVal, TPr> handle, TPr newPriority)
        {
            var item = handle as TItem;
            Debug.Assert(item != null);
            var oldPr = item.Priority;
            item.Priority = newPriority;
            var cmp = Cmp(newPriority, oldPr);
            if (cmp < 0)
            {
                MoveUp(item);
            }
            else if (cmp > 0)
            {
                MoveDown(item);
            }
            return oldPr;
        }

        public TVal UpdateValue(IHeapHandle<TVal, TPr> handle, TVal newValue)
        {
            var item = handle as TItem;
            Debug.Assert(item != null);
            var oldVal = item.Value;
            item.Value = newValue;
            return oldVal;
        }

        /**********************************************************************
		 * Abstract methods and properties
		 **********************************************************************/

        protected abstract TItem GetHandle(TItem item);

        protected abstract void MoveDown(TItem item);

        protected abstract void MoveUp(TItem item);

        /**********************************************************************
		 * IItem interface
		 **********************************************************************/

        public interface IItem
        {
            TVal Value { get; set; }

            TPr Priority { get; set; }
        }
    }
}