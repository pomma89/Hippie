// 
// IHeap.cs
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
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Contracts;

    /// <summary>
    ///   A generic heap that supports all the operations that can be done
    ///   in a reasonable amount of time on that data structure.<br/>
    ///   This heap is an enumerable collection of heap handles; however, pairs are _not_
    ///   granted to be visited in the order determined by <see cref="IThinHeap{TVal, TPr}.Comparer"/>.
    /// </summary>
    /// <typeparam name="TVal">The type of the values contained in the heap.</typeparam>
    /// <typeparam name="TPr">The type of the priorities associated with the values contained in the heap.</typeparam>
    /// <remarks>
    ///   A raw heap allows the presence of duplicate values. Moreover, null values
    ///   are allowed, while null priorities are not (to avoid issues with comparers).
    /// </remarks>
    [ContractClass(typeof(RawHeapContract<,>))]
    public interface IRawHeap<TVal, TPr> : IThinHeap<TVal, TPr>
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
        TPr this[IHeapHandle<TVal, TPr> handle] { set; }

        /// <summary>
        ///   Adds an handle with given value and given priority to the heap.
        /// </summary>
        /// <param name="val">The value to be added.</param>
        /// <param name="priority">The priority associated with given value.</param>
        /// <returns>An handle with allows to "edit" the pair added.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="priority"/> is null.</exception>
        new IHeapHandle<TVal, TPr> Add(TVal val, TPr priority);

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
        TPr UpdatePriorityOf(IHeapHandle<TVal, TPr> handle, TPr newPriority);

        /// <summary>
        ///   Updates given handle with the new specified value.
        /// </summary>
        /// <param name="handle">The handle whose value has to be updated.</param>
        /// <param name="newValue">The new value that will replace given old value.</param>
        /// <returns>The value previously associated with given handle.</returns>
        /// <exception cref="ArgumentException">Given handle does not belong to this heap.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="handle"/> is null.</exception>
        TVal UpdateValue(IHeapHandle<TVal, TPr> handle, TVal newValue);
    }

    /// <summary>
    ///   A generic heap that supports all the operations that can be done
    ///   in a reasonable amount of time on that data structure.<br/>
    ///   This heap is an enumerable collection of pairs; however, pairs are _not_
    ///   granted to be visited in the order determined by <see cref="IThinHeap{TVal, TPr}.Comparer"/>.
    /// </summary>
    /// <typeparam name="TV">The type of the values contained in the heap.</typeparam>
    /// <typeparam name="TP">The type of the priorities associated with the values contained in the heap.</typeparam>
    /// <remarks>
    ///   This heap does not allow the presence of duplicate values. Moreover, null values
    ///   are null priorities _not_ allowed (to avoid issues with comparers).
    /// </remarks>
    [ContractClass(typeof(UniqueHeapContract<,>))]
    public interface IHeap<TV, TP> : IThinHeap<TV, TP>
    {
        /// <summary>
        ///   Gets or sets the priority associated with given value.
        /// </summary>
        /// <param name="val">The value for which priority has to be set or read.</param>
        /// <param name="value">The new priority to associate with given value.</param>
        /// <returns>The priority associated with given value.</returns>
        /// <exception cref="ArgumentException">There is no handle with given value.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="val"/> is null.</exception>
        [Pure]
        TP this[TV val] { get; set; }

        /// <summary>
        ///   Returns true if the heap contains an handle with given value, false otherwise.
        /// </summary>
        /// <param name="value">The value to look for.</param>
        /// <returns>True if the heap contains an handle with given value, false otherwise.</returns>
        [Pure]
        bool Contains(TV value);

        /// <summary>
        ///   Returns true if the heap contains an handle with given value and priority, false otherwise.
        /// </summary>
        /// <param name="value">The value to look for.</param>
        /// <param name="priority">The priority associated with given value.</param>
        /// <returns>True if the heap contains an handle with given value, false otherwise.</returns>
        [Pure]
        bool Contains(TV value, TP priority);

        /// <summary>
        ///   Returns the priority associated with given value.
        /// </summary>
        /// <param name="value">The value for which priority is required.</param>
        /// <returns>The priority associated with given value.</returns>
        /// <exception cref="ArgumentException">There is no handle with given value.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        [Pure]
        TP PriorityOf(TV value);

        /// <summary>
        ///   Removes and returns the handle associated with given value.
        /// </summary>
        /// <param name="value">The value to remove.</param>
        /// <returns>The removed handle.</returns>
        /// <exception cref="ArgumentException">There is no handle with given value.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        IHeapHandle<TV, TP> Remove(TV value);

        /// <summary>
        ///   Updates given value with the new specified value and the priority associated
        ///   with given value, returns the old priority.
        /// </summary>
        /// <param name="value">The value to update.</param>
        /// <param name="newValue">The new value that will replace given old value.</param>
        /// <param name="newPriority">The new priority to associate with given value.</param>
        /// <returns>The priority previously associated with given value.</returns>
        /// <exception cref="ArgumentException">There is no handle with given value.</exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="value"/>, <paramref name="newValue"/> or <paramref name="newPriority"/> are null.
        /// </exception>
        TP Update(TV value, TV newValue, TP newPriority);

        /// <summary>
        ///   Updates the priority associated with given value and returns the old priority.
        /// </summary>
        /// <param name="value">The value for which priority must be updated.</param>
        /// <param name="newPriority">The new priority to associate with given value.</param>
        /// <returns>The priority previously associated with given value.</returns>
        /// <exception cref="ArgumentException">There is no handle with given value.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        TP UpdatePriorityOf(TV value, TP newPriority);

        /// <summary>
        ///   Updates given value with the new specified value.
        /// </summary>
        /// <param name="value">The value to update.</param>
        /// <param name="newValue">The new value that will replace given old value.</param>
        /// <exception cref="ArgumentException">There is no handle with given value.</exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="value"/> or <paramref name="newValue"/> are null.
        /// </exception>
        void UpdateValue(TV value, TV newValue);
    }

    /// <summary>
    ///   A generic heap that supports all the operations that can be done
    ///   in a reasonable amount of time on that data structure.
    ///   This heap is an enumerable collection of items; however, items are _not_
    ///   granted to be visited in the order determined by <see cref="Comparer"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values contained in the heap.</typeparam>
    /// <remarks>
    ///   This heap allows the presence of duplicate values. Moreover, null values
    ///   are _not_ allowed (to avoid issues with equality comparers).
    /// </remarks>
    [ContractClass(typeof(MultiHeapContract<>))]
    public interface IHeap<T> : ICollection<T>
    {
        /// <summary>
        ///   The comparer used to decide the order of values contained in the heap.
        /// </summary>
        [Pure]
        IComparer<T> Comparer { get; }

        /// <summary>
        ///   The comparer used to decide the equality of values contained in the heap.
        /// </summary>
        [Pure]
        IEqualityComparer<T> EqualityComparer { get; }

        /// <summary>
        ///   The minimum value of the heap.
        /// </summary>
        /// <exception cref="InvalidOperationException">Heap is empty.</exception>
        [Pure]
        T Min { get; }

        /// <summary>
        ///   Moves all values contained in <paramref name="other"/> into this heap. Since all values are moved,
        ///   not simply added to the heap, <paramref name="other"/> is emptied during the merge operation.
        /// </summary>
        /// <param name="other">The heap to be merged.</param>
        /// <exception cref="ArgumentException">
        ///   <paramref name="other"/> has not the same <see cref="Comparer"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        /// <exception cref="InvalidOperationException">
        ///   Adding all values of <paramref name="other"/> would make this heap reach max capacity.
        /// </exception>	
        void Merge<TVal2>(IHeap<TVal2> other) where TVal2 : T;

        /// <summary>
        ///   Removes and returns the minimum value.
        /// </summary>
        /// <returns>The removed value.</returns>
        /// <exception cref="InvalidOperationException">Heap is empty.</exception>
        T RemoveMin();

        /// <summary>
        ///   Returns a snapshot of the current underlying tree structure of the heap.
        /// </summary>
        /// <remarks>
        ///   Implementing this method should be optional, it was declared only to allow
        ///   the study of the underlying data structure by creating something like a graphical tester.
        /// </remarks>
        /// <returns>A snapshot of the current underlying tree structure of the heap.</returns>
        IEnumerable<IReadOnlyTree<T>> ToReadOnlyForest();
    }
}