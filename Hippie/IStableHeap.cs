// IStableHeap.cs
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
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Contracts;

    /// <summary>
    ///   A generic stable heap that supports all the operations that can be done in a reasonable
    ///   amount of time on that data structure. <br/> This stable heap is an enumerable collection
    ///   of heap handles; however, pairs are _not_ granted to be visited in the order determined by <see cref="IRawHeap{TVal, TPr}.Comparer"/>.
    /// </summary>
    /// <typeparam name="TVal">The type of the values contained in the stable heap.</typeparam>
    /// <typeparam name="TPr">
    ///   The type of the priorities associated with the values contained in the stable heap.
    /// </typeparam>
    /// <remarks>
    ///   A stable raw heap allows the presence of duplicate values. Moreover, null values are
    ///   allowed, while null priorities are not (to avoid issues with comparers).
    /// </remarks>
    [ContractClass(typeof(StableRawHeapContract<,>))]
    public interface IStableRawHeap<TVal, TPr> : IRawHeap<TVal, TPr>, IStableThinHeap<TVal, TPr>
    {
        /// <summary>
        ///   Updates the priority associated with given handle and returns the old priority.
        /// </summary>
        /// <param name="handle">The handle to update.</param>
        /// <param name="value">The new priority to associate with given handle.</param>
        /// <returns>The priority previously associated with given handle.</returns>
        /// <exception cref="ArgumentException">Given handle does not belong to this heap.</exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="handle"/> or <paramref name="value"/> are null.
        /// </exception>
        [Pure]
        TPr this[IHeapHandle<TVal, IVersionedPriority<TPr>> handle] { set; }

        /// <summary>
        ///   Adds an handle with given value and given priority to the heap.
        /// </summary>
        /// <param name="val">The value to be added.</param>
        /// <param name="priority">The priority associated with given value.</param>
        /// <returns>An handle with allows to "edit" the pair added.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="priority"/> is null.</exception>
        new IHeapHandle<TVal, IVersionedPriority<TPr>> Add(TVal val, TPr priority);

        /// <summary>
        ///   Adds an handle with given value and given priority to the heap. Specified version will
        ///   be used, instead of the default one.
        /// </summary>
        /// <param name="val">The value to be added.</param>
        /// <param name="priority">The priority associated with given value.</param>
        /// <param name="version">The version associated with given value.</param>
        /// <returns>An handle with allows to "edit" the pair added.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="priority"/> is null.</exception>
        new IHeapHandle<TVal, IVersionedPriority<TPr>> Add(TVal val, TPr priority, long version);

        /// <summary>
        ///   Determines whether an element is in the heap.
        /// </summary>
        /// <param name="handle">The handle to be checked.</param>
        /// <returns>True if the element is contained, false otherwise.</returns>
        [Pure]
        bool Contains(IHeapHandle<TVal, IVersionedPriority<TPr>> handle);

        /// <summary>
        ///   Returns an enumerator that iterates through the heap.
        /// </summary>
        /// <returns>An enumerator that iterates through the heap.</returns>
        [Pure]
        new IEnumerator<IHeapHandle<TVal, IVersionedPriority<TPr>>> GetEnumerator();

        /// <summary>
        ///   Removes the item corresponding to given handle from the heap.
        /// </summary>
        /// <param name="handle">The handle corresponding to the item to be checked.</param>
        /// <returns>True if the item was contained and removed, false otherwise.</returns>
        bool Remove(IHeapHandle<TVal, IVersionedPriority<TPr>> handle);

        /// <summary>
        ///   Updates the priority associated with given handle and returns the old priority.
        /// </summary>
        /// <param name="handle">The handle to update.</param>
        /// <param name="newPriority">The new priority to associate with given handle.</param>
        /// <returns>The priority previously associated with given handle.</returns>
        /// <exception cref="ArgumentException">Given handle does not belong to this heap.</exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="handle"/> or <paramref name="newPriority"/> are null.
        /// </exception>
        IVersionedPriority<TPr> UpdatePriorityOf(IHeapHandle<TVal, IVersionedPriority<TPr>> handle,
                                                 TPr newPriority);

        /// <summary>
        ///   Updates the priority associated with given handle and returns the old priority.
        ///   Specified version will be used, instead of the default one.
        /// </summary>
        /// <param name="handle">The handle to update.</param>
        /// <param name="newPriority">The new priority to associate with given handle.</param>
        /// <param name="newVersion">The new version to associate with given handle.</param>
        /// <returns>The priority previously associated with given handle.</returns>
        /// <exception cref="ArgumentException">Given handle does not belong to this heap.</exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="handle"/> or <paramref name="newPriority"/> are null.
        /// </exception>
        IVersionedPriority<TPr> UpdatePriorityOf(IHeapHandle<TVal, IVersionedPriority<TPr>> handle,
                                                 TPr newPriority, long newVersion);

        /// <summary>
        ///   Updates given handle with the new specified value.
        /// </summary>
        /// <param name="handle">The handle whose value has to be updated.</param>
        /// <param name="newValue">The new value that will replace given old value.</param>
        /// <returns>The value previously associated with given handle.</returns>
        /// <exception cref="ArgumentException">Given handle does not belong to this heap.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="handle"/> is null.</exception>
        TVal UpdateValue(IHeapHandle<TVal, IVersionedPriority<TPr>> handle, TVal newValue);
    }
}