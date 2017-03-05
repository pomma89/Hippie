// HeapFactory.cs
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
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace DIBRIS.Hippie
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///   Factory able to create five types of heap: array, binary, binomial, Fibonacci, pairing.
    ///   Each heap, of course, has its strengths and weaknesses: see the documentation of each
    ///   method to better understand the complexities of various heap operations. <br/> As a general
    ///   rule, however, what you need is simply a binary heap, that is, an array heap with two
    ///   children per node. As a matter of fact, they seem to offer the best performance. <br/>
    ///   While specyfing complexities, by writing "log(a, b)" we mean logarithm in base b of a, and
    ///   by writing "log(a)" we mean a function growing with the same speed of a logarithm.
    /// </summary>
    public static class HeapFactory
    {
        /**********************************************************************
		 * Thin
		 **********************************************************************/

        /// <summary>
        ///   Returns a thin heap implemented as a binary heap; see
        ///   https://en.wikipedia.org/wiki/Binary_heap for details about binary heaps. <br/> Heap
        ///   will have default priority comparer and default value equality comparer if
        ///   <paramref name="equalityComparer"/> is null or given value equality comparer otherwise.
        ///   <br/> If n is the number of pairs contained in the heap, then the time complexities for
        ///   the operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations |
        ///   Complexity | | :- | :- | | Add | O(log(n, 2)) | | Contains | O(n) | | Merge{V, P} | O(m
        ///   * log(m+n, 2)), m is other.Count | | Merge{V2, P2} | O(m * log(m+n, 2)), m is
        ///     other.Count | | Min | O(1) | | Remove | O(n) | | RemoveMin | O(log(n, 2)) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="equalityComparer">The value equality comparer the heap will use.</param>
        /// <returns>A unique heap implemented as a binary heap.</returns>
        public static ThinHeap<TVal, TPr> NewHeap<TVal, TPr>(IEqualityComparer<TVal> equalityComparer = null)
            where TPr : IComparable<TPr>
        {
            return NewHeap(BetterComparer<TPr>.Default, equalityComparer);
        }

        /// <summary>
        ///   Returns a thin heap implemented as a binary heap; see
        ///   https://en.wikipedia.org/wiki/Binary_heap for details about binary heaps. <br/> Heap
        ///   will have given priority comparer and default value equality comparer if
        ///   <paramref name="equalityComparer"/> is null or given value equality comparer otherwise.
        ///   <br/> If n is the number of pairs contained in the heap, then the time complexities for
        ///   the operations <see cref="IThinHeap{TVal,TPr}"/> offers are: <br/> | Operations |
        ///   Complexity | | :- | :- | | Add | O(log(n, 2)) | | Contains | O(n) | | Merge{V, P} | O(m
        ///   * log(m+n, 2)), m is other.Count | | Merge{V2, P2} | O(m * log(m+n, 2)), m is
        ///     other.Count | | Min | O(1) | | Remove | O(n) | | RemoveMin | O(log(n, 2)) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="comparer">The priority comparer the heap will use.</param>
        /// <param name="equalityComparer">The value equality comparer the heap will use.</param>
        /// <returns>A unique heap implemented as a binary heap.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is null.</exception>
        public static ThinHeap<TVal, TPr> NewHeap<TVal, TPr>(IComparer<TPr> comparer,
                                                             IEqualityComparer<TVal> equalityComparer = null)
        {
            return new ThinHeap<TVal, TPr>(comparer, equalityComparer ?? EqualityComparer<TVal>.Default);
        }

        /**********************************************************************
		 * Array
		 **********************************************************************/

        /// <summary>
        ///   Returns a raw heap implemented as a array heap; see
        ///   https://en.wikipedia.org/wiki/D-ary_heap for details about array heaps. <br/> Heap will
        ///   have default priority comparer and default value equality comparer. <br/> If n is the
        ///   number of pairs contained in the heap and k is the number of children of each node,
        ///   then the time complexities for the operations <see cref="IHeap{TVal,TPr}"/> offers are:
        ///   <br/> | Operations | Complexity | | :- | :- | | Add | O(log(n, k)) | | Contains | O(1)
        ///   | | Merge{V, P} | O(m * log(m+n, k)), m is other.Count | | Merge{V2, P2} | O(m *
        ///   log(m+n, k)), m is other.Count | | Min | O(1) | | PriorityOf | O(1) | | Remove |
        ///   O(log(n, k)) | | RemoveMin | O(log(n, k)) | | Update | O(log(n, k)) | |
        ///   UpdatePriorityOf | O(log(n, k)) | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="cc">The number of children each node will have.</param>
        /// <returns>A raw heap implemented as a array heap.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="cc"/> is less than two.
        /// </exception>
        public static ArrayHeap<TVal, TPr> NewRawArrayHeap<TVal, TPr>(byte cc) where TPr : IComparable<TPr>
        {
            return NewRawArrayHeap<TVal, TPr>(cc, BetterComparer<TPr>.Default);
        }

        /// <summary>
        ///   Returns a raw heap implemented as a array heap; see
        ///   https://en.wikipedia.org/wiki/D-ary_heap for details about array heaps. <br/> Heap will
        ///   have given priority comparer and default value equality comparer. <br/> If n is the
        ///   number of pairs contained in the heap and k is the number of children of each node,
        ///   then the time complexities for the operations <see cref="IHeap{TVal,TPr}"/> offers are:
        ///   <br/> | Operations | Complexity | | :- | :- | | Add | O(log(n, k)) | | Contains | O(1)
        ///   | | Merge{V, P} | O(m * log(m+n, k)), m is other.Count | | Merge{V2, P2} | O(m *
        ///   log(m+n, k)), m is other.Count | | Min | O(1) | | PriorityOf | O(1) | | Remove |
        ///   O(log(n, k)) | | RemoveMin | O(log(n, k)) | | Update | O(log(n, k)) | |
        ///   UpdatePriorityOf | O(log(n, k)) | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="cc">The number of children each node will have.</param>
        /// <param name="cmp">The priority comparer the heap will use.</param>
        /// <returns>A raw heap implemented as a array heap.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="cc"/> is less than two.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static ArrayHeap<TVal, TPr> NewRawArrayHeap<TVal, TPr>(Byte cc, IComparer<TPr> cmp)
        {
            return new ArrayHeap<TVal, TPr>(cc, cmp);
        }

        /// <summary>
        ///   Returns a unique heap implemented as a array heap; see
        ///   https://en.wikipedia.org/wiki/D-ary_heap for details about array heaps. <br/> Heap will
        ///   have default priority comparer and default value equality comparer if
        ///   <paramref name="eqCmp"/> is null or given value equality comparer otherwise. <br/> If n
        ///   is the number of pairs contained in the heap and k is the number of children of each
        ///   node, then the time complexities for the operations <see cref="IHeap{TVal,TPr}"/>
        ///   offers are: <br/> | Operations | Complexity | | :- | :- | | Add | O(log(n, k)) | |
        ///   Contains | O(1) | | Merge{V, P} | O(m * log(m+n, k)), m is other.Count | | Merge{V2,
        ///   P2} | O(m * log(m+n, k)), m is other.Count | | Min | O(1) | | PriorityOf | O(1) | |
        ///   Remove | O(log(n, k)) | | RemoveMin | O(log(n, k)) | | Update | O(log(n, k)) | |
        ///   UpdatePriorityOf | O(log(n, k)) | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="cc">The number of children each node will have.</param>
        /// <param name="eqCmp">The value equality comparer the heap will use.</param>
        /// <returns>A unique heap implemented as a array heap.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="cc"/> is less than two.
        /// </exception>
        public static UniqueHeap<TVal, TPr> NewArrayHeap<TVal, TPr>(Byte cc, IEqualityComparer<TVal> eqCmp = null)
            where TPr : IComparable<TPr>
        {
            return NewArrayHeap(cc, BetterComparer<TPr>.Default, eqCmp);
        }

        /// <summary>
        ///   Returns a unique heap implemented as a array heap; see
        ///   https://en.wikipedia.org/wiki/D-ary_heap for details about array heaps. <br/> Heap will
        ///   have given priority comparer and default value equality comparer if
        ///   <paramref name="eqCmp"/> is null or given value equality comparer otherwise. <br/> If n
        ///   is the number of pairs contained in the heap and k is the number of children of each
        ///   node, then the time complexities for the operations <see cref="IHeap{TVal,TPr}"/>
        ///   offers are: <br/> | Operations | Complexity | | :- | :- | | Add | O(log(n, k)) | |
        ///   Contains | O(1) | | Merge{V, P} | O(m * log(m+n, k)), m is other.Count | | Merge{V2,
        ///   P2} | O(m * log(m+n, k)), m is other.Count | | Min | O(1) | | PriorityOf | O(1) | |
        ///   Remove | O(log(n, k)) | | RemoveMin | O(log(n, k)) | | Update | O(log(n, k)) | |
        ///   UpdatePriorityOf | O(log(n, k)) | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="cc">The number of children each node will have.</param>
        /// <param name="cmp">The priority comparer the heap will use.</param>
        /// <param name="eqCmp">The value equality comparer the heap will use.</param>
        /// <returns>A unique heap implemented as a array heap.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="cc"/> is less than two.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static UniqueHeap<TVal, TPr> NewArrayHeap<TVal, TPr>(Byte cc, IComparer<TPr> cmp,
                                                                    IEqualityComparer<TVal> eqCmp = null)
        {
            return new UniqueHeap<TVal, TPr>(new ArrayHeap<TVal, TPr>(cc, cmp), eqCmp);
        }

        /// <summary>
        ///   Returns a multi heap implemented as a array heap; see
        ///   https://en.wikipedia.org/wiki/D-ary_heap for details about array heaps. <br/> Heap will
        ///   have default comparer and default equality comparer if <paramref name="eqCmp"/> is null
        ///   or given value equality comparer otherwise. <br/> If n is the number of pairs contained
        ///   in the heap and k is the number of children of each node, then the time complexities
        ///   for the operations <see cref="IHeap{T}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(log(n, k)) | | Contains | O(n) | | Merge{T} | O(m
        ///   * log(m+n, k)), m is other.Count | | Merge{T2} | O(m * log(m+n, k)), m is other.Count |
        ///     | Min | O(1) | | Remove | O(n) | | RemoveMin | O(log(n, k)) |
        /// </summary>
        /// <typeparam name="T">The type of the items held by the heap.</typeparam>
        /// <param name="cc">The number of children each node will have.</param>
        /// <param name="eqCmp">The equality comparer the heap will use.</param>
        /// <returns>A multi heap implemented as a array heap.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="cc"/> is less than two.
        /// </exception>
        public static MultiHeap<T> NewArrayHeap<T>(Byte cc, IEqualityComparer<T> eqCmp = null) where T : IComparable<T>
        {
            return NewArrayHeap<T>(cc, BetterComparer<T>.Default, eqCmp);
        }

        /// <summary>
        ///   Returns a multi heap implemented as a array heap; see
        ///   https://en.wikipedia.org/wiki/D-ary_heap for details about array heaps. <br/> Heap will
        ///   have given comparer and default equality comparer if <paramref name="eqCmp"/> is null
        ///   or given value equality comparer otherwise. <br/> If n is the number of pairs contained
        ///   in the heap and k is the number of children of each node, then the time complexities
        ///   for the operations <see cref="IHeap{T}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(log(n, k)) | | Contains | O(n) | | Merge{T} | O(m
        ///   * log(m+n, k)), m is other.Count | | Merge{T2} | O(m * log(m+n, k)), m is other.Count |
        ///     | Min | O(1) | | Remove | O(n) | | RemoveMin | O(log(n, k)) |
        /// </summary>
        /// <typeparam name="T">The type of the items held by the heap.</typeparam>
        /// <param name="cc">The number of children each node will have.</param>
        /// <param name="cmp">The comparer the heap will use.</param>
        /// <param name="eqCmp">The equality comparer the heap will use.</param>
        /// <returns>A multi heap implemented as a array heap.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="cc"/> is less than two.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static MultiHeap<T> NewArrayHeap<T>(Byte cc, IComparer<T> cmp, IEqualityComparer<T> eqCmp = null)
        {
            return new MultiHeap<T>(new ArrayHeap<T, T>(cc, cmp), eqCmp);
        }

        /**********************************************************************
		 * Binary
		 **********************************************************************/

        /// <summary>
        ///   Returns a raw heap implemented as a binary heap; see
        ///   https://en.wikipedia.org/wiki/Binary_heap for details about binary heaps. <br/> Heap
        ///   will have default priority comparer and default value equality comparer. <br/> If n is
        ///   the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(log(n, 2)) | | Contains | O(1) | | Merge{V, P} | O(m * log(m+n,
        ///   2)), m is other.Count | | Merge{V2, P2} | O(m * log(m+n, 2)), m is other.Count | | Min
        ///   | O(1) | | PriorityOf | O(1) | | Remove | O(log(n, 2)) | | RemoveMin | O(log(n, 2)) | |
        ///   Update | O(log(n, 2)) | | UpdatePriorityOf | O(log(n, 2)) | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <returns>A raw heap implemented as a binary heap.</returns>
        public static ArrayHeap<TVal, TPr> NewRawBinaryHeap<TVal, TPr>() where TPr : IComparable<TPr>
        {
            return NewRawBinaryHeap<TVal, TPr>(BetterComparer<TPr>.Default);
        }

        /// <summary>
        ///   Returns a raw heap implemented as a binary heap; see
        ///   https://en.wikipedia.org/wiki/Binary_heap for details about binary heaps. <br/> Heap
        ///   will have given priority comparer and default value equality comparer. <br/> If n is
        ///   the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(log(n, 2)) | | Contains | O(1) | | Merge{V, P} | O(m * log(m+n,
        ///   2)), m is other.Count | | Merge{V2, P2} | O(m * log(m+n, 2)), m is other.Count | | Min
        ///   | O(1) | | PriorityOf | O(1) | | Remove | O(log(n, 2)) | | RemoveMin | O(log(n, 2)) | |
        ///   Update | O(log(n, 2)) | | UpdatePriorityOf | O(log(n, 2)) | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="cmp">The priority comparer the heap will use.</param>
        /// <returns>A raw heap implemented as a binary heap.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static ArrayHeap<TVal, TPr> NewRawBinaryHeap<TVal, TPr>(IComparer<TPr> cmp)
        {
            return new ArrayHeap<TVal, TPr>(2, cmp);
        }

        /// <summary>
        ///   Returns a unique heap implemented as a binary heap; see
        ///   https://en.wikipedia.org/wiki/Binary_heap for details about binary heaps. <br/> Heap
        ///   will have default priority comparer and default value equality comparer if
        ///   <paramref name="eqCmp"/> is null or given value equality comparer otherwise. <br/> If n
        ///   is the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(log(n, 2)) | | Contains | O(1) | | Merge{V, P} | O(m * log(m+n,
        ///   2)), m is other.Count | | Merge{V2, P2} | O(m * log(m+n, 2)), m is other.Count | | Min
        ///   | O(1) | | PriorityOf | O(1) | | Remove | O(log(n, 2)) | | RemoveMin | O(log(n, 2)) | |
        ///   Update | O(log(n, 2)) | | UpdatePriorityOf | O(log(n, 2)) | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="eqCmp">The value equality comparer the heap will use.</param>
        /// <returns>A unique heap implemented as a binary heap.</returns>
        public static UniqueHeap<TVal, TPr> NewBinaryHeap<TVal, TPr>(IEqualityComparer<TVal> eqCmp = null)
            where TPr : IComparable<TPr>
        {
            return NewBinaryHeap(BetterComparer<TPr>.Default, eqCmp);
        }

        /// <summary>
        ///   Returns a unique heap implemented as a binary heap; see
        ///   https://en.wikipedia.org/wiki/Binary_heap for details about binary heaps. <br/> Heap
        ///   will have given priority comparer and default value equality comparer if
        ///   <paramref name="eqCmp"/> is null or given value equality comparer otherwise. <br/> If n
        ///   is the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(log(n, 2)) | | Contains | O(1) | | Merge{V, P} | O(m * log(m+n,
        ///   2)), m is other.Count | | Merge{V2, P2} | O(m * log(m+n, 2)), m is other.Count | | Min
        ///   | O(1) | | PriorityOf | O(1) | | Remove | O(log(n, 2)) | | RemoveMin | O(log(n, 2)) | |
        ///   Update | O(log(n, 2)) | | UpdatePriorityOf | O(log(n, 2)) | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="cmp">The priority comparer the heap will use.</param>
        /// <param name="eqCmp">The value equality comparer the heap will use.</param>
        /// <returns>A unique heap implemented as a binary heap.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static UniqueHeap<TVal, TPr> NewBinaryHeap<TVal, TPr>(IComparer<TPr> cmp,
                                                                     IEqualityComparer<TVal> eqCmp = null)
        {
            return new UniqueHeap<TVal, TPr>(new ArrayHeap<TVal, TPr>(2, cmp), eqCmp);
        }

        /// <summary>
        ///   Returns a multi heap implemented as a binary heap; see
        ///   https://en.wikipedia.org/wiki/Binary_heap for details about binary heaps. <br/> Heap
        ///   will have default comparer and default equality comparer if <paramref name="eqCmp"/> is
        ///   null or given value equality comparer otherwise. <br/> If n is the number of items
        ///   contained in the heap, then the time complexities for the operations
        ///   <see cref="IHeap{T}"/> offers are: <br/> | Operations | Complexity | | :- | :- | | Add
        ///   | O(log(n, 2)) | | Contains | O(n) | | Merge{T} | O(m * log(m+n, 2)), m is other.Count
        ///   | | Merge{T2} | O(m * log(m+n, 2)), m is other.Count | | Min | O(1) | | Remove | O(n) |
        ///   | RemoveMin | O(log(n, 2)) |
        /// </summary>
        /// <typeparam name="T">The type of the items held by the heap.</typeparam>
        /// <param name="eqCmp">The equality comparer the heap will use.</param>
        /// <returns>A multi heap implemented as a binary heap.</returns>
        public static MultiHeap<T> NewBinaryHeap<T>(IEqualityComparer<T> eqCmp = null) where T : IComparable<T>
        {
            return NewBinaryHeap<T>(BetterComparer<T>.Default, eqCmp);
        }

        /// <summary>
        ///   Returns a multi heap implemented as a binary heap; see
        ///   https://en.wikipedia.org/wiki/Binary_heap for details about binary heaps. <br/> Heap
        ///   will have given comparer and default equality comparer if <paramref name="eqCmp"/> is
        ///   null or given value equality comparer otherwise. <br/> If n is the number of items
        ///   contained in the heap, then the time complexities for the operations
        ///   <see cref="IHeap{T}"/> offers are: <br/> | Operations | Complexity | | :- | :- | | Add
        ///   | O(log(n, 2)) | | Contains | O(n) | | Merge{T} | O(m * log(m+n, 2)), m is other.Count
        ///   | | Merge{T2} | O(m * log(m+n, 2)), m is other.Count | | Min | O(1) | | Remove | O(n) |
        ///   | RemoveMin | O(log(n, 2)) |
        /// </summary>
        /// <typeparam name="T">The type of the items held by the heap.</typeparam>
        /// <param name="cmp">The comparer the heap will use.</param>
        /// <param name="eqCmp">The equality comparer the heap will use.</param>
        /// <returns>A multi heap implemented as a binary heap.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static MultiHeap<T> NewBinaryHeap<T>(IComparer<T> cmp, IEqualityComparer<T> eqCmp = null)
        {
            return new MultiHeap<T>(new ArrayHeap<T, T>(2, cmp), eqCmp);
        }

        /**********************************************************************
		 * Binomial
		 **********************************************************************/

        /// <summary>
        ///   Returns a raw heap implemented as a binomial heap; see
        ///   https://en.wikipedia.org/wiki/Binomial_heap for details about binomial heaps. <br/>
        ///   Heap will have default priority comparer and default value equality comparer. <br/> If
        ///   n is the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(log(n)) | | Contains | O(1) | | Merge{V, P} | O(log(m)), m is
        ///   larger heap size | | Merge{V2, P2} | O(m * log(m+n)), m is other.Count | | Min | O(1) |
        ///   | PriorityOf | O(1) | | Remove | O(log(n)) | | RemoveMin | O(log(n)) | | Update |
        ///   O(log(n)) | | UpdatePriorityOf | O(log(n)) | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <returns>A raw heap implemented as a binomial heap.</returns>
        public static BinomialHeap<TVal, TPr> NewRawBinomialHeap<TVal, TPr>() where TPr : IComparable<TPr>
        {
            return NewRawBinomialHeap<TVal, TPr>(BetterComparer<TPr>.Default);
        }

        /// <summary>
        ///   Returns a raw heap implemented as a binomial heap; see
        ///   https://en.wikipedia.org/wiki/Binomial_heap for details about binomial heaps. <br/>
        ///   Heap will have given priority comparer and default value equality comparer. <br/> If n
        ///   is the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(log(n)) | | Contains | O(1) | | Merge{V, P} | O(log(m)), m is
        ///   larger heap size | | Merge{V2, P2} | O(m * log(m+n)), m is other.Count | | Min | O(1) |
        ///   | PriorityOf | O(1) | | Remove | O(log(n)) | | RemoveMin | O(log(n)) | | Update |
        ///   O(log(n)) | | UpdatePriorityOf | O(log(n)) | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="cmp">The priority comparer the heap will use.</param>
        /// <returns>A raw heap implemented as a binomial heap.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static BinomialHeap<TVal, TPr> NewRawBinomialHeap<TVal, TPr>(IComparer<TPr> cmp)
        {
            return new BinomialHeap<TVal, TPr>(cmp);
        }

        /// <summary>
        ///   Returns a unique heap implemented as a binomial heap; see
        ///   https://en.wikipedia.org/wiki/Binomial_heap for details about binomial heaps. <br/>
        ///   Heap will have default priority comparer and default value equality comparer if
        ///   <paramref name="eqCmp"/> is null or given value equality comparer otherwise. <br/> If n
        ///   is the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(log(n)) | | Contains | O(1) | | Merge{V, P} | O(m * log(m+n)), m
        ///   is other.Count | | Merge{V2, P2} | O(m * log(m+n)), m is other.Count | | Min | O(1) | |
        ///   PriorityOf | O(1) | | Remove | O(log(n)) | | RemoveMin | O(log(n)) | | Update |
        ///   O(log(n)) | | UpdatePriorityOf | O(log(n)) | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="eqCmp">The value equality comparer the heap will use.</param>
        /// <returns>A unique heap implemented as a binomial heap.</returns>
        public static UniqueHeap<TVal, TPr> NewBinomialHeap<TVal, TPr>(IEqualityComparer<TVal> eqCmp = null)
            where TPr : IComparable<TPr>
        {
            return NewBinomialHeap(BetterComparer<TPr>.Default, eqCmp);
        }

        /// <summary>
        ///   Returns a unique heap implemented as a binomial heap; see
        ///   https://en.wikipedia.org/wiki/Binomial_heap for details about binomial heaps. <br/>
        ///   Heap will have given priority comparer and default value equality comparer if
        ///   <paramref name="eqCmp"/> is null or given value equality comparer otherwise. <br/> If n
        ///   is the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(log(n)) | | Contains | O(1) | | Merge{V, P} | O(m * log(m+n)), m
        ///   is other.Count | | Merge{V2, P2} | O(m * log(m+n)), m is other.Count | | Min | O(1) | |
        ///   PriorityOf | O(1) | | Remove | O(log(n)) | | RemoveMin | O(log(n)) | | Update |
        ///   O(log(n)) | | UpdatePriorityOf | O(log(n)) | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="cmp">The priority comparer the heap will use.</param>
        /// <param name="eqCmp">The value equality comparer the heap will use.</param>
        /// <returns>A unique heap implemented as a binomial heap.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static UniqueHeap<TVal, TPr> NewBinomialHeap<TVal, TPr>(IComparer<TPr> cmp,
                                                                       IEqualityComparer<TVal> eqCmp = null)
        {
            return new UniqueHeap<TVal, TPr>(new BinomialHeap<TVal, TPr>(cmp), eqCmp);
        }

        /// <summary>
        ///   Returns a multi heap implemented as a binomial heap; see
        ///   https://en.wikipedia.org/wiki/Binomial_heap for details about binomial heaps. <br/>
        ///   Heap will have default comparer and default equality comparer if
        ///   <paramref name="eqCmp"/> is null or given value equality comparer otherwise. <br/> If n
        ///   is the number of items contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{T}"/> offers are: <br/> | Operations | Complexity | | :- |
        ///   :- | | Add | O(log(n)) | | Contains | O(n) | | Merge{T} | O(m * log(m+n)), m is
        ///   other.Count | | Merge{T2} | O(m * log(m+n)), m is other.Count | | Min | O(1) | | Remove
        ///   | O(n) | | RemoveMin | O(log(n)) |
        /// </summary>
        /// <typeparam name="T">The type of the items held by the heap.</typeparam>
        /// <param name="eqCmp">The equality comparer the heap will use.</param>
        /// <returns>A multi heap implemented as a binomial heap.</returns>
        public static MultiHeap<T> NewBinomialHeap<T>(IEqualityComparer<T> eqCmp = null) where T : IComparable<T>
        {
            return NewBinomialHeap<T>(BetterComparer<T>.Default, eqCmp);
        }

        /// <summary>
        ///   Returns a multi heap implemented as a binomial heap; see
        ///   https://en.wikipedia.org/wiki/Binomial_heap for details about binomial heaps. <br/>
        ///   Heap will have given comparer and default equality comparer if <paramref name="eqCmp"/>
        ///   is null or given value equality comparer otherwise. <br/> If n is the number of items
        ///   contained in the heap, then the time complexities for the operations
        ///   <see cref="IHeap{T}"/> offers are: <br/> | Operations | Complexity | | :- | :- | | Add
        ///   | O(log(n)) | | Contains | O(n) | | Merge{T} | O(m * log(m+n)), m is other.Count | |
        ///   Merge{T2} | O(m * log(m+n)), m is other.Count | | Min | O(1) | | Remove | O(n) | |
        ///   RemoveMin | O(log(n)) |
        /// </summary>
        /// <typeparam name="T">The type of the items held by the heap.</typeparam>
        /// <param name="cmp">The comparer the heap will use.</param>
        /// <param name="eqCmp">The equality comparer the heap will use.</param>
        /// <returns>A multi heap implemented as a binomial heap.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static MultiHeap<T> NewBinomialHeap<T>(IComparer<T> cmp, IEqualityComparer<T> eqCmp = null)
        {
            return new MultiHeap<T>(new BinomialHeap<T, T>(cmp), eqCmp);
        }

        /**********************************************************************
		 * Fibonacci
		 **********************************************************************/

        /// <summary>
        ///   Returns a raw heap implemented as a Fibonacci heap; see
        ///   https://en.wikipedia.org/wiki/Fibonacci_heap for details about Fibonacci heaps. <br/>
        ///   Heap will have default priority comparer and default value equality comparer. <br/> If
        ///   n is the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(1) | | Contains | O(1) | | Merge{V, P} | O(1) | | Merge{V2, P2} |
        ///   O(m), m is other.Count | | Min | O(1) | | PriorityOf | O(1) | | Remove | O(log(n)),
        ///   amortized | | RemoveMin | O(log(n)), amortized | | Update | O(1), amortized | |
        ///   UpdatePriorityOf | O(1), amortized | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <returns>A raw heap implemented as a Fibonacci heap.</returns>
        public static FibonacciHeap<TVal, TPr> NewRawFibonacciHeap<TVal, TPr>() where TPr : IComparable<TPr>
        {
            return NewRawFibonacciHeap<TVal, TPr>(BetterComparer<TPr>.Default);
        }

        /// <summary>
        ///   Returns a raw heap implemented as a Fibonacci heap; see
        ///   https://en.wikipedia.org/wiki/Fibonacci_heap for details about Fibonacci heaps. <br/>
        ///   Heap will have given priority comparer and default value equality comparer. <br/> If n
        ///   is the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(1) | | Contains | O(1) | | Merge{V, P} | O(1) | | Merge{V2, P2} |
        ///   O(m), m is other.Count | | Min | O(1) | | PriorityOf | O(1) | | Remove | O(log(n)),
        ///   amortized | | RemoveMin | O(log(n)), amortized | | Update | O(1), amortized | |
        ///   UpdatePriorityOf | O(1), amortized | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="cmp">The priority comparer the heap will use.</param>
        /// <returns>A raw heap implemented as a Fibonacci heap.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static FibonacciHeap<TVal, TPr> NewRawFibonacciHeap<TVal, TPr>(IComparer<TPr> cmp)
        {
            return new FibonacciHeap<TVal, TPr>(cmp);
        }

        /// <summary>
        ///   Returns a unique heap implemented as a Fibonacci heap; see
        ///   https://en.wikipedia.org/wiki/Fibonacci_heap for details about Fibonacci heaps. <br/>
        ///   Heap will have default priority comparer and default value equality comparer if
        ///   <paramref name="eqCmp"/> is null or given value equality comparer otherwise. <br/> If n
        ///   is the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(1) | | Contains | O(1) | | Merge{V, P} | O(m), m is other.Count |
        ///   | Merge{V2, P2} | O(m), m is other.Count | | Min | O(1) | | PriorityOf | O(1) | |
        ///   Remove | O(log(n)), amortized | | RemoveMin | O(log(n)), amortized | | Update | O(1),
        ///   amortized | | UpdatePriorityOf | O(1), amortized | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="eqCmp">The value equality comparer the heap will use.</param>
        /// <returns>A unique heap implemented as a Fibonacci heap.</returns>
        public static UniqueHeap<TVal, TPr> NewFibonacciHeap<TVal, TPr>(IEqualityComparer<TVal> eqCmp = null)
            where TPr : IComparable<TPr>
        {
            return NewFibonacciHeap(BetterComparer<TPr>.Default, eqCmp);
        }

        /// <summary>
        ///   Returns a unique heap implemented as a Fibonacci heap; see
        ///   https://en.wikipedia.org/wiki/Fibonacci_heap for details about Fibonacci heaps. <br/>
        ///   Heap will have given priority comparer and default value equality comparer if
        ///   <paramref name="eqCmp"/> is null or given value equality comparer otherwise. <br/> If n
        ///   is the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(1) | | Contains | O(1) | | Merge{V, P} | O(m), m is other.Count |
        ///   | Merge{V2, P2} | O(m), m is other.Count | | Min | O(1) | | PriorityOf | O(1) | |
        ///   Remove | O(log(n)), amortized | | RemoveMin | O(log(n)), amortized | | Update | O(1),
        ///   amortized | | UpdatePriorityOf | O(1), amortized | | UpdateValue | O(1) |
        /// </summary>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="cmp">The priority comparer the heap will use.</param>
        /// <param name="eqCmp">The value equality comparer the heap will use.</param>
        /// <returns>A unique heap implemented as a Fibonacci heap.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static UniqueHeap<TVal, TPr> NewFibonacciHeap<TVal, TPr>(IComparer<TPr> cmp,
                                                                        IEqualityComparer<TVal> eqCmp = null)
        {
            return new UniqueHeap<TVal, TPr>(new FibonacciHeap<TVal, TPr>(cmp), eqCmp);
        }

        /// <summary>
        ///   Returns a multi heap implemented as a Fibonacci heap; see
        ///   https://en.wikipedia.org/wiki/Fibonacci_heap for details about Fibonacci heaps. <br/>
        ///   Heap will have default comparer and default equality comparer if
        ///   <paramref name="eqCmp"/> is null or given value equality comparer otherwise. <br/> If n
        ///   is the number of items contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{T}"/> offers are: <br/> | Operations | Complexity | | :- |
        ///   :- | | Add | O(1) | | Contains | O(n) | | Merge{T} | O(m), m is other.Count | |
        ///   Merge{T2} | O(m), m is other.Count | | Min | O(1) | | Remove | O(n) | | RemoveMin |
        ///   O(log(n)), amortized |
        /// </summary>
        /// <typeparam name="T">The type of the items held by the heap.</typeparam>
        /// <param name="eqCmp">The equality comparer the heap will use.</param>
        /// <returns>A multi heap implemented as a Fibonacci heap.</returns>
        public static MultiHeap<T> NewFibonacciHeap<T>(IEqualityComparer<T> eqCmp = null) where T : IComparable<T>
        {
            return NewFibonacciHeap<T>(BetterComparer<T>.Default, eqCmp);
        }

        /// <summary>
        ///   Returns a multi heap implemented as a Fibonacci heap; see
        ///   https://en.wikipedia.org/wiki/Fibonacci_heap for details about Fibonacci heaps. <br/>
        ///   Heap will have given comparer and default equality comparer if <paramref name="eqCmp"/>
        ///   is null or given value equality comparer otherwise. <br/> If n is the number of items
        ///   contained in the heap, then the time complexities for the operations
        ///   <see cref="IHeap{T}"/> offers are: <br/> | Operations | Complexity | | :- | :- | | Add
        ///   | O(1) | | Contains | O(n) | | Merge{T} | O(m), m is other.Count | | Merge{T2} | O(m),
        ///   m is other.Count | | Min | O(1) | | Remove | O(n) | | RemoveMin | O(log(n)), amortized |
        /// </summary>
        /// <typeparam name="T">The type of the items held by the heap.</typeparam>
        /// <param name="cmp">The comparer the heap will use.</param>
        /// <param name="eqCmp">The equality comparer the heap will use.</param>
        /// <returns>A multi heap implemented as a Fibonacci heap.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static MultiHeap<T> NewFibonacciHeap<T>(IComparer<T> cmp, IEqualityComparer<T> eqCmp = null)
        {
            return new MultiHeap<T>(new FibonacciHeap<T, T>(cmp), eqCmp);
        }

        /**********************************************************************
		 * Pairing
		 **********************************************************************/

        /// <summary>
        ///   Returns a raw heap implemented as a pairing heap; see
        ///   https://en.wikipedia.org/wiki/Pairing_heap for details about pairing heaps. <br/> Heap
        ///   will have default priority comparer and default value equality comparer. <br/> If n is
        ///   the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(1) | | Contains | O(1) | | Merge{V, P} | O(1) | | Merge{V2, P2} |
        ///   O(m), m is other.Count | | Min | O(1) | | PriorityOf | O(1) | | Remove | O(log(n)),
        ///   amortized | | RemoveMin | O(log(n)), amortized | | Update | O(log(n)), amortized | |
        ///   UpdatePriorityOf | O(log(n)), amortized | | UpdateValue | O(1) |
        /// </summary>
        /// <remarks>
        ///   The returned heap is implemented as an auxiliary two pass pairing heap, as described
        ///   here: https://www.cise.ufl.edu/~sahni/dsaaj/enrich/c13/pairing.htm.
        /// </remarks>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <returns>A raw heap implemented as a pairing heap.</returns>
        public static PairingHeap<TVal, TPr> NewRawPairingHeap<TVal, TPr>() where TPr : IComparable<TPr>
        {
            return NewRawPairingHeap<TVal, TPr>(BetterComparer<TPr>.Default);
        }

        /// <summary>
        ///   Returns a raw heap implemented as a pairing heap; see
        ///   https://en.wikipedia.org/wiki/Pairing_heap for details about pairing heaps. <br/> Heap
        ///   will have given priority comparer and default value equality comparer. <br/> If n is
        ///   the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(1) | | Contains | O(1) | | Merge{V, P} | O(1) | | Merge{V2, P2} |
        ///   O(m), m is other.Count | | Min | O(1) | | PriorityOf | O(1) | | Remove | O(log(n)),
        ///   amortized | | RemoveMin | O(log(n)), amortized | | Update | O(log(n)), amortized | |
        ///   UpdatePriorityOf | O(log(n)), amortized | | UpdateValue | O(1) |
        /// </summary>
        /// <remarks>
        ///   The returned heap is implemented as an auxiliary two pass pairing heap, as described
        ///   here: https://www.cise.ufl.edu/~sahni/dsaaj/enrich/c13/pairing.htm.
        /// </remarks>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="cmp">The priority comparer the heap will use.</param>
        /// <returns>A raw heap implemented as a pairing heap.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static PairingHeap<TVal, TPr> NewRawPairingHeap<TVal, TPr>(IComparer<TPr> cmp)
        {
            return new PairingHeap<TVal, TPr>(cmp);
        }

        /// <summary>
        ///   Returns a unique heap implemented as a pairing heap; see
        ///   https://en.wikipedia.org/wiki/Pairing_heap for details about pairing heaps. <br/> Heap
        ///   will have default priority comparer and default value equality comparer if
        ///   <paramref name="eqCmp"/> is null or given value equality comparer otherwise. <br/> If n
        ///   is the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(1) | | Contains | O(1) | | Merge{V, P} | O(m), m is other.Count |
        ///   | Merge{V2, P2} | O(m), m is other.Count | | Min | O(1) | | PriorityOf | O(1) | |
        ///   Remove | O(log(n)), amortized | | RemoveMin | O(log(n)), amortized | | Update |
        ///   O(log(n)), amortized | | UpdatePriorityOf | O(log(n)), amortized | | UpdateValue | O(1) |
        /// </summary>
        /// <remarks>
        ///   The returned heap is implemented as an auxiliary two pass pairing heap, as described
        ///   here: https://www.cise.ufl.edu/~sahni/dsaaj/enrich/c13/pairing.htm.
        /// </remarks>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="eqCmp">The value equality comparer the heap will use.</param>
        /// <returns>A unique heap implemented as a pairing heap.</returns>
        public static UniqueHeap<TVal, TPr> NewPairingHeap<TVal, TPr>(IEqualityComparer<TVal> eqCmp = null)
            where TPr : IComparable<TPr>
        {
            return NewPairingHeap(BetterComparer<TPr>.Default, eqCmp);
        }

        /// <summary>
        ///   Returns a unique heap implemented as a pairing heap; see
        ///   https://en.wikipedia.org/wiki/Pairing_heap for details about pairing heaps. <br/> Heap
        ///   will have given priority comparer and default value equality comparer if
        ///   <paramref name="eqCmp"/> is null or given value equality comparer otherwise. <br/> If n
        ///   is the number of pairs contained in the heap, then the time complexities for the
        ///   operations <see cref="IHeap{TVal,TPr}"/> offers are: <br/> | Operations | Complexity |
        ///   | :- | :- | | Add | O(1) | | Contains | O(1) | | Merge{V, P} | O(m), m is other.Count |
        ///   | Merge{V2, P2} | O(m), m is other.Count | | Min | O(1) | | PriorityOf | O(1) | |
        ///   Remove | O(log(n)), amortized | | RemoveMin | O(log(n)), amortized | | Update |
        ///   O(log(n)), amortized | | UpdatePriorityOf | O(log(n)), amortized | | UpdateValue | O(1) |
        /// </summary>
        /// <remarks>
        ///   The returned heap is implemented as an auxiliary two pass pairing heap, as described
        ///   here: https://www.cise.ufl.edu/~sahni/dsaaj/enrich/c13/pairing.htm.
        /// </remarks>
        /// <typeparam name="TVal">The type of the values held by the heap.</typeparam>
        /// <typeparam name="TPr">The type of the priority associated with each value.</typeparam>
        /// <param name="cmp">The priority comparer the heap will use.</param>
        /// <param name="eqCmp">The value equality comparer the heap will use.</param>
        /// <returns>A unique heap implemented as a pairing heap.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static UniqueHeap<TVal, TPr> NewPairingHeap<TVal, TPr>(IComparer<TPr> cmp,
                                                                      IEqualityComparer<TVal> eqCmp = null)
        {
            return new UniqueHeap<TVal, TPr>(new PairingHeap<TVal, TPr>(cmp), eqCmp);
        }

        /// <summary>
        ///   Returns a multi heap implemented as a pairing heap; see
        ///   https://en.wikipedia.org/wiki/Pairing_heap for details about pairing heaps. <br/> Heap
        ///   will have default comparer and default equality comparer if <paramref name="eqCmp"/> is
        ///   null or given value equality comparer otherwise. <br/> If n is the number of items
        ///   contained in the heap, then the time complexities for the operations
        ///   <see cref="IHeap{T}"/> offers are: <br/> | Operations | Complexity | | :- | :- | | Add
        ///   | O(1) | | Contains | O(n) | | Merge{T} | O(m), m is other.Count | | Merge{T2} | O(m),
        ///   m is other.Count | | Min | O(1) | | Remove | O(n) | | RemoveMin | O(log(n)), amortized |
        /// </summary>
        /// <remarks>
        ///   The returned heap is implemented as an auxiliary two pass pairing heap, as described
        ///   here: https://www.cise.ufl.edu/~sahni/dsaaj/enrich/c13/pairing.htm.
        /// </remarks>
        /// <typeparam name="T">The type of the items held by the heap.</typeparam>
        /// <param name="eqCmp">The equality comparer the heap will use.</param>
        /// <returns>A multi heap implemented as a pairing heap.</returns>
        public static MultiHeap<T> NewPairingHeap<T>(IEqualityComparer<T> eqCmp = null) where T : IComparable<T>
        {
            return NewPairingHeap<T>(BetterComparer<T>.Default, eqCmp);
        }

        /// <summary>
        ///   Returns a multi heap implemented as a pairing heap; see
        ///   https://en.wikipedia.org/wiki/Pairing_heap for details about pairing heaps. <br/> Heap
        ///   will have given comparer and default equality comparer if <paramref name="eqCmp"/> is
        ///   null or given value equality comparer otherwise. <br/> If n is the number of items
        ///   contained in the heap, then the time complexities for the operations
        ///   <see cref="IHeap{T}"/> offers are: <br/> | Operations | Complexity | | :- | :- | | Add
        ///   | O(1) | | Contains | O(n) | | Merge{T} | O(m), m is other.Count | | Merge{T2} | O(m),
        ///   m is other.Count | | Min | O(1) | | Remove | O(n) | | RemoveMin | O(log(n)), amortized |
        /// </summary>
        /// <remarks>
        ///   The returned heap is implemented as an auxiliary two pass pairing heap, as described
        ///   here: https://www.cise.ufl.edu/~sahni/dsaaj/enrich/c13/pairing.htm.
        /// </remarks>
        /// <typeparam name="T">The type of the items held by the heap.</typeparam>
        /// <param name="cmp">The comparer the heap will use.</param>
        /// <param name="eqCmp">The equality comparer the heap will use.</param>
        /// <returns>A multi heap implemented as a pairing heap.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cmp"/> is null.</exception>
        public static MultiHeap<T> NewPairingHeap<T>(IComparer<T> cmp, IEqualityComparer<T> eqCmp = null)
        {
            return new MultiHeap<T>(new PairingHeap<T, T>(cmp), eqCmp);
        }
    }
}