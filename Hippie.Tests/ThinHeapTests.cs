// ThinHeapTests.cs
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

namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DIBRIS.Hippie;
    using NUnit.Framework;

    public abstract class ThinHeapTests : ValPrHeapTests
    {
        private static readonly Dictionary<int, IHeapHandle<int, int>> IntHandles;
        private static readonly Dictionary<int, IHeapHandle<int, int>> RefIntHandles;

        private static readonly Dictionary<int, IHeapHandle<string, string>> StringHandles;
        private static readonly Dictionary<int, IHeapHandle<string, string>> RefStringHandles;

        private static readonly Dictionary<int, IHeapHandle<int, int>> RandIntHandles;
        private static readonly Dictionary<int, IHeapHandle<int, int>> RefRandIntHandles;

        protected IThinHeap<int, int> IntHeap;
        private static readonly IRawHeap<int, int> RefIntHeap;

        private IThinHeap<string, string> _stringHeap;
        private static readonly IRawHeap<string, string> RefStringHeap;

        static ThinHeapTests()
        {
            IntHandles = new Dictionary<int, IHeapHandle<int, int>>();
            RefIntHandles = new Dictionary<int, IHeapHandle<int, int>>();
            StringHandles = new Dictionary<int, IHeapHandle<string, string>>();
            RefStringHandles = new Dictionary<int, IHeapHandle<string, string>>();
            RandIntHandles = new Dictionary<int, IHeapHandle<int, int>>();
            RefRandIntHandles = new Dictionary<int, IHeapHandle<int, int>>();
            RefIntHeap = HeapFactory.NewRawBinaryHeap<int, int>();
            RefStringHeap = HeapFactory.NewRawBinaryHeap<string, string>();
        }

        protected ThinHeapTests()
        {
            RandSet.Clear();
        }

        protected abstract IThinHeap<T, T> GetHeap<T>() where T : IComparable<T>;

        protected abstract IThinHeap<T, T> GetHeap<T>(IComparer<T> cmp) where T : IComparable<T>;

        [SetUp]
        public void SetUp()
        {
            IntHeap = GetHeap<int>();
            _stringHeap = GetHeap<string>();
        }

        [TearDown]
        public void TearDown()
        {
            IntHandles.Clear();
            RefIntHandles.Clear();
            StringHandles.Clear();
            RefStringHandles.Clear();
            RandIntHandles.Clear();
            RefRandIntHandles.Clear();
            RefIntHeap.Clear();
            RefStringHeap.Clear();
            IntHeap = null;
            _stringHeap = null;
        }

        /**********************************************************************
         * Min
         **********************************************************************/

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Min_EmptyHeap()
        {
            // ReSharper disable UnusedVariable
            var min = IntHeap.Min;
            // ReSharper restore UnusedVariable
        }

        /**********************************************************************
         * Add
         **********************************************************************/

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_NullStringPriority()
        {
            _stringHeap.Add(StringValues[0], null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_NullArguments()
        {
            _stringHeap.Add(null, null);
        }

        [Test]
        public void Add_ManyIntItems()
        {
            AddIntValues(IntHeap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void Add_ManyRandIntItems()
        {
            AddRandomIntValues(IntHeap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void Add_OneIntItem()
        {
            AddIntValues(IntHeap, 0, 1);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void Add_OneRandIntItem()
        {
            AddRandomIntValues(IntHeap, 0, 1);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        /**********************************************************************
         * Clear
         **********************************************************************/

        [Test]
        public void Clear_EmptyHeap()
        {
            IntHeap.Clear();
            Assert.AreEqual(IntHeap.Count, 0);
        }

        [Test]
        public void Clear_FullHeap()
        {
            AddIntValues(IntHeap);
            IntHeap.Clear();
            Assert.AreEqual(0, IntHeap.Count);
        }

        [Test]
        public void Clear_TwoTimes()
        {
            AddIntValues(IntHeap);
            IntHeap.Clear();
            IntHeap.Clear();
            Assert.AreEqual(0, IntHeap.Count);
        }

        /**********************************************************************
         * Merge
         **********************************************************************/

        [Test]
        public void Merge_CovariantHeaps()
        {
            var aHeap = HeapFactory.NewHeap<A, int>();
            var bHeap = HeapFactory.NewHeap<B, int>();
            for (var i = 0; i < 100; ++i)
            {
                var rand = NextRandInt();
                var aItem = new A(rand);
                aHeap.Add(aItem, i);
                var bItem = new B(rand);
                bHeap.Add(bItem, i);
            }
            aHeap.Merge(bHeap);
            Assert.AreEqual(200, aHeap.Count);
            Assert.AreEqual(0, bHeap.Count);
        }

        [Test]
        public void Merge_CovariantHeaps_TwoTimes()
        {
            var aHeap = HeapFactory.NewHeap<A, int>();
            var bHeap = HeapFactory.NewHeap<B, int>();
            for (var i = 0; i < 100; ++i)
            {
                var rand = NextRandInt();
                var aItem = new A(rand);
                aHeap.Add(aItem, i);
                var bItem = new B(rand);
                bHeap.Add(bItem, i);
            }
            aHeap.Merge(bHeap);
            for (var i = 0; i < 100; ++i)
            {
                var rand = NextRandInt();
                var aItem = new A(rand);
                aHeap.Add(aItem, i);
                var bItem = new B(rand);
                bHeap.Add(bItem, i);
            }
            aHeap.Merge(bHeap);
            Assert.AreEqual(400, aHeap.Count);
            Assert.AreEqual(0, bHeap.Count);
        }

        [Test]
        public void Merge_WithSameHeap()
        {
            AddIntValues(IntHeap);
            IntHeap.Merge(IntHeap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Merge_EmptyHeap_WithNullHeap()
        {
            IntHeap.Merge<int, int>(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Merge_FullHeap_WithNullHeap()
        {
            AddIntValues(IntHeap);
            IntHeap.Merge<int, int>(null);
        }

        [Test]
        public void Merge_EmptyHeap_WithEmptyHeap_OfSameType()
        {
            var heap = GetHeap<int>();
            IntHeap.Merge(heap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void Merge_FullHeap_WithEmptyHeap_OfSameType()
        {
            AddIntValues(IntHeap);
            var heap = GetHeap<int>();
            IntHeap.Merge(heap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void Merge_EmptyHeap_WithFullHeap_OfSameType()
        {
            var heap = GetHeap<int>();
            AddIntValues(heap);
            IntHeap.Merge(heap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void Merge_FullHeap_WithFullHeap_OfSameType()
        {
            AddIntValues(IntHeap, 0, IntValueCount / 2);
            var heap = GetHeap<int>();
            AddIntValues(heap, IntValueCount / 2);
            IntHeap.Merge(heap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void Merge_EmptyHeap_WithEmptyHeap_OfDifferentType()
        {
            var heap = HeapFactory.NewHeap<int, int>();
            IntHeap.Merge(heap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void Merge_FullHeap_WithEmptyHeap_OfDifferentType()
        {
            AddIntValues(IntHeap);
            var heap = HeapFactory.NewHeap<int, int>();
            IntHeap.Merge(heap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void Merge_EmptyHeap_WithFullHeap_OfDifferentType()
        {
            var heap = HeapFactory.NewHeap<int, int>();
            AddIntValues(heap);
            IntHeap.Merge(heap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void Merge_FullHeap_WithFullHeap_OfDifferentType()
        {
            AddIntValues(IntHeap, 0, IntValueCount / 2);
            var heap = HeapFactory.NewHeap<int, int>();
            AddIntValues(heap, IntValueCount / 2);
            IntHeap.Merge(heap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Merge_WithFullHeap_WithDifferentComparer()
        {
            AddIntValues(IntHeap, 0, IntValueCount / 2);
            var heap = HeapFactory.NewHeap<int, int>(new ReversedIntComparer());
            AddIntValues(heap, IntValueCount / 2);
            IntHeap.Merge(heap);
        }

        /**********************************************************************
         * RemoveMin
         **********************************************************************/

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveMin_EmptyHeap()
        {
            IntHeap.RemoveMin();
        }

        /**********************************************************************
         * HeapSort-based tests
         **********************************************************************/

        [Test]
        public void HeapSort_IntValues()
        {
            HeapSort_Test(RefIntHeap, IntHeap, IntValues);
        }

        [Test]
        public void HeapSort_RandomIntValues()
        {
            HeapSort_Test(RefIntHeap, IntHeap, RandomIntValues);
        }

        private static void HeapSort_Test<T>(IRawHeap<T, T> refHeap, IThinHeap<T, T> heap, List<T> values) where T : struct
        {
            var refSortedValues = HeapSort.Sort(refHeap, values);
            var sortedValues = HeapSort.Sort(heap, values);
            Assert.AreEqual(refSortedValues.Length, sortedValues.Length);
            for (var i = 0; i < refSortedValues.Length; ++i)
                Assert.AreEqual(refSortedValues[i], sortedValues[i]);
        }

        /**********************************************************************
		 * ToReadOnlyForest
		 **********************************************************************/

        [Test]
        public void ToReadOnlyForest_ResultNotNull_EmptyHeap()
        {
            Assert.NotNull(IntHeap.ToReadOnlyForest());
        }

        [Test]
        public void ToReadOnlyForest_ResultNotNull_FullHeap()
        {
            AddRandomIntValues(IntHeap);
            Assert.NotNull(IntHeap.ToReadOnlyForest());
        }

        [Test]
        public void ToReadOnlyForest_OneElement()
        {
            IntHeap.Add(IntValues[0], 0);
            var f = IntHeap.ToReadOnlyForest().ToList();
            Assert.AreEqual(1, f.Count);
            var t = f[0];
            Assert.NotNull(t);
            Assert.Null(t.Parent);
            Assert.AreEqual(IntValues[0], t.Value);
            Assert.AreEqual(0, t.Priority);
            Assert.NotNull(t.Children);
            Assert.AreEqual(0, t.Children.Count());
        }

        [Test]
        [Repeat(3)]
        public void ToReadOnlyForest_Generic()
        {
            AddRandomIntValues(IntHeap);
            var values = new HashSet<int>(IntHeap.Select(p => p.Value));
            var priorities = new HashSet<int>(IntHeap.Select(p => p.Priority));
            var forest = IntHeap.ToReadOnlyForest().ToList();
            // Breadth-first, no action
            var trees = forest.SelectMany(t => t.BreadthFirstVisit()).ToList();
            Assert.AreEqual(IntValueCount, trees.Count);
            foreach (var t in trees)
            {
                Assert.True(values.Contains(t.Value));
                Assert.True(priorities.Contains(t.Priority));
            }
            // Depth-first, no action
            trees = forest.SelectMany(t => t.DepthFirstVisit()).ToList();
            Assert.AreEqual(IntValueCount, trees.Count);
            foreach (var t in trees)
            {
                Assert.True(values.Contains(t.Value));
                Assert.True(priorities.Contains(t.Priority));
            }
            // Breadth-first, with selector
            var res = forest.SelectMany(t => t.BreadthFirstVisit((n, a) => n.Priority, 0)).ToList();
            Assert.AreEqual(IntValueCount, res.Count);
            foreach (var r in res)
            {
                Assert.True(priorities.Contains(r));
            }
            // Depth-first, with selector
            res = forest.SelectMany(t => t.DepthFirstVisit((n, a) => n.Priority, 0)).ToList();
            Assert.AreEqual(IntValueCount, res.Count);
            foreach (var r in res)
            {
                Assert.True(priorities.Contains(r));
            }
            // Breadth-first, with action
            var set = new HashSet<int>();
            // ReSharper disable AccessToModifiedClosure
            forest.ForEach(t => t.BreadthFirstVisit(n => set.Add(n.Priority)));
            // ReSharper restore AccessToModifiedClosure
            Assert.AreEqual(priorities.Count, set.Count);
            foreach (var s in set)
            {
                Assert.True(priorities.Contains(s));
            }
            // Depth-first, with action
            set = new HashSet<int>();
            forest.ForEach(t => t.DepthFirstVisit(n => set.Add(n.Priority)));
            Assert.AreEqual(priorities.Count, set.Count);
            foreach (var s in set)
            {
                Assert.True(priorities.Contains(s));
            }
        }

        /**********************************************************************
		 * Reversed int comparer
		 **********************************************************************/

        [Test]
        public void Add_ReversedIntComparer()
        {
            IntHeap = GetHeap(new ReversedIntComparer());
            IntHeap.Add(1, 10);
            IntHeap.Add(2, 20);
            IntHeap.Add(3, 30);
            Assert.AreEqual(30, IntHeap.RemoveMin().Priority);
            Assert.AreEqual(20, IntHeap.RemoveMin().Priority);
            Assert.AreEqual(10, IntHeap.RemoveMin().Priority);
        }

        /**********************************************************************
         * Private methods
         **********************************************************************/

        private static void AddIntValues(IThinHeap<int, int> heap, int from = 0, int to = IntValueCount)
        {
            for (var i = from; i < to; ++i)
            {
                heap.Add(IntValues[i], IntValues[i]);
                RefIntHandles.Add(i, RefIntHeap.Add(IntValues[i], IntValues[i]));
            }
        }

        private static void AddRandomIntValues(IThinHeap<int, int> heap, int from = 0, int to = IntValueCount)
        {
            for (var i = from; i < to; ++i)
            {
                heap.Add(RandomIntValues[i], RandomIntValues[i]);
                RefRandIntHandles.Add(i, RefIntHeap.Add(RandomIntValues[i], RandomIntValues[i]));
            }
        }

        private static void AssertSameContents<T>(IRawHeap<T, T> refHeap, IThinHeap<T, T> heap) where T : struct, IComparable<T>
        {
            Assert.AreEqual(refHeap.Count, heap.Count);

            while (refHeap.Count != 0)
            {
                AssertSameHandle(refHeap.Min, heap.Min);
                AssertSameHandle(refHeap.RemoveMin(), heap.RemoveMin());
                Assert.AreEqual(refHeap.Count, heap.Count);
            }

            Assert.AreEqual(0, heap.Count);
        }

        private static void AssertSameHandle<TVal, TPr>(IHeapHandle<TVal, TPr> p1, IHeapHandle<TVal, TPr> p2) where TPr : struct
        {
            Assert.AreEqual(p1.Value, p2.Value);
            Assert.AreEqual(p1.Priority, p2.Priority);
        }
    }

    public sealed class RealThinHeapTests : ThinHeapTests
    {
        protected override IThinHeap<T, T> GetHeap<T>()
        {
            return HeapFactory.NewHeap<T, T>();
        }

        protected override IThinHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewHeap<T, T>(cmp);
        }
    }

    public sealed class ArrayAsThinHeapTests : ThinHeapTests
    {
        protected override IThinHeap<T, T> GetHeap<T>()
        {
            return HeapFactory.NewArrayHeap<T, T>(7);
        }

        protected override IThinHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewArrayHeap<T, T>(7, cmp);
        }
    }

    public sealed class BinaryAsThinHeapTests : ThinHeapTests
    {
        protected override IThinHeap<T, T> GetHeap<T>()
        {
            return HeapFactory.NewBinaryHeap<T, T>();
        }

        protected override IThinHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewBinaryHeap<T, T>(cmp);
        }
    }

    public sealed class RawArrayAsThinHeapTests : ThinHeapTests
    {
        protected override IThinHeap<T, T> GetHeap<T>()
        {
            return HeapFactory.NewRawArrayHeap<T, T>(7);
        }

        protected override IThinHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewRawArrayHeap<T, T>(7, cmp);
        }
    }

    public sealed class RawBinaryAsThinHeapTests : ThinHeapTests
    {
        protected override IThinHeap<T, T> GetHeap<T>()
        {
            return HeapFactory.NewRawBinaryHeap<T, T>();
        }

        protected override IThinHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewRawBinaryHeap<T, T>(cmp);
        }
    }

    public abstract class StableThinHeapTests : ThinHeapTests
    {
        private IStableThinHeap<int, int> StableIntHeap
        {
            get { return (IStableThinHeap<int, int>) IntHeap; }
        }

        [TestCase(1)]
        [TestCase(IntValueCount / 2)]
        [TestCase(IntValueCount)]
        public void Add_SamePriority(int valueCount)
        {
            for (var i = 0; i < valueCount; ++i)
            {
                IntHeap.Add(i, 0);
            }
            for (var i = 0; i < valueCount; ++i)
            {
                Assert.AreEqual(i, IntHeap.Min.Value);
                Assert.AreEqual(0, IntHeap.Min.Priority);
                var min = IntHeap.RemoveMin();
                Assert.AreEqual(i, min.Value);
                Assert.AreEqual(0, min.Priority);
            }
        }

        [TestCase(1)]
        [TestCase(IntValueCount / 2)]
        [TestCase(IntValueCount)]
        public void Add_SamePriority_StableHeap(int valueCount)
        {
            for (var i = 0; i < valueCount; ++i)
            {
                StableIntHeap.Add(i, 0);
            }
            for (var i = 0; i < valueCount; ++i)
            {
                Assert.AreEqual(i, StableIntHeap.Min.Value);
                Assert.AreEqual(0, StableIntHeap.Min.Priority.Value);
                var min = StableIntHeap.RemoveMin();
                Assert.AreEqual(i, min.Value);
                Assert.AreEqual(0, min.Priority.Value);
            }
        }

        [TestCase(1)]
        [TestCase(IntValueCount / 2)]
        [TestCase(IntValueCount)]
        public void Add_SamePriority_CustomVersion(int valueCount)
        {
            var version = 0L;
            for (var i = 0; i < valueCount; ++i)
            {
                StableIntHeap.Add(i, 0, version++);
            }
            for (var i = 0; i < valueCount; ++i)
            {
                Assert.AreEqual(i, StableIntHeap.Min.Value);
                Assert.AreEqual(0, StableIntHeap.Min.Priority.Value);
                var min = StableIntHeap.RemoveMin();
                Assert.AreEqual(i, min.Value);
                Assert.AreEqual(0, min.Priority.Value);
            }
        }
    }

    public sealed class RealStableThinHeapTests : StableThinHeapTests
    {
        protected override IThinHeap<T, T> GetHeap<T>()
        {
            return StableHeapFactory.NewHeap<T, T>();
        }

        protected override IThinHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewHeap<T, T>(cmp);
        }
    }

    public sealed class ArrayAsStableThinHeapTests : ThinHeapTests
    {
        protected override IThinHeap<T, T> GetHeap<T>()
        {
            return StableHeapFactory.NewArrayHeap<T, T>(7);
        }

        protected override IThinHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewArrayHeap<T, T>(7, cmp);
        }
    }

    public sealed class BinaryAsStableThinHeapTests : ThinHeapTests
    {
        protected override IThinHeap<T, T> GetHeap<T>()
        {
            return StableHeapFactory.NewBinaryHeap<T, T>();
        }

        protected override IThinHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewBinaryHeap<T, T>(cmp);
        }
    }

    public sealed class RawArrayAsStableThinHeapTests : ThinHeapTests
    {
        protected override IThinHeap<T, T> GetHeap<T>()
        {
            return StableHeapFactory.NewRawArrayHeap<T, T>(7);
        }

        protected override IThinHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewRawArrayHeap<T, T>(7, cmp);
        }
    }

    public sealed class RawBinaryAsStableThinHeapTests : ThinHeapTests
    {
        protected override IThinHeap<T, T> GetHeap<T>()
        {
            return StableHeapFactory.NewRawBinaryHeap<T, T>();
        }

        protected override IThinHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewRawBinaryHeap<T, T>(cmp);
        }
    }
}