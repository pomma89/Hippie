// MultiHeapTests.cs
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

namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using DIBRIS.Hippie;
    using NUnit.Framework;

    public abstract class MultiHeapTests : HeapTestsBase
    {
        protected const int ManyIntItemsCount = 6120;
        private const int FewIntItemsCount = ManyIntItemsCount / 100;
        private const int RandomTestsRepetitionCount = 5;

        private readonly IHeap<int> _refIntHeap = HeapFactory.NewBinaryHeap<int>();
        private IHeap<int> _intHeap;

        protected abstract IHeap<T> GetHeap<T>() where T : IComparable<T>;

        protected abstract IHeap<T> GetHeap<T>(IEqualityComparer<T> eqCmp) where T : IComparable<T>;

        protected abstract IHeap<T> GetHeap<T>(IComparer<T> cmp);

        [SetUp]
        public void SetUp()
        {
            _intHeap = GetHeap<int>();
        }

        [TearDown]
        public void TearDown()
        {
            _refIntHeap.Clear();
            _intHeap = null;
        }

        /**********************************************************************
		 * Add
		 **********************************************************************/

        [Test]
        public void Add_FewOrderedIntItems()
        {
            Add_MainTest(AddOrderedIntItems, FewIntItemsCount);
        }

        [Test]
        public void Add_FewReverseOrderedIntItems()
        {
            Add_MainTest(AddReverseOrderedIntItems, FewIntItemsCount);
        }

        [Test]
        [Repeat(RandomTestsRepetitionCount)]
        public void Add_FewRandomIntItems()
        {
            Add_MainTest(AddRandomIntItems, FewIntItemsCount);
        }

        [Test]
        public void Add_FewSameIntItems()
        {
            Add_MainTest(AddSameIntItems, FewIntItemsCount);
        }

        [Test]
        public void Add_ManyOrderedIntItems()
        {
            Add_MainTest(AddOrderedIntItems);
        }

        [Test]
        public void Add_ManyReverseOrderedIntItems()
        {
            Add_MainTest(AddReverseOrderedIntItems);
        }

        [Test]
        [Repeat(RandomTestsRepetitionCount)]
        public void Add_ManyRandomIntItems()
        {
            Add_MainTest(AddRandomIntItems);
        }

        [Test]
        public void Add_ManySameIntItems()
        {
            Add_MainTest(AddSameIntItems);
        }

        private void Add_MainTest(Action<int, IHeap<int>> addMethod, int count = ManyIntItemsCount)
        {
            addMethod(count, null);
            AssertSameContents(_refIntHeap, _intHeap);
        }

        /**********************************************************************
		 * Clear
		 **********************************************************************/

        [Test]
        public void Clear_FewOrderedIntItems()
        {
            Clear_MainTest(AddOrderedIntItems, FewIntItemsCount);
        }

        [Test]
        public void Clear_FewReverseOrderedIntItems()
        {
            Clear_MainTest(AddReverseOrderedIntItems, FewIntItemsCount);
        }

        [Test]
        [Repeat(RandomTestsRepetitionCount)]
        public void Clear_FewRandomIntItems()
        {
            Clear_MainTest(AddRandomIntItems, FewIntItemsCount);
        }

        [Test]
        public void Clear_FewSameIntItems()
        {
            Clear_MainTest(AddSameIntItems, FewIntItemsCount);
        }

        [Test]
        public void Clear_ManyOrderedIntItems()
        {
            Clear_MainTest(AddOrderedIntItems);
        }

        [Test]
        public void Clear_ManyReverseOrderedIntItems()
        {
            Clear_MainTest(AddReverseOrderedIntItems);
        }

        [Test]
        [Repeat(RandomTestsRepetitionCount)]
        public void Clear_ManyRandomIntItems()
        {
            Clear_MainTest(AddRandomIntItems);
        }

        [Test]
        public void Clear_ManySameIntItems()
        {
            Clear_MainTest(AddSameIntItems);
        }

        private void Clear_MainTest(Action<int, IHeap<int>> addMethod, int count = ManyIntItemsCount)
        {
            addMethod(count, null);
            _intHeap.Clear();
            foreach (var item in _refIntHeap)
                Assert.False(_intHeap.Contains(item));
            Assert.AreEqual(0, _intHeap.Count);
        }

        /**********************************************************************
		 * Contains
		 **********************************************************************/

        [Test]
        public void Contains_FewOrderedIntItems()
        {
            Contains_MainTest(AddOrderedIntItems, FewIntItemsCount);
        }

        [Test]
        public void Contains_FewReverseOrderedIntItems()
        {
            Contains_MainTest(AddReverseOrderedIntItems, FewIntItemsCount);
        }

        [Test]
        [Repeat(RandomTestsRepetitionCount)]
        public void Contains_FewRandomIntItems()
        {
            Contains_MainTest(AddRandomIntItems, FewIntItemsCount);
        }

        [Test]
        public void Contains_FewSameIntItems()
        {
            Contains_MainTest(AddSameIntItems, FewIntItemsCount);
        }

        [Test]
        public void Contains_ManyOrderedIntItems()
        {
            Contains_MainTest(AddOrderedIntItems);
        }

        [Test]
        public void Contains_ManyReverseOrderedIntItems()
        {
            Contains_MainTest(AddReverseOrderedIntItems);
        }

        [Test]
        [Repeat(RandomTestsRepetitionCount)]
        public void Contains_ManyRandomIntItems()
        {
            Contains_MainTest(AddRandomIntItems);
        }

        [Test]
        public void Contains_ManySameIntItems()
        {
            Contains_MainTest(AddSameIntItems);
        }

        private void Contains_MainTest(Action<int, IHeap<int>> addMethod, int count = ManyIntItemsCount)
        {
            addMethod(count, null);
            foreach (var item in _refIntHeap)
                Assert.True(_intHeap.Contains(item));
        }

        /**********************************************************************
		 * Remove
		 **********************************************************************/

        [Test]
        public void Remove_FewOrderedIntItems()
        {
            Remove_MainTest(AddOrderedIntItems, FewIntItemsCount);
        }

        [Test]
        public void Remove_FewReverseOrderedIntItems()
        {
            Remove_MainTest(AddReverseOrderedIntItems, FewIntItemsCount);
        }

        [Test]
        [Repeat(RandomTestsRepetitionCount)]
        public void Remove_FewRandomIntItems()
        {
            Remove_MainTest(AddRandomIntItems, FewIntItemsCount);
        }

        [Test]
        public void Remove_FewSameIntItems()
        {
            Remove_MainTest(AddSameIntItems, FewIntItemsCount);
        }

        [Test]
        public void Remove_ManyOrderedIntItems()
        {
            Remove_MainTest(AddOrderedIntItems);
        }

        [Test]
        public void Remove_ManyReverseOrderedIntItems()
        {
            Remove_MainTest(AddReverseOrderedIntItems);
        }

        [Test]
        [Repeat(RandomTestsRepetitionCount)]
        public void Remove_ManyRandomIntItems()
        {
            Remove_MainTest(AddRandomIntItems);
        }

        [Test]
        public void Remove_ManySameIntItems()
        {
            Remove_MainTest(AddSameIntItems);
        }

        [Test]
        public void Remove_NotExistingItem()
        {
            Assert.False(_intHeap.Remove(0));
        }

        [Test]
        public void Remove_AddRemove_NotExistingItem()
        {
            _intHeap.Add(0);
            Assert.True(_intHeap.Remove(0));
            Assert.False(_intHeap.Remove(0));
        }

        private void Remove_MainTest(Action<int, IHeap<int>> addMethod, int count = ManyIntItemsCount)
        {
            addMethod(count, null);
            foreach (var item in _refIntHeap)
                Assert.True(_intHeap.Remove(item));
            Assert.AreEqual(0, _intHeap.Count);
        }

        /**********************************************************************
		 * Merge
		 **********************************************************************/

        [Test]
        public void Merge_DifferentHeapType_ManyOrderedIntItems()
        {
            Merge_DifferentHeapType_MainTest(AddOrderedIntItems);
        }

        [Test]
        public void Merge_DifferentHeapType_ManyReverseOrderedIntItems()
        {
            Merge_DifferentHeapType_MainTest(AddReverseOrderedIntItems);
        }

        [Test]
        [Repeat(RandomTestsRepetitionCount)]
        public void Merge_DifferentHeapType_ManyRandomIntItems()
        {
            Merge_DifferentHeapType_MainTest(AddRandomIntItems);
        }

        [Test]
        public void Merge_DifferentHeapType_ManySameIntItems()
        {
            Merge_DifferentHeapType_MainTest(AddSameIntItems);
        }

        [Test]
        public void Merge_SameHeapType_ManyOrderedIntItems()
        {
            Merge_SameHeapType_MainTest(AddOrderedIntItems);
        }

        [Test]
        public void Merge_SameHeapType_ManyReverseOrderedIntItems()
        {
            Merge_SameHeapType_MainTest(AddReverseOrderedIntItems);
        }

        [Test]
        [Repeat(RandomTestsRepetitionCount)]
        public void Merge_SameHeapType_ManyRandomIntItems()
        {
            Merge_SameHeapType_MainTest(AddRandomIntItems);
        }

        [Test]
        public void Merge_SameHeapType_ManySameIntItems()
        {
            Merge_SameHeapType_MainTest(AddSameIntItems);
        }

        [Test]
        public void Merge_SameHeap()
        {
            AddRandomIntItems();
            _intHeap.Merge(_intHeap);
            AssertSameContents(_refIntHeap, _intHeap);
        }

        [Test]
        public void Merge_Covariant()
        {
            var h1 = HeapFactory.NewBinaryHeap<A>();
            var h2 = HeapFactory.NewBinaryHeap<B>();

            h1.Add(new A(0));
            h2.Add(new B(1));
            h1.Merge(h2);

            Assert.AreEqual(0, h2.Count);
            Assert.AreEqual(2, h1.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Merge_NullOtherHeap()
        {
            _intHeap.Merge<int>(null);
        }

        [Test]
        public void Merge_WithEmptyHeap()
        {
            AddRandomIntItems();
            var otherIntHeap = HeapFactory.NewArrayHeap<int>(3);
            _intHeap.Merge(otherIntHeap);
            AssertSameContents(_refIntHeap, _intHeap);
        }

        [Test]
        public void Merge_EmptyHeap_WithFullHeap()
        {
            var otherIntHeap = HeapFactory.NewArrayHeap<int>(3);
            AddRandomIntItems(ManyIntItemsCount, otherIntHeap);
            _intHeap.Merge(otherIntHeap);
            AssertSameContents(_refIntHeap, _intHeap);
        }

        private void Merge_DifferentHeapType_MainTest(Action<int, IHeap<int>> addMethod, int count = ManyIntItemsCount)
        {
            addMethod(count, null);
            var otherIntHeap = HeapFactory.NewArrayHeap<int>(30);
            addMethod(count, otherIntHeap);
            _intHeap.Merge(otherIntHeap);
            Assert.AreEqual(0, otherIntHeap.Count);

            addMethod(count, null);
            addMethod(count, otherIntHeap);
            _intHeap.Merge(otherIntHeap);
            Assert.AreEqual(0, otherIntHeap.Count);
            AssertSameContents(_refIntHeap, _intHeap);
        }

        private void Merge_SameHeapType_MainTest(Action<int, IHeap<int>> addMethod, int count = ManyIntItemsCount)
        {
            addMethod(count, null);
            var otherIntHeap = GetHeap<int>();
            addMethod(count, otherIntHeap);
            _intHeap.Merge(otherIntHeap);
            Assert.AreEqual(0, otherIntHeap.Count);

            addMethod(count, null);
            addMethod(count, otherIntHeap);
            _intHeap.Merge(otherIntHeap);
            Assert.AreEqual(0, otherIntHeap.Count);
            AssertSameContents(_refIntHeap, _intHeap);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Merge_WithFullHeap_WithDifferentComparer()
        {
            AddRandomIntItems(ManyIntItemsCount, _intHeap);
            var heap = HeapFactory.NewArrayHeap(21, new ReversedIntComparer());
            AddRandomIntItems(ManyIntItemsCount, heap);
            _intHeap.Merge(heap);
        }

        [Test]
        public void Merge_WithFullHeap_WithSameValues()
        {
            AddRandomIntItems(ManyIntItemsCount, _intHeap); // Method also adds values to RefIntHeap
            _intHeap.Merge(_refIntHeap);
            Assert.AreEqual(2 * ManyIntItemsCount, _intHeap.Count);
        }

        /**********************************************************************
		 * HeapSort
		 **********************************************************************/

        [Test]
        public void HeapSort_FewOrderedIntItems()
        {
            HeapSort_MainTest(AddOrderedIntItems, FewIntItemsCount);
        }

        [Test]
        public void HeapSort_FewReverseOrderedIntItems()
        {
            HeapSort_MainTest(AddReverseOrderedIntItems, FewIntItemsCount);
        }

        [Test]
        [Repeat(RandomTestsRepetitionCount)]
        public void HeapSort_FewRandomIntItems()
        {
            HeapSort_MainTest(AddRandomIntItems, FewIntItemsCount);
        }

        [Test]
        public void HeapSort_FewSameIntItems()
        {
            HeapSort_MainTest(AddSameIntItems, FewIntItemsCount);
        }

        [Test]
        public void HeapSort_ManyOrderedIntItems()
        {
            HeapSort_MainTest(AddOrderedIntItems);
        }

        [Test]
        public void HeapSort_ManyReverseOrderedIntItems()
        {
            HeapSort_MainTest(AddReverseOrderedIntItems);
        }

        [Test]
        [Repeat(RandomTestsRepetitionCount)]
        public void HeapSort_ManyRandomIntItems()
        {
            HeapSort_MainTest(AddRandomIntItems);
        }

        [Test]
        public void HeapSort_ManySameIntItems()
        {
            HeapSort_MainTest(AddSameIntItems);
        }

        private void HeapSort_MainTest(Action<int, IHeap<int>> addMethod, int count = ManyIntItemsCount)
        {
            addMethod(count, null);
            var list = _refIntHeap.ToList();
            list.Sort();
            foreach (var item in list)
                Assert.AreEqual(item, _intHeap.RemoveMin());
            Assert.AreEqual(0, _intHeap.Count);
        }

        /**********************************************************************
		 * ToReadOnlyForest
		 **********************************************************************/

        [Test]
        public void ToReadOnlyForest_ResultNotNull_EmptyHeap()
        {
            Assert.NotNull(_intHeap.ToReadOnlyForest());
        }

        [Test]
        public void ToReadOnlyForest_ResultNotNull_FullHeap()
        {
            AddRandomIntItems();
            Assert.NotNull(_intHeap.ToReadOnlyForest());
        }

        [Test]
        public void ToReadOnlyForest_OneElement()
        {
            _intHeap.Add(0);
            var f = _intHeap.ToReadOnlyForest().ToList();
            Assert.AreEqual(1, f.Count);
            var t = f[0];
            Assert.NotNull(t);
            Assert.Null(t.Parent);
            Assert.AreEqual(0, t.Item);
            Assert.NotNull(t.Children);
            Assert.AreEqual(0, t.Children.Count());
        }

        [Test]
        public void ToReadOnlyForest_Generic_ManyOrderedIntItems()
        {
            ToReadOnlyForest_Generic_MainTest(AddOrderedIntItems);
        }

        [Test]
        public void ToReadOnlyForest_Generic_ManyReverseOrderedIntItems()
        {
            ToReadOnlyForest_Generic_MainTest(AddReverseOrderedIntItems);
        }

        [Test]
        [Repeat(RandomTestsRepetitionCount)]
        public void ToReadOnlyForest_Generic_ManyRandomIntItems()
        {
            ToReadOnlyForest_Generic_MainTest(AddRandomIntItems);
        }

        [Test]
        public void ToReadOnlyForest_Generic_ManySameIntItems()
        {
            ToReadOnlyForest_Generic_MainTest(AddSameIntItems);
        }

        private void ToReadOnlyForest_Generic_MainTest(Action<int, IHeap<int>> addMethod, int count = ManyIntItemsCount)
        {
            addMethod(count, null);
            var items = new HashSet<int>(_intHeap);
            var forest = _intHeap.ToReadOnlyForest().ToList();
            // Breadth-first, no action
            var trees = forest.SelectMany(t => t.BreadthFirstVisit()).ToList();
            Assert.AreEqual(count, trees.Count);
            foreach (var t in trees)
            {
                Assert.True(items.Contains(t.Item));
            }
            // Depth-first, no action
            trees = forest.SelectMany(t => t.DepthFirstVisit()).ToList();
            Assert.AreEqual(count, trees.Count);
            foreach (var t in trees)
            {
                Assert.True(items.Contains(t.Item));
            }
            // Breadth-first, with selector
            var res = forest.SelectMany(t => t.BreadthFirstVisit((n, a) => n.Item, 0)).ToList();
            Assert.AreEqual(count, res.Count);
            foreach (var r in res)
            {
                Assert.True(items.Contains(r));
            }
            // Depth-first, with selector
            res = forest.SelectMany(t => t.DepthFirstVisit((n, a) => n.Item, 0)).ToList();
            Assert.AreEqual(count, res.Count);
            foreach (var r in res)
            {
                Assert.True(items.Contains(r));
            }
            // Breadth-first, with action
            var set = new HashSet<int>();
            // ReSharper disable AccessToModifiedClosure
            forest.ForEach(t => t.BreadthFirstVisit(n => set.Add(n.Item)));
            // ReSharper restore AccessToModifiedClosure
            Assert.AreEqual(items.Count, set.Count);
            foreach (var s in set)
            {
                Assert.True(items.Contains(s));
            }
            // Depth-first, with action
            set = new HashSet<int>();
            forest.ForEach(t => t.DepthFirstVisit(n => set.Add(n.Item)));
            Assert.AreEqual(items.Count, set.Count);
            foreach (var s in set)
            {
                Assert.True(items.Contains(s));
            }
        }

        /**********************************************************************
		 * Fake int equality comparer
		 **********************************************************************/

        [Test]
        public void Add_FakeIntEqualityComparer()
        {
            _intHeap = GetHeap(new FakeIntEqualityComparer());
            _intHeap.Add(1);
            _intHeap.Add(2);
            _intHeap.Add(3);
            Assert.AreEqual(3, _intHeap.Count);
        }

        [Test]
        public void Contains_FakeIntEqualityComparer()
        {
            _intHeap = GetHeap(new FakeIntEqualityComparer());
            _intHeap.Add(1);
            Assert.True(_intHeap.Contains(1));
            Assert.True(_intHeap.Contains(10));
            Assert.True(_intHeap.Contains(100));
            _intHeap.Add(5);
            Assert.True(_intHeap.Contains(1));
            Assert.True(_intHeap.Contains(10));
            Assert.True(_intHeap.Contains(100));
            Assert.True(_intHeap.Contains(5));
            Assert.True(_intHeap.Contains(50));
            Assert.True(_intHeap.Contains(500));
        }

        [Test]
        public void Remove_FakeIntEqualityComparer()
        {
            _intHeap = GetHeap(new FakeIntEqualityComparer());
            _intHeap.Add(1);
            _intHeap.Remove(10);
            Assert.IsEmpty(_intHeap);
            _intHeap.Add(5);
            _intHeap.Remove(5);
            Assert.IsEmpty(_intHeap);
        }

        /**********************************************************************
		 * Reversed int comparer
		 **********************************************************************/

        [Test]
        public void Add_ReversedIntComparer()
        {
            _intHeap = GetHeap(new ReversedIntComparer());
            _intHeap.Add(1);
            _intHeap.Add(2);
            _intHeap.Add(3);
            Assert.AreEqual(3, _intHeap.RemoveMin());
            Assert.AreEqual(2, _intHeap.RemoveMin());
            Assert.AreEqual(1, _intHeap.RemoveMin());
        }

        /**********************************************************************
		 * Private methods
		 **********************************************************************/

        private static void AssertSameContents<T>(IHeap<T> refHeap, IHeap<T> heap)
            where T : IComparable<T>
        {
            Assert.AreEqual(refHeap.Count, heap.Count);

            foreach (var p in refHeap)
                Assert.True(heap.Contains(p));

            foreach (var p in heap)
                Assert.True(refHeap.Contains(p));

            while (refHeap.Count != 0)
            {
                Assert.AreEqual(refHeap.Min, heap.Min);
                Assert.AreEqual(refHeap.RemoveMin(), heap.RemoveMin());
                Assert.AreEqual(refHeap.Count, heap.Count);
            }

            Assert.AreEqual(0, heap.Count);
        }

        private void AddOrderedIntItems(int count = ManyIntItemsCount, IHeap<int> heap = null)
        {
            Debug.Assert(count <= ManyIntItemsCount);
            heap = heap ?? _intHeap;
            for (var i = 0; i < count / 2; ++i)
            {
                // Values are added twice, because we want duplicates
                _refIntHeap.Add(i);
                _refIntHeap.Add(i);
                heap.Add(i);
                heap.Add(i);
            }
        }

        private void AddReverseOrderedIntItems(int count = ManyIntItemsCount, IHeap<int> heap = null)
        {
            Debug.Assert(count <= ManyIntItemsCount);
            heap = heap ?? _intHeap;
            for (var i = count / 2; i > 0; --i)
            {
                // Values are added twice, because we want duplicates
                _refIntHeap.Add(i);
                _refIntHeap.Add(i);
                heap.Add(i);
                heap.Add(i);
            }
        }

        private void AddRandomIntItems(int count = ManyIntItemsCount, IHeap<int> heap = null)
        {
            Debug.Assert(count <= ManyIntItemsCount);
            heap = heap ?? _intHeap;
            for (var i = 0; i < count; ++i)
            {
                var r = Rand.Next(0, 50); // We want duplicates, if possible
                _refIntHeap.Add(r);
                heap.Add(r);
            }
        }

        private void AddSameIntItems(int count = ManyIntItemsCount, IHeap<int> heap = null)
        {
            Debug.Assert(count <= ManyIntItemsCount);
            heap = heap ?? _intHeap;
            for (var i = 0; i < count; ++i)
            {
                _refIntHeap.Add(0);
                heap.Add(0);
            }
        }
    }

    public sealed class MultiArrayHeapTests : MultiHeapTests
    {
        protected override IHeap<T> GetHeap<T>()
        {
            return HeapFactory.NewArrayHeap<T>(7);
        }

        protected override IHeap<T> GetHeap<T>(IEqualityComparer<T> eqCmp)
        {
            return HeapFactory.NewArrayHeap(7, eqCmp);
        }

        protected override IHeap<T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewArrayHeap(7, cmp);
        }
    }

    public sealed class MultiBinaryHeapTests : MultiHeapTests
    {
        protected override IHeap<T> GetHeap<T>()
        {
            return HeapFactory.NewBinaryHeap<T>();
        }

        protected override IHeap<T> GetHeap<T>(IEqualityComparer<T> eqCmp)
        {
            return HeapFactory.NewBinaryHeap(eqCmp);
        }

        protected override IHeap<T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewBinaryHeap(cmp);
        }
    }

    public sealed class MultiBinomialHeapTests : MultiHeapTests
    {
        protected override IHeap<T> GetHeap<T>()
        {
            return HeapFactory.NewBinomialHeap<T>();
        }

        protected override IHeap<T> GetHeap<T>(IEqualityComparer<T> eqCmp)
        {
            return HeapFactory.NewBinomialHeap(eqCmp);
        }

        protected override IHeap<T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewBinomialHeap(cmp);
        }
    }

    public sealed class MultiFibonacciHeapTests : MultiHeapTests
    {
        protected override IHeap<T> GetHeap<T>()
        {
            return HeapFactory.NewFibonacciHeap<T>();
        }

        protected override IHeap<T> GetHeap<T>(IEqualityComparer<T> eqCmp)
        {
            return HeapFactory.NewFibonacciHeap(eqCmp);
        }

        protected override IHeap<T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewFibonacciHeap(cmp);
        }
    }

    public sealed class MultiPairingHeapTests : MultiHeapTests
    {
        protected override IHeap<T> GetHeap<T>()
        {
            return HeapFactory.NewPairingHeap<T>();
        }

        protected override IHeap<T> GetHeap<T>(IEqualityComparer<T> eqCmp)
        {
            return HeapFactory.NewPairingHeap(eqCmp);
        }

        protected override IHeap<T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewPairingHeap(cmp);
        }
    }

    public abstract class StableMultiHeapTests : MultiHeapTests
    {
        [TestCase(1)]
        [TestCase(ManyIntItemsCount / 2)]
        [TestCase(ManyIntItemsCount)]
        public void Add_SamePriority(int valueCount)
        {
            var heap = GetHeap<TheStrangeDuo>();
            for (var i = 0; i < valueCount; ++i)
            {
                heap.Add(new TheStrangeDuo(i, 0));
            }
            for (var i = 0; i < valueCount; ++i)
            {
                Assert.AreEqual(i, heap.Min.Aa);
                Assert.AreEqual(0, heap.Min.Bb);
                var min = heap.RemoveMin();
                Assert.AreEqual(i, min.Aa);
                Assert.AreEqual(0, min.Bb);
            }
        }

        private struct TheStrangeDuo : IComparable<TheStrangeDuo>
        {
            public readonly int Aa;
            public readonly int Bb;

            public TheStrangeDuo(int a, int b)
            {
                Aa = a;
                Bb = b;
            }

            public int CompareTo(TheStrangeDuo other)
            {
                return Bb.CompareTo(other.Bb);
            }
        }
    }

    public sealed class StableMultiArrayHeapTests : StableMultiHeapTests
    {
        protected override IHeap<T> GetHeap<T>()
        {
            return StableHeapFactory.NewArrayHeap<T>(7);
        }

        protected override IHeap<T> GetHeap<T>(IEqualityComparer<T> eqCmp)
        {
            return StableHeapFactory.NewArrayHeap(7, eqCmp);
        }

        protected override IHeap<T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewArrayHeap(7, cmp);
        }
    }

    public sealed class StableMultiBinaryHeapTests : StableMultiHeapTests
    {
        protected override IHeap<T> GetHeap<T>()
        {
            return StableHeapFactory.NewBinaryHeap<T>();
        }

        protected override IHeap<T> GetHeap<T>(IEqualityComparer<T> eqCmp)
        {
            return StableHeapFactory.NewBinaryHeap(eqCmp);
        }

        protected override IHeap<T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewBinaryHeap(cmp);
        }
    }

    public sealed class StableMultiBinomialHeapTests : StableMultiHeapTests
    {
        protected override IHeap<T> GetHeap<T>()
        {
            return StableHeapFactory.NewBinomialHeap<T>();
        }

        protected override IHeap<T> GetHeap<T>(IEqualityComparer<T> eqCmp)
        {
            return StableHeapFactory.NewBinomialHeap(eqCmp);
        }

        protected override IHeap<T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewBinomialHeap(cmp);
        }
    }

    public sealed class StableMultiFibonacciHeapTests : StableMultiHeapTests
    {
        protected override IHeap<T> GetHeap<T>()
        {
            return StableHeapFactory.NewFibonacciHeap<T>();
        }

        protected override IHeap<T> GetHeap<T>(IEqualityComparer<T> eqCmp)
        {
            return StableHeapFactory.NewFibonacciHeap(eqCmp);
        }

        protected override IHeap<T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewFibonacciHeap(cmp);
        }
    }

    public sealed class StableMultiPairingHeapTests : StableMultiHeapTests
    {
        protected override IHeap<T> GetHeap<T>()
        {
            return StableHeapFactory.NewPairingHeap<T>();
        }

        protected override IHeap<T> GetHeap<T>(IEqualityComparer<T> eqCmp)
        {
            return StableHeapFactory.NewPairingHeap(eqCmp);
        }

        protected override IHeap<T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewPairingHeap(cmp);
        }
    }
}