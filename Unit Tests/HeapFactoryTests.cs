// File name: HeapFactoryTests.cs
// 
// Author(s): Alessio Parma <alessio.parma@gmail.com>
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

namespace UnitTests
{
    using DIBRIS.Hippie;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class HeapFactoryTests
    {
        /**********************************************************************
         * Array heap
         **********************************************************************/

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewRawArrayHeap_NullPriorityComparer()
        {
            HeapFactory.NewRawArrayHeap<A, B>(7, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewUniqueArrayHeap_NullPriorityComparer()
        {
            HeapFactory.NewArrayHeap<A, B>(7, null, EqualityComparer<A>.Default);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewMultiArrayHeap_NullPriorityComparer()
        {
            HeapFactory.NewArrayHeap(7, null, EqualityComparer<A>.Default);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NewRawArrayHeap_ZeroChildCount()
        {
            HeapFactory.NewRawArrayHeap<A, int>(0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NewRawArrayHeap_OneChildCount()
        {
            HeapFactory.NewRawArrayHeap<A, int>(1);
        }

        /**********************************************************************
         * Binary heap
         **********************************************************************/

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewRawBinaryHeap_NullPriorityComparer()
        {
            NewRawHeap_NullPriorityComparer(HeapFactory.NewRawBinaryHeap<A, B>);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewUniqueBinaryHeap_NullPriorityComparer()
        {
            NewUniqueHeap_NullPriorityComparer(HeapFactory.NewBinaryHeap);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewMultiBinaryHeap_NullPriorityComparer()
        {
            NewMultiHeap_NullPriorityComparer(HeapFactory.NewBinaryHeap<A>);
        }

        [Test]
        public void NewRawBinaryIntHeap_CheckDefaultPriorityComparer()
        {
            NewRawIntHeap_CheckDefaultComparer(HeapFactory.NewRawBinaryHeap<A, int>);
        }

        [Test]
        public void NewUniqueBinaryIntHeap_CheckDefaultPriorityComparer()
        {
            NewUniqueIntHeap_CheckDefaultComparer(HeapFactory.NewBinaryHeap<A, int>);
        }

        [Test]
        public void NewMultiBinaryIntHeap_CheckDefaultPriorityComparer()
        {
            NewMultiIntHeap_CheckDefaultComparer(HeapFactory.NewBinaryHeap);
        }

        /**********************************************************************
         * Binomial heap
         **********************************************************************/

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewRawBinomialHeap_NullPriorityComparer()
        {
            NewRawHeap_NullPriorityComparer(HeapFactory.NewRawBinomialHeap<A, B>);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewUniqueBinomialHeap_NullPriorityComparer()
        {
            NewUniqueHeap_NullPriorityComparer(HeapFactory.NewBinomialHeap);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewMultiBinomialHeap_NullPriorityComparer()
        {
            NewMultiHeap_NullPriorityComparer(HeapFactory.NewBinomialHeap<A>);
        }

        [Test]
        public void NewRawBinomialIntHeap_CheckDefaultPriorityComparer()
        {
            NewRawIntHeap_CheckDefaultComparer(HeapFactory.NewRawBinomialHeap<A, int>);
        }

        [Test]
        public void NewUniqueBinomialIntHeap_CheckDefaultPriorityComparer()
        {
            NewUniqueIntHeap_CheckDefaultComparer(HeapFactory.NewBinaryHeap<A, int>);
        }

        [Test]
        public void NewMultiBinomialIntHeap_CheckDefaultPriorityComparer()
        {
            NewMultiIntHeap_CheckDefaultComparer(HeapFactory.NewBinomialHeap);
        }

        /**********************************************************************
         * Fibonacci heap
         **********************************************************************/

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewRawFibonacciHeap_NullPriorityComparer()
        {
            NewRawHeap_NullPriorityComparer(HeapFactory.NewRawFibonacciHeap<A, B>);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewUniqueFibonacciHeap_NullPriorityComparer()
        {
            NewUniqueHeap_NullPriorityComparer(HeapFactory.NewFibonacciHeap);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewMultiFibonacciHeap_NullPriorityComparer()
        {
            NewMultiHeap_NullPriorityComparer(HeapFactory.NewFibonacciHeap<A>);
        }

        [Test]
        public void NewRawFibonacciIntHeap_CheckDefaultPriorityComparer()
        {
            NewRawIntHeap_CheckDefaultComparer(HeapFactory.NewRawFibonacciHeap<A, int>);
        }

        [Test]
        public void NewUniqueFibonacciIntHeap_CheckDefaultPriorityComparer()
        {
            NewUniqueIntHeap_CheckDefaultComparer(HeapFactory.NewFibonacciHeap<A, int>);
        }

        [Test]
        public void NewMultiFibonacciIntHeap_CheckDefaultPriorityComparer()
        {
            NewMultiIntHeap_CheckDefaultComparer(HeapFactory.NewFibonacciHeap);
        }

        /**********************************************************************
         * Pairing heap
         **********************************************************************/

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewRawPairingHeap_NullPriorityComparer()
        {
            NewRawHeap_NullPriorityComparer(HeapFactory.NewRawPairingHeap<A, B>);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewUniquePairingHeap_NullPriorityComparer()
        {
            NewUniqueHeap_NullPriorityComparer(HeapFactory.NewPairingHeap);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewMultiPairingHeap_NullPriorityComparer()
        {
            NewMultiHeap_NullPriorityComparer(HeapFactory.NewPairingHeap<A>);
        }

        [Test]
        public void NewRawPairingIntHeap_CheckDefaultPriorityComparer()
        {
            NewRawIntHeap_CheckDefaultComparer(HeapFactory.NewRawPairingHeap<A, int>);
        }

        [Test]
        public void NewUniquePairingIntHeap_CheckDefaultPriorityComparer()
        {
            NewUniqueIntHeap_CheckDefaultComparer(HeapFactory.NewPairingHeap<A, int>);
        }

        [Test]
        public void NewMultiPairingIntHeap_CheckDefaultPriorityComparer()
        {
            NewMultiIntHeap_CheckDefaultComparer(HeapFactory.NewPairingHeap);
        }

        /**********************************************************************
         * NewBinomialHeap
         **********************************************************************/

        [Test]
        public void NewBinomialHeap_IntHeap_WithDefaultPriorityComparer()
        {
            var heap = HeapFactory.NewBinomialHeap<string, int>();
            AssertHeapCorrectness(heap, BetterComparer<int>.Default, EqualityComparer<string>.Default);
        }

        [Test]
        public void NewBinomialHeap_IntHeap_WithCustomPriorityComparer()
        {
            var heap = HeapFactory.NewBinomialHeap<string, int>(Comparer<int>.Default);
            AssertHeapCorrectness(heap, Comparer<int>.Default, EqualityComparer<string>.Default);
        }

        [Test]
        public void NewBinomialHeap_StringHeap_WithDefaultPriorityComparer()
        {
            var heap = HeapFactory.NewBinomialHeap<string, string>();
            AssertHeapCorrectness(heap, BetterComparer<string>.Default, EqualityComparer<string>.Default);
        }

        [Test]
        public void NewBinomialHeap_StringHeap_WithCustomPriorityComparer()
        {
            var heap = HeapFactory.NewBinomialHeap<string, string>(Comparer<string>.Default);
            AssertHeapCorrectness(heap, Comparer<string>.Default, EqualityComparer<string>.Default);
        }

        /**********************************************************************
         * NewRawBinaryHeap
         **********************************************************************/

        [Test]
        public void NewRawBinaryHeap_IntHeap_WithDefaultPriorityComparer()
        {
            var heap = HeapFactory.NewRawBinaryHeap<int, int>();
            AssertHeapCorrectness(heap, BetterComparer<int>.Default);
        }

        [Test]
        public void NewRawBinaryHeap_IntHeap_WithCustomPriorityComparer()
        {
            var heap = HeapFactory.NewRawBinaryHeap<int, int>(Comparer<int>.Default);
            AssertHeapCorrectness(heap, Comparer<int>.Default);
        }

        [Test]
        public void NewRawBinaryHeap_StringHeap_WithDefaultPriorityComparer()
        {
            var heap = HeapFactory.NewRawBinaryHeap<string, string>();
            AssertHeapCorrectness(heap, BetterComparer<string>.Default);
        }

        [Test]
        public void NewRawBinaryHeap_StringHeap_WithCustomPriorityComparer()
        {
            var heap = HeapFactory.NewRawBinaryHeap<string, string>(Comparer<string>.Default);
            AssertHeapCorrectness(heap, Comparer<string>.Default);
        }

        /**********************************************************************
         * Private methods
         **********************************************************************/

        private static void AssertHeapCorrectness<TVal, TPr>(IRawHeap<TVal, TPr> heap, IComparer<TPr> cmp)
        {
            Assert.NotNull(heap);
            Assert.AreEqual(cmp, heap.Comparer);
            Assert.AreEqual(0, heap.Count);
        }

        private static void AssertHeapCorrectness<TVal, TPr>(IHeap<TVal, TPr> heap, IComparer<TPr> cmp, IEqualityComparer<TVal> eqCmp)
        {
            Assert.NotNull(heap);
            Assert.AreEqual(cmp, heap.Comparer);
            Assert.AreEqual(eqCmp, heap.EqualityComparer);
            Assert.AreEqual(0, heap.Count);
        }

        private static void AssertHeapCorrectness<TItem>(IHeap<TItem> heap, IComparer<TItem> cmp, IEqualityComparer<TItem> eqCmp)
        {
            Assert.NotNull(heap);
            Assert.AreEqual(cmp, heap.Comparer);
            Assert.AreEqual(eqCmp, heap.EqualityComparer);
            Assert.AreEqual(0, heap.Count);
        }

        private static void NewRawHeap_NullPriorityComparer(Func<IComparer<B>, IRawHeap<A, B>> ctor)
        {
            ctor(null);
        }

        private static void NewUniqueHeap_NullPriorityComparer(Func<IComparer<B>, IEqualityComparer<A>, IHeap<A, B>> ctor)
        {
            ctor(null, EqualityComparer<A>.Default);
        }

        private static void NewMultiHeap_NullPriorityComparer(Func<IComparer<A>, IEqualityComparer<A>, IHeap<A>> ctor)
        {
            ctor(null, EqualityComparer<A>.Default);
        }

        private static void NewRawIntHeap_CheckDefaultComparer(Func<IRawHeap<A, int>> ctor)
        {
            var heap = ctor();
            AssertHeapCorrectness(heap, BetterComparer<int>.Default);
        }

        private static void NewUniqueIntHeap_CheckDefaultComparer(Func<IEqualityComparer<A>, IHeap<A, int>> ctor)
        {
            var heap = ctor(EqualityComparer<A>.Default);
            AssertHeapCorrectness(heap, BetterComparer<int>.Default, EqualityComparer<A>.Default);
        }

        private static void NewMultiIntHeap_CheckDefaultComparer(Func<IEqualityComparer<int>, IHeap<int>> ctor)
        {
            var heap = ctor(EqualityComparer<int>.Default);
            AssertHeapCorrectness(heap, BetterComparer<int>.Default, EqualityComparer<int>.Default);
        }

        private abstract class A
        {
        }

        private abstract class B
        {
        }
    }
}
