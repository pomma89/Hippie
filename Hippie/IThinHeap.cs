// IThinHeap.cs
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
    ///   Heaps can be seen as a partially ordered collection of (value, priority) pairs. This
    ///   interface conceptually represents that kind of pair.
    /// </summary>
    /// <typeparam name="TVal">The type of <see cref="Value"/>.</typeparam>
    /// <typeparam name="TPr">The type of <see cref="Priority"/>.</typeparam>
    public interface IHeapHandle<out TVal, out TPr>
    {
        /// <summary>
        ///   The value associated with the handle.
        /// </summary>
        TVal Value { get; }

        /// <summary>
        ///   The priority corresponding to <see cref="Value"/>.
        /// </summary>
        TPr Priority { get; }
    }

    /// <summary>
    ///   Represents a priority with an version count that allows to make existing heaps stable.
    /// </summary>
    /// <typeparam name="TPr">The type of <see cref="Value"/>.</typeparam>
    public interface IVersionedPriority<out TPr>
    {
        /// <summary>
        ///   The value associated with the priority.
        /// </summary>
        TPr Value { get; }

        /// <summary>
        ///   The version corresponding to <see cref="Value"/>.
        /// </summary>
        long Version { get; }
    }

    /// <summary>
    ///   Represents an heap with a very low memory footprint, quick operations but with a low
    ///   variety of operations allowed.
    /// </summary>
    /// <typeparam name="TVal">The type of the values contained in the stable heap.</typeparam>
    /// <typeparam name="TPr">
    ///   The type of the priorities associated with the values contained in the stable heap.
    /// </typeparam>
    [ContractClass(typeof(ThinHeapContract<,>))]
    public interface IThinHeap<TVal, TPr> : ICollection<IHeapHandle<TVal, TPr>>
    {
        /// <summary>
        ///   The comparer used to decide the order of handles contained in the heap.
        /// </summary>
        [Pure]
        IComparer<TPr> Comparer { get; }

        /// <summary>
        ///   The comparer used to decide the equality of values contained in the heap.
        /// </summary>
        [Pure]
        IEqualityComparer<TVal> EqualityComparer { get; }

        /// <summary>
        ///   The minimum handle of the heap.
        /// </summary>
        /// <exception cref="InvalidOperationException">Heap is empty.</exception>
        [Pure]
        IHeapHandle<TVal, TPr> Min { get; }

        /// <summary>
        ///   Adds an handle with given value and given priority to the heap.
        /// </summary>
        /// <param name="val">The value to be added.</param>
        /// <param name="priority">The priority associated with given value.</param>
        /// <exception cref="ArgumentException">Heap already contains <paramref name="val"/>.</exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="val"/> or <paramref name="priority"/> are null.
        /// </exception>
        void Add(TVal val, TPr priority);

        /// <summary>
        ///   Moves all handles contained in <paramref name="other"/> into this heap. Since all
        ///   handles are moved, not simply added to the heap, <paramref name="other"/> is emptied
        ///   during the merge operation.
        /// </summary>
        /// <param name="other">The heap to be merged.</param>
        /// <exception cref="ArgumentException">
        ///   <paramref name="other"/> has not the same <see cref="IThinHeap{TVal, TPr}.Comparer"/>
        ///   or it contains values which are already contained in this heap.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        /// <exception cref="InvalidOperationException">
        ///   Adding all handles of <paramref name="other"/> would make this heap reach max capacity.
        /// </exception>
        void Merge<TVal2, TPr2>(IThinHeap<TVal2, TPr2> other)
            where TVal2 : TVal
            where TPr2 : TPr;

        /// <summary>
        ///   Removes and returns the handle associated with the value having minimum priority.
        /// </summary>
        /// <returns>The removed handle.</returns>
        /// <exception cref="InvalidOperationException">Heap is empty.</exception>
        IHeapHandle<TVal, TPr> RemoveMin();

        /// <summary>
        ///   Returns a snapshot of the current underlying tree structure of the heap.
        /// </summary>
        /// <remarks>
        ///   Implementing this method should be optional, it was declared only to allow the study
        ///   of the underlying data structure by creating something like a graphical tester.
        /// </remarks>
        /// <returns>A snapshot of the current underlying tree structure of the heap.</returns>
        [Pure]
        IEnumerable<IReadOnlyTree<TVal, TPr>> ToReadOnlyForest();
    }

    /// <summary>
    ///   Represents a stable heap with a very low memory footprint, quick operations but with a low
    ///   variety of operations allowed.
    /// </summary>
    /// <typeparam name="TVal">The type of the values contained in the stable heap.</typeparam>
    /// <typeparam name="TPr">
    ///   The type of the priorities associated with the values contained in the stable heap.
    /// </typeparam>
    [ContractClass(typeof(StableThinHeapContract<,>))]
    public interface IStableThinHeap<TVal, TPr> : IThinHeap<TVal, TPr>
    {
        /// <summary>
        ///   The minimum handle of the heap.
        /// </summary>
        /// <exception cref="InvalidOperationException">Heap is empty.</exception>
        [Pure]
        new IHeapHandle<TVal, IVersionedPriority<TPr>> Min { get; }

        /// <summary>
        ///   The next version that it will be automatically assigned to handles.
        /// </summary>
        [Pure]
        long NextVersion { get; }

        /// <summary>
        ///   Adds an handle with given value and given priority to the heap. Specified version will
        ///   be used, instead of the default one.
        /// </summary>
        /// <param name="value">The value to be added.</param>
        /// <param name="priority">The priority associated with given value.</param>
        /// <param name="version">The version associated with given value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="priority"/> is null.</exception>
        void Add(TVal value, TPr priority, long version);

        bool Contains(IHeapHandle<TVal, IVersionedPriority<TPr>> handle);

        /// <summary>
        ///   Removes and returns the handle associated with the value having minimum priority.
        /// </summary>
        /// <returns>The removed handle.</returns>
        /// <exception cref="InvalidOperationException">Heap is empty.</exception>
        new IHeapHandle<TVal, IVersionedPriority<TPr>> RemoveMin();
    }
}