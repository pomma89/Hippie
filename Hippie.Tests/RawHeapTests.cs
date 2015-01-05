// RawHeapTests.cs
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

    public abstract class RawHeapTests : ValPrHeapTests
    {
        private static readonly Dictionary<int, IHeapHandle<int, int>> IntHandles;
        private static readonly Dictionary<int, IHeapHandle<int, int>> RefIntHandles;

        private static readonly Dictionary<int, IHeapHandle<string, string>> StringHandles;
        private static readonly Dictionary<int, IHeapHandle<string, string>> RefStringHandles;

        private static readonly Dictionary<int, IHeapHandle<int, int>> RandIntHandles;
        private static readonly Dictionary<int, IHeapHandle<int, int>> RefRandIntHandles;

        protected IRawHeap<int, int> IntHeap;
        private static readonly IRawHeap<int, int> RefIntHeap;

        private IRawHeap<string, string> _stringHeap;
        private static readonly IRawHeap<string, string> RefStringHeap;

        static RawHeapTests()
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

        protected RawHeapTests()
        {
            RandSet.Clear();
        }

        protected abstract IRawHeap<T, T> GetHeap<T>() where T : IComparable<T>;

        protected abstract IRawHeap<T, T> GetHeap<T>(IComparer<T> cmp);

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
        public void Add_SameIntValues()
        {
            AddSameIntValues(IntHeap);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_SameStringValues()
        {
            AddStringValues(_stringHeap);
            AddStringValues(_stringHeap);
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
        public void Add_ManyStringItems()
        {
            AddStringValues(_stringHeap);
            AssertSameContents(RefStringHeap, _stringHeap);
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

        [Test]
        public void Add_OneStringItem()
        {
            AddStringValues(_stringHeap, 0, 1);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        /**********************************************************************
         * UpdatePriorityOf
         **********************************************************************/

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdatePriorityOf_NullStringValue()
        {
            _stringHeap.UpdatePriorityOf(null, StringValues[0]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdatePriorityOf_NullStringPriority()
        {
            AddStringValues(_stringHeap, 0, 1);
            _stringHeap.UpdatePriorityOf(StringHandles[0], null);
        }

        [Test]
        public void UpdatePriorityOf_ManyIntPriorities()
        {
            AddIntValues(IntHeap);
            IncreaseIntPriorities(IntHeap);
            IncrDecrIntPriorities(IntHeap);
            DecrIncrIntPriorities(IntHeap);
            DecreaseIntPriorities(IntHeap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_ManyStringPriorities()
        {
            AddStringValues(_stringHeap);
            IncreaseStringPriorities(_stringHeap);
            IncrDecrStringPriorities(_stringHeap);
            DecrIncrStringPriorities(_stringHeap);
            DecreaseStringPriorities(_stringHeap);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdatePriorityOf_ChangeManyRandIntPriorities()
        {
            AddRandomIntValues(IntHeap);
            UpdateRandIntPriorities(IntHeap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_ChangeOneRandIntPriority()
        {
            AddRandomIntValues(IntHeap, 0, 1);
            UpdateRandIntPriorities(IntHeap, 0, 1);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_DecreaseManyIntPriorities()
        {
            AddIntValues(IntHeap);
            DecreaseIntPriorities(IntHeap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_DecreaseManyStringPriorities()
        {
            AddStringValues(_stringHeap);
            DecreaseStringPriorities(_stringHeap);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdatePriorityOf_DecreaseOneIntPriority()
        {
            AddIntValues(IntHeap, 0, 1);
            DecreaseIntPriorities(IntHeap, 0, 1);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_DecreaseOneStringPriority()
        {
            AddStringValues(_stringHeap, 0, 1);
            DecreaseStringPriorities(_stringHeap, 0, 1);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdatePriorityOf_DecreaseAndIncreaseManyIntPriorities()
        {
            AddIntValues(IntHeap);
            DecreaseIntPriorities(IntHeap);
            IncreaseIntPriorities(IntHeap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_DecreaseAndIncreaseManyStringPriorities()
        {
            AddStringValues(_stringHeap);
            DecreaseStringPriorities(_stringHeap);
            IncreaseStringPriorities(_stringHeap);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdatePriorityOf_DecreaseAndIncreaseOneIntPriority()
        {
            AddIntValues(IntHeap, 0, 1);
            DecreaseIntPriorities(IntHeap, 0, 1);
            IncreaseIntPriorities(IntHeap, 0, 1);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_DecreaseAndIncreaseOneStringPriority()
        {
            AddStringValues(_stringHeap, 0, 1);
            DecreaseStringPriorities(_stringHeap, 0, 1);
            IncreaseStringPriorities(_stringHeap, 0, 1);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdatePriorityOf_DecrIncrManyIntPriorities()
        {
            AddIntValues(IntHeap);
            DecrIncrIntPriorities(IntHeap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_DecrIncrManyStringPriorities()
        {
            AddStringValues(_stringHeap);
            DecrIncrStringPriorities(_stringHeap);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdatePriorityOf_DecrIncrOneIntPriority()
        {
            AddIntValues(IntHeap, 0, 1);
            DecrIncrIntPriorities(IntHeap, 0, 1);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_DecrIncrOneStringPriority()
        {
            AddStringValues(_stringHeap, 0, 1);
            DecrIncrStringPriorities(_stringHeap, 0, 1);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdatePriorityOf_IncreaseManyIntPriorities()
        {
            AddIntValues(IntHeap);
            IncreaseIntPriorities(IntHeap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_IncreaseManyStringPriorities()
        {
            AddStringValues(_stringHeap);
            IncreaseStringPriorities(_stringHeap);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdatePriorityOf_IncreaseOneIntPriority()
        {
            AddIntValues(IntHeap, 0, 1);
            IncreaseIntPriorities(IntHeap, 0, 1);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_IncreaseOneStringPriority()
        {
            AddStringValues(_stringHeap, 0, 1);
            IncreaseStringPriorities(_stringHeap, 0, 1);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdatePriorityOf_IncreaseAndDecreaseManyIntPriorities()
        {
            AddIntValues(IntHeap);
            IncreaseIntPriorities(IntHeap);
            DecreaseIntPriorities(IntHeap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_IncreaseAndDecreaseManyStringPriorities()
        {
            AddStringValues(_stringHeap);
            IncreaseStringPriorities(_stringHeap);
            DecreaseStringPriorities(_stringHeap);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdatePriorityOf_IncreaseAndDecreaseOneIntPriority()
        {
            AddIntValues(IntHeap, 0, 1);
            IncreaseIntPriorities(IntHeap, 0, 1);
            DecreaseIntPriorities(IntHeap, 0, 1);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_IncreaseAndDecreaseOneStringPriority()
        {
            AddStringValues(_stringHeap, 0, 1);
            IncreaseStringPriorities(_stringHeap, 0, 1);
            DecreaseStringPriorities(_stringHeap, 0, 1);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdatePriorityOf_IncrDecrManyIntPriorities()
        {
            AddIntValues(IntHeap);
            IncrDecrIntPriorities(IntHeap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_IncrDecrManyStringPriorities()
        {
            AddStringValues(_stringHeap);
            IncrDecrStringPriorities(_stringHeap);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdatePriorityOf_IncrDecrOneIntPriority()
        {
            AddIntValues(IntHeap, 0, 1);
            IncrDecrIntPriorities(IntHeap, 0, 1);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdatePriorityOf_IncrDecrOneStringPriority()
        {
            AddStringValues(_stringHeap, 0, 1);
            IncrDecrStringPriorities(_stringHeap, 0, 1);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        /**********************************************************************
         * UpdateValue
         **********************************************************************/

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateValue_NullStringValue()
        {
            _stringHeap.UpdateValue(null, StringValues[0]);
        }

        [Test]
        public void UpdateValue_ManyRandIntValues()
        {
            AddIntValues(IntHeap);
            UpdateRandIntValues(IntHeap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdateValue_ManyStringValues()
        {
            AddStringValues(_stringHeap);
            UpdateStringValues(_stringHeap);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdateValue_OneRandIntValue()
        {
            AddIntValues(IntHeap, 0, 1);
            UpdateRandIntValues(IntHeap, 0, 1);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void UpdateValue_OneStringValue()
        {
            AddStringValues(_stringHeap, 0, 1);
            UpdateStringValues(_stringHeap, 0, 1);
            AssertSameContents(RefStringHeap, _stringHeap);
        }

        [Test]
        public void UpdateValue_SameValues()
        {
            AddIntValues(IntHeap);
            foreach (var p in IntHandles)
                IntHeap.UpdateValue(p.Value, p.Value.Value);
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
            foreach (var p in IntHandles)
                Assert.False(IntHeap.Contains(p.Value));
        }

        [Test]
        public void Clear_TwoTimes()
        {
            AddIntValues(IntHeap);
            IntHeap.Clear();
            IntHeap.Clear();
            Assert.AreEqual(0, IntHeap.Count);
            foreach (var p in IntHandles)
                Assert.False(IntHeap.Contains(p.Value));
        }

        /**********************************************************************
         * Merge
         **********************************************************************/

        [Test]
        public void Merge_CovariantHeaps()
        {
            var aHeap = GetHeap<A>();
            var bHeap = GetHeap<B>();
            for (var i = 0; i < 100; ++i)
            {
                var rand = NextRandInt();
                var aItem = new A(rand);
                aHeap.Add(aItem, aItem);
                var bItem = new B(rand);
                bHeap.Add(bItem, bItem);
            }
            aHeap.Merge(bHeap);
            Assert.AreEqual(200, aHeap.Count);
            Assert.AreEqual(0, bHeap.Count);
        }

        [Test]
        public void Merge_CovariantHeaps_TwoTimes()
        {
            var aHeap = GetHeap<A>();
            var bHeap = GetHeap<B>();
            for (var i = 0; i < 100; ++i)
            {
                var rand = NextRandInt();
                var aItem = new A(rand);
                aHeap.Add(aItem, aItem);
                var bItem = new B(rand);
                bHeap.Add(bItem, bItem);
            }
            aHeap.Merge(bHeap);
            for (var i = 0; i < 100; ++i)
            {
                var rand = NextRandInt();
                var aItem = new A(rand);
                aHeap.Add(aItem, aItem);
                var bItem = new B(rand);
                bHeap.Add(bItem, bItem);
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
            var heap = HeapFactory.NewRawArrayHeap<int, int>(21);
            IntHeap.Merge(heap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void Merge_FullHeap_WithEmptyHeap_OfDifferentType()
        {
            AddIntValues(IntHeap);
            var heap = HeapFactory.NewRawArrayHeap<int, int>(21);
            IntHeap.Merge(heap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void Merge_EmptyHeap_WithFullHeap_OfDifferentType()
        {
            var heap = HeapFactory.NewRawArrayHeap<int, int>(21);
            AddIntValues(heap);
            IntHeap.Merge(heap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        public void Merge_FullHeap_WithFullHeap_OfDifferentType()
        {
            AddIntValues(IntHeap, 0, IntValueCount / 2);
            var heap = HeapFactory.NewRawArrayHeap<int, int>(21);
            AddIntValues(heap, IntValueCount / 2);
            IntHeap.Merge(heap);
            AssertSameContents(RefIntHeap, IntHeap);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Merge_WithFullHeap_WithDifferentComparer()
        {
            AddIntValues(IntHeap, 0, IntValueCount / 2);
            var heap = HeapFactory.NewRawArrayHeap<int, int>(21, new ReversedIntComparer());
            AddIntValues(heap, IntValueCount / 2);
            IntHeap.Merge(heap);
        }

        [Test]
        public void Merge_WithFullHeap_WithSameValues()
        {
            AddIntValues(IntHeap); // Method also adds values to RefIntHeap
            IntHeap.Merge(RefIntHeap);
            Assert.AreEqual(2 * IntValueCount, IntHeap.Count);
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
		 * Dijkstra-based tests
		 **********************************************************************/

        [Test]
        public void Dijkstra_EmptyGraph()
        {
            Dijkstra(EmptyGraph);
        }

        [Test]
        public void Dijkstra_SparseGraph()
        {
            Dijkstra(SparseGraph);
        }

        [Test]
        public void Dijkstra_MediumGraph()
        {
            Dijkstra(MediumGraph);
        }

        private void Dijkstra(RandomGraph graph)
        {
            var refDist = graph.Dijkstra(RefIntHeap, 0);
            var dist = graph.Dijkstra(IntHeap, 0);
            Assert.AreEqual(refDist.Length, dist.Length);
            for (var i = 0; i < refDist.Length; ++i)
                Assert.AreEqual(refDist[i], dist[i]);
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

        [Test]
        public void HeapSort_StringValues()
        {
            HeapSort_Test(RefStringHeap, _stringHeap, StringValues);
        }

        private static void HeapSort_Test<T>(IRawHeap<T, T> refHeap, IRawHeap<T, T> heap, List<T> values)
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

        private static void AddIntValues(IRawHeap<int, int> heap, int from = 0, int to = IntValueCount)
        {
            for (var i = from; i < to; ++i)
            {
                IntHandles.Add(i, heap.Add(IntValues[i], IntValues[i]));
                RefIntHandles.Add(i, RefIntHeap.Add(IntValues[i], IntValues[i]));
            }
        }

        private static void AddRandomIntValues(IRawHeap<int, int> heap, int from = 0, int to = IntValueCount)
        {
            for (var i = from; i < to; ++i)
            {
                RandIntHandles.Add(i, heap.Add(RandomIntValues[i], RandomIntValues[i]));
                RefRandIntHandles.Add(i, RefIntHeap.Add(RandomIntValues[i], RandomIntValues[i]));
            }
        }

        private static void AddSameIntValues(IRawHeap<int, int> heap, int count = IntValueCount)
        {
            for (var i = 0; i < count; ++i)
            {
                heap.Add(0, 0);
                RefIntHeap.Add(0, 0);
            }
        }

        private static void AddStringValues(IRawHeap<string, string> heap, int from = 0, int to = StringValueCount)
        {
            for (var i = from; i < to; ++i)
            {
                StringHandles.Add(i, heap.Add(StringValues[i], StringValues[i]));
                RefStringHandles.Add(i, RefStringHeap.Add(StringValues[i], StringValues[i]));
            }
        }

        private static void UpdateRandIntPriorities(IRawHeap<int, int> heap, int from = 0, int to = IntValueCount)
        {
            for (var i = from; i < to; ++i)
            {
                var r = NextRandInt();
                heap.UpdatePriorityOf(RandIntHandles[i], r);
                RefIntHeap.UpdatePriorityOf(RefRandIntHandles[i], r);
            }

            // Repeat the loop to create more disorder...
            for (var i = from; i < to; ++i)
            {
                var r = NextRandInt();
                heap.UpdatePriorityOf(RandIntHandles[i], r);
                RefIntHeap.UpdatePriorityOf(RefRandIntHandles[i], r);
            }
        }

        private static void DecreaseIntPriorities(IRawHeap<int, int> heap, int from = 0, int to = IntValueCount)
        {
            for (var i = from; i < to; ++i)
            {
                var oldPr = heap.UpdatePriorityOf(IntHandles[i], IntValues[i] - IntValueCount);
                var refOldPr = RefIntHeap.UpdatePriorityOf(RefIntHandles[i], IntValues[i] - IntValueCount);
                Assert.AreEqual(refOldPr, oldPr);
            }
        }

        private static void DecreaseStringPriorities(IRawHeap<string, string> heap, int from = 0, int to = StringValueCount)
        {
            for (var i = from; i < to; ++i)
            {
                heap.UpdatePriorityOf(StringHandles[i], '\0' + StringValues[i]);
                RefStringHeap.UpdatePriorityOf(RefStringHandles[i], '\0' + StringValues[i]);
            }
        }

        private static void DecrIncrIntPriorities(IRawHeap<int, int> heap, int from = 0, int to = IntValueCount)
        {
            for (var i = from; i < to; ++i)
            {
                heap.UpdatePriorityOf(IntHandles[i], IntValues[i] - IntValueCount);
                RefIntHeap.UpdatePriorityOf(RefIntHandles[i], IntValues[i] - IntValueCount);
                heap.UpdatePriorityOf(IntHandles[i], IntValues[i] + IntValueCount);
                RefIntHeap.UpdatePriorityOf(RefIntHandles[i], IntValues[i] + IntValueCount);
            }
        }

        private static void DecrIncrStringPriorities(IRawHeap<string, string> heap, int from = 0, int to = StringValueCount)
        {
            for (var i = from; i < to; ++i)
            {
                heap.UpdatePriorityOf(StringHandles[i], '\0' + StringValues[i]);
                RefStringHeap.UpdatePriorityOf(RefStringHandles[i], '\0' + StringValues[i]);
                heap.UpdatePriorityOf(StringHandles[i], 'z' + StringValues[i]);
                RefStringHeap.UpdatePriorityOf(RefStringHandles[i], 'z' + StringValues[i]);
            }
        }

        private static void IncreaseIntPriorities(IRawHeap<int, int> heap, int from = 0, int to = IntValueCount)
        {
            for (var i = to - 1; i >= from; --i)
            {
                heap.UpdatePriorityOf(IntHandles[i], IntValues[i] + IntValueCount);
                RefIntHeap.UpdatePriorityOf(RefIntHandles[i], IntValues[i] + IntValueCount);
            }
        }

        private static void IncreaseStringPriorities(IRawHeap<string, string> heap, int from = 0, int to = StringValueCount)
        {
            for (var i = to - 1; i >= from; --i)
            {
                heap.UpdatePriorityOf(StringHandles[i], 'z' + StringValues[i]);
                RefStringHeap.UpdatePriorityOf(RefStringHandles[i], 'z' + StringValues[i]);
            }
        }

        private static void IncrDecrIntPriorities(IRawHeap<int, int> heap, int from = 0, int to = IntValueCount)
        {
            for (var i = to - 1; i >= from; --i)
            {
                heap.UpdatePriorityOf(IntHandles[i], IntValues[i] + IntValueCount);
                RefIntHeap.UpdatePriorityOf(RefIntHandles[i], IntValues[i] + IntValueCount);
                heap.UpdatePriorityOf(IntHandles[i], IntValues[i] - IntValueCount);
                RefIntHeap.UpdatePriorityOf(RefIntHandles[i], IntValues[i] - IntValueCount);
            }
        }

        private static void IncrDecrStringPriorities(IRawHeap<string, string> heap, int from = 0, int to = StringValueCount)
        {
            for (var i = to - 1; i >= from; --i)
            {
                heap.UpdatePriorityOf(StringHandles[i], 'z' + StringValues[i]);
                RefStringHeap.UpdatePriorityOf(RefStringHandles[i], 'z' + StringValues[i]);
                heap.UpdatePriorityOf(StringHandles[i], '\0' + StringValues[i]);
                RefStringHeap.UpdatePriorityOf(RefStringHandles[i], '\0' + StringValues[i]);
            }
        }

        private static void UpdateRandIntValues(IRawHeap<int, int> heap, int from = 0, int to = IntValueCount)
        {
            for (var i = from; i < to; ++i)
            {
                heap.UpdateValue(IntHandles[i], RandomIntValues[i]);
                RefIntHeap.UpdateValue(RefIntHandles[i], RandomIntValues[i]);
            }
        }

        private static void UpdateStringValues(IRawHeap<string, string> heap, int from = 0, int to = StringValueCount)
        {
            for (var i = from; i < to; ++i)
            {
                var newValue = "GINA & PINO " + i;
                heap.UpdateValue(StringHandles[i], newValue);
                RefStringHeap.UpdateValue(RefStringHandles[i], newValue);
            }
        }

        private static void AssertSameContents<T>(IRawHeap<T, T> refHeap, IRawHeap<T, T> heap)
            where T : IComparable<T>
        {
            Assert.AreEqual(refHeap.Count, heap.Count);

            while (refHeap.Count != 0)
            {
                AssertSameHandle(refHeap.Min, heap.Min);
                if (refHeap.Count % 2 == 0)
                {
                    AssertSameHandle(refHeap.RemoveMin(), heap.RemoveMin());
                }
                else
                {
                    Assert.True(refHeap.Remove(refHeap.Min));
                    Assert.True(heap.Remove(heap.Min));
                }
                Assert.AreEqual(refHeap.Count, heap.Count);
            }

            Assert.AreEqual(0, heap.Count);
        }

        private static void AssertSameHandle<TVal, TPr>(IHeapHandle<TVal, TPr> p1, IHeapHandle<TVal, TPr> p2)
        {
            Assert.AreEqual(p1.Value, p2.Value);
            Assert.AreEqual(p1.Priority, p2.Priority);
        }
    }

    public sealed class RawArrayHeapTests : RawHeapTests
    {
        protected override IRawHeap<T, T> GetHeap<T>()
        {
            return HeapFactory.NewRawArrayHeap<T, T>(7);
        }

        protected override IRawHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewRawArrayHeap<T, T>(7, cmp);
        }
    }

    public sealed class RawBinaryHeapTests : RawHeapTests
    {
        protected override IRawHeap<T, T> GetHeap<T>()
        {
            return HeapFactory.NewRawBinaryHeap<T, T>();
        }

        protected override IRawHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewRawBinaryHeap<T, T>(cmp);
        }
    }

    public sealed class RawBinomialHeapTests : RawHeapTests
    {
        protected override IRawHeap<T, T> GetHeap<T>()
        {
            return HeapFactory.NewRawBinomialHeap<T, T>();
        }

        protected override IRawHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewRawBinomialHeap<T, T>(cmp);
        }
    }

    public sealed class RawFibonacciHeapTests : RawHeapTests
    {
        protected override IRawHeap<T, T> GetHeap<T>()
        {
            return HeapFactory.NewRawFibonacciHeap<T, T>();
        }

        protected override IRawHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewRawFibonacciHeap<T, T>(cmp);
        }
    }

    public sealed class RawPairingHeapTests : RawHeapTests
    {
        protected override IRawHeap<T, T> GetHeap<T>()
        {
            return HeapFactory.NewRawPairingHeap<T, T>();
        }

        protected override IRawHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return HeapFactory.NewRawPairingHeap<T, T>(cmp);
        }
    }

    public abstract class StableRawHeapTests : RawHeapTests
    {
        private IStableRawHeap<int, int> StableIntHeap
        {
            get { return (IStableRawHeap<int, int>) IntHeap; }
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

        [TestCase(1)]
        [TestCase(IntValueCount / 2)]
        [TestCase(IntValueCount)]
        public void UpdatedPriorityOf_SamePriority_SameOrder(int valueCount)
        {
            var handles = new IHeapHandle<int, int>[valueCount];
            for (var i = 0; i < valueCount; ++i)
            {
                handles[i] = IntHeap.Add(i, 0);
            }
            for (var i = 0; i < valueCount; ++i)
            {
                IntHeap.UpdatePriorityOf(handles[i], 0);
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
        public void UpdatedPriorityOf_SamePriority_SameOrder_StableHeap(int valueCount)
        {
            var handles = new IHeapHandle<int, IVersionedPriority<int>>[valueCount];
            for (var i = 0; i < valueCount; ++i)
            {
                handles[i] = StableIntHeap.Add(i, 0);
            }
            for (var i = 0; i < valueCount; ++i)
            {
                StableIntHeap.UpdatePriorityOf(handles[i], 0);
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
        public void UpdatedPriorityOf_SamePriority_SameOrder_CustomVersion(int valueCount)
        {
            var version = 0L;
            var handles = new IHeapHandle<int, IVersionedPriority<int>>[valueCount];
            for (var i = 0; i < valueCount; ++i)
            {
                handles[i] = StableIntHeap.Add(i, 0, version++);
            }
            for (var i = 0; i < valueCount; ++i)
            {
                StableIntHeap.UpdatePriorityOf(handles[i], 0, version++);
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
        public void UpdatedPriorityOf_SamePriority_ReverseOrder(int valueCount)
        {
            var handles = new IHeapHandle<int, int>[valueCount];
            for (var i = 0; i < valueCount; ++i)
            {
                handles[i] = IntHeap.Add(i, 0);
            }
            for (var i = valueCount - 1; i >= 0; --i)
            {
                IntHeap.UpdatePriorityOf(handles[i], 0);
            }
            for (var i = valueCount - 1; i >= 0; --i)
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
        public void UpdatedPriorityOf_SamePriority_ReverseOrder_StableHeap(int valueCount)
        {
            var handles = new IHeapHandle<int, IVersionedPriority<int>>[valueCount];
            for (var i = 0; i < valueCount; ++i)
            {
                handles[i] = StableIntHeap.Add(i, 0);
            }
            for (var i = valueCount - 1; i >= 0; --i)
            {
                StableIntHeap.UpdatePriorityOf(handles[i], 0);
            }
            for (var i = valueCount - 1; i >= 0; --i)
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
        public void UpdatedPriorityOf_SamePriority_ReverseOrder_CustomVersion(int valueCount)
        {
            var version = 0L;
            var handles = new IHeapHandle<int, IVersionedPriority<int>>[valueCount];
            for (var i = 0; i < valueCount; ++i)
            {
                handles[i] = StableIntHeap.Add(i, 0, version++);
            }
            for (var i = valueCount - 1; i >= 0; --i)
            {
                StableIntHeap.UpdatePriorityOf(handles[i], 0, version++);
            }
            for (var i = valueCount - 1; i >= 0; --i)
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
        public void Indexer_SamePriority_SameOrder(int valueCount)
        {
            var handles = new IHeapHandle<int, int>[valueCount];
            for (var i = 0; i < valueCount; ++i)
            {
                handles[i] = IntHeap.Add(i, 0);
            }
            for (var i = 0; i < valueCount; ++i)
            {
                IntHeap[handles[i]] = 0;
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
        public void Indexer_SamePriority_SameOrder_StableHeap(int valueCount)
        {
            var handles = new IHeapHandle<int, IVersionedPriority<int>>[valueCount];
            for (var i = 0; i < valueCount; ++i)
            {
                handles[i] = StableIntHeap.Add(i, 0);
            }
            for (var i = 0; i < valueCount; ++i)
            {
                StableIntHeap[handles[i]] = 0;
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
        public void Indexer_SamePriority_ReverseOrder(int valueCount)
        {
            var handles = new IHeapHandle<int, int>[valueCount];
            for (var i = 0; i < valueCount; ++i)
            {
                handles[i] = IntHeap.Add(i, 0);
            }
            for (var i = valueCount - 1; i >= 0; --i)
            {
                IntHeap[handles[i]] = 0;
            }
            for (var i = valueCount - 1; i >= 0; --i)
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
        public void Indexer_SamePriority_ReverseOrder_StableHeap(int valueCount)
        {
            var handles = new IHeapHandle<int, IVersionedPriority<int>>[valueCount];
            for (var i = 0; i < valueCount; ++i)
            {
                handles[i] = StableIntHeap.Add(i, 0);
            }
            for (var i = valueCount - 1; i >= 0; --i)
            {
                StableIntHeap[handles[i]] = 0;
            }
            for (var i = valueCount - 1; i >= 0; --i)
            {
                Assert.AreEqual(i, StableIntHeap.Min.Value);
                Assert.AreEqual(0, StableIntHeap.Min.Priority.Value);
                var min = StableIntHeap.RemoveMin();
                Assert.AreEqual(i, min.Value);
                Assert.AreEqual(0, min.Priority.Value);
            }
        }
    }

    public sealed class StableRawArrayHeapTests : StableRawHeapTests
    {
        protected override IRawHeap<T, T> GetHeap<T>()
        {
            return StableHeapFactory.NewRawArrayHeap<T, T>(7);
        }

        protected override IRawHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewRawArrayHeap<T, T>(7, cmp);
        }
    }

    public sealed class StableRawBinaryHeapTests : StableRawHeapTests
    {
        protected override IRawHeap<T, T> GetHeap<T>()
        {
            return StableHeapFactory.NewRawBinaryHeap<T, T>();
        }

        protected override IRawHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewRawBinaryHeap<T, T>(cmp);
        }
    }

    public sealed class StableRawBinomialHeapTests : StableRawHeapTests
    {
        protected override IRawHeap<T, T> GetHeap<T>()
        {
            return StableHeapFactory.NewRawBinomialHeap<T, T>();
        }

        protected override IRawHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewRawBinomialHeap<T, T>(cmp);
        }
    }

    public sealed class StableRawFibonacciHeapTests : StableRawHeapTests
    {
        protected override IRawHeap<T, T> GetHeap<T>()
        {
            return StableHeapFactory.NewRawFibonacciHeap<T, T>();
        }

        protected override IRawHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewRawFibonacciHeap<T, T>(cmp);
        }
    }

    public sealed class StableRawPairingHeapTests : StableRawHeapTests
    {
        protected override IRawHeap<T, T> GetHeap<T>()
        {
            return StableHeapFactory.NewRawPairingHeap<T, T>();
        }

        protected override IRawHeap<T, T> GetHeap<T>(IComparer<T> cmp)
        {
            return StableHeapFactory.NewRawPairingHeap<T, T>(cmp);
        }
    }
}