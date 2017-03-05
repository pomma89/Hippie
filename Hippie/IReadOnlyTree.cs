// IReadOnlyTree.cs
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
    ///   A simple interface modeling a tree, whose nodes have a variable number of children.
    /// </summary>
    /// <typeparam name="TVal">The type of the value contained in the root of this tree.</typeparam>
    /// <typeparam name="TPr">The type of the priority contained in the root of this tree.</typeparam>
    public interface IReadOnlyTree<TVal, TPr>
    {
        /// <summary>
        ///   The value contained in the root of this tree.
        /// </summary>
        TVal Value { get; }

        /// <summary>
        ///   The priority contained in the root of this tree.
        /// </summary>
        TPr Priority { get; }

        /// <summary>
        ///   The node this tree is child of.
        /// </summary>
        IReadOnlyTree<TVal, TPr> Parent { get; }

        /// <summary>
        ///   The nodes which have this node as parent.
        /// </summary>
        IEnumerable<IReadOnlyTree<TVal, TPr>> Children { get; }

        /// <summary>
        ///   Visits the tree in a breadth-first fashion.
        /// </summary>
        /// <returns>Each tree node in breadth-first order.</returns>
        IEnumerable<IReadOnlyTree<TVal, TPr>> BreadthFirstVisit();

        /// <summary>
        ///   Applies given function on each node in a breadth-first fashion.
        /// </summary>
        /// <param name="visitor">The function applied on each node.</param>
        void BreadthFirstVisit(Action<IReadOnlyTree<TVal, TPr>> visitor);

        /// <summary>
        ///   Applies given function on each node in a breadth-first fashion, and returns in that
        ///   order the results produced during the visit. Result produced by a node is passed as a
        ///   parameter to all its children.
        /// </summary>
        /// <param name="visitor">The function applied on each node.</param>
        /// <param name="start">The value passed to the root.</param>
        /// <returns>
        ///   The results, in breadth-first order, produced by <paramref name="visitor"/> while
        ///   visiting the tree.
        /// </returns>
        IEnumerable<TRet> BreadthFirstVisit<TRet>(Func<IReadOnlyTree<TVal, TPr>, TRet, TRet> visitor, TRet start);

        /// <summary>
        ///   Visits the tree in a depth-first fashion.
        /// </summary>
        /// <returns>Each tree node in depth-first order.</returns>
        IEnumerable<IReadOnlyTree<TVal, TPr>> DepthFirstVisit();

        /// <summary>
        ///   Applies given function on each node in a depth-first fashion.
        /// </summary>
        /// <param name="visitor">The function applied on each node.</param>
        void DepthFirstVisit(Action<IReadOnlyTree<TVal, TPr>> visitor);

        /// <summary>
        ///   Applies given function on each node in a depth-first fashion, and returns in that order
        ///   the results produced during the visit. Result produced by a node is passed as a
        ///   parameter to all its children.
        /// </summary>
        /// <param name="visitor">The function applied on each node.</param>
        /// <param name="start">The value passed to the root.</param>
        /// <returns>
        ///   The results, in depth-first order, produced by <paramref name="visitor"/> while
        ///   visiting the tree.
        /// </returns>
        IEnumerable<TRet> DepthFirstVisit<TRet>(Func<IReadOnlyTree<TVal, TPr>, TRet, TRet> visitor, TRet start);
    }

    /// <summary>
    ///   A simple interface modeling a tree, whose nodes have a variable number of children.
    /// </summary>
    /// <typeparam name="TItem">The type of the item contained in the root of this tree.</typeparam>
    public interface IReadOnlyTree<TItem>
    {
        /// <summary>
        ///   The item contained in the root of this tree.
        /// </summary>
        TItem Item { get; }

        /// <summary>
        ///   The node this tree is child of.
        /// </summary>
        IReadOnlyTree<TItem> Parent { get; }

        /// <summary>
        ///   The nodes which have this node as parent.
        /// </summary>
        IEnumerable<IReadOnlyTree<TItem>> Children { get; }

        /// <summary>
        ///   Visits the tree in a breadth-first fashion.
        /// </summary>
        /// <returns>Each tree node in breadth-first order.</returns>
        IEnumerable<IReadOnlyTree<TItem>> BreadthFirstVisit();

        /// <summary>
        ///   Applies given function on each node in a breadth-first fashion.
        /// </summary>
        /// <param name="visitor">The function applied on each node.</param>
        void BreadthFirstVisit(Action<IReadOnlyTree<TItem>> visitor);

        /// <summary>
        ///   Applies given function on each node in a breadth-first fashion, and returns in that
        ///   order the results produced during the visit. Result produced by a node is passed as a
        ///   parameter to all its children.
        /// </summary>
        /// <param name="visitor">The function applied on each node.</param>
        /// <param name="start">The value passed to the root.</param>
        /// <returns>
        ///   The results, in breadth-first order, produced by <paramref name="visitor"/> while
        ///   visiting the tree.
        /// </returns>
        IEnumerable<TRet> BreadthFirstVisit<TRet>(Func<IReadOnlyTree<TItem>, TRet, TRet> visitor, TRet start);

        /// <summary>
        ///   Visits the tree in a depth-first fashion.
        /// </summary>
        /// <returns>Each tree node in depth-first order.</returns>
        IEnumerable<IReadOnlyTree<TItem>> DepthFirstVisit();

        /// <summary>
        ///   Applies given function on each node in a depth-first fashion.
        /// </summary>
        /// <param name="visitor">The function applied on each node.</param>
        void DepthFirstVisit(Action<IReadOnlyTree<TItem>> visitor);

        /// <summary>
        ///   Applies given function on each node in a depth-first fashion, and returns in that order
        ///   the results produced during the visit. Result produced by a node is passed as a
        ///   parameter to all its children.
        /// </summary>
        /// <param name="visitor">The function applied on each node.</param>
        /// <param name="start">The value passed to the root.</param>
        /// <returns>
        ///   The results, in depth-first order, produced by <paramref name="visitor"/> while
        ///   visiting the tree.
        /// </returns>
        IEnumerable<TRet> DepthFirstVisit<TRet>(Func<IReadOnlyTree<TItem>, TRet, TRet> visitor, TRet start);
    }
}