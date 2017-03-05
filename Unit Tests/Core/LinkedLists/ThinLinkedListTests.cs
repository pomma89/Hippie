// ThinLinkedListTests.cs
// 
// Author: Alessio Parma <alessio.parma@gmail.com>
// 
// Copyright (c) 2013-2014 Alessio Parma <alessio.parma@gmail.com>
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

using System;
using System.Collections.Generic;
using DIBRIS.Hippie.Core.LinkedLists;
using NUnit.Framework;

namespace DIBRIS.Hippie.UnitTests.Core.LinkedLists
{
    abstract class ThinLinkedListTests<TList> where TList : class, IThinLinkedList<int>
    {
        protected const int SmallCount = 10;
        protected const int MediumCount = 100;
        protected const int BigCount = 1000;

        protected TList List;

        [SetUp]
        public void SetUp()
        {
            List = GetList();
        }

        [TearDown]
        public void TearDown()
        {
            List = null;
        }

        protected virtual TList GetList()
        {
            var lst = (TList) (new ThinLinkedList<int>() as IThinLinkedList<int>);
            IThinLinkedList<int> ret = new MockedThinLinkedList<TList, int>(lst);
            return (TList) ret;
        }

        protected virtual TList GetList(IEqualityComparer<int> equalityComparer)
        {
            var lst = (TList) (new ThinLinkedList<int>(equalityComparer) as IThinLinkedList<int>);
            IThinLinkedList<int> ret = new MockedThinLinkedList<TList, int>(lst);
            return (TList) ret;
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Add_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.Add(i);
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void AddFirst_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddFirst(i);
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Add_SameItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.Add(0);
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void AddFirst_SameItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddFirst(0);
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Contains_DifferentItems_SameOrder(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddFirst(i);
            }
            for (var i = itemCount - 1; i >= 0; --i)
            {
                Assert.True(List.Contains(i));
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Contains_DifferentItems_ReverseOrder(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddFirst(i);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                Assert.True(List.Contains(i));
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Contains_DifferentItems_NotContained(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddFirst(i);
            }
            for (var i = itemCount; i < itemCount * 2; ++i)
            {
                Assert.False(List.Contains(i));
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Contains_DifferentItems_RandomOrder(int itemCount)
        {
            var heap = new SortedList<int, int>();
            var rand = new Random();
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddFirst(i);
                heap.Add(rand.Next(), i);
            }
            foreach (var i in heap)
            {
                Assert.True(List.Contains(i.Value));
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Factory_NullEqualityComparer()
        {
            GetList(null);
        }

        [Test]
        public void Factory_CustomEqualityComparer()
        {
            var eqCmp = new DummyEqualityComparer();
            Assert.AreSame(GetList(eqCmp).EqualityComparer, eqCmp);
        }

        [Test]
        public void Factory_DefaultEqualityComparer()
        {
            Assert.AreSame(GetList().EqualityComparer, EqualityComparer<int>.Default);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void First_NoItems()
        {
            // ReSharper disable UnusedVariable
            var f = List.First;
            // ReSharper restore UnusedVariable
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void First_NoItems_AfterAddRemove()
        {
            List.Add(5);
            List.RemoveFirst();
            // ReSharper disable UnusedVariable
            var f = List.First;
            // ReSharper restore UnusedVariable
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveFirst_NoItems()
        {
            List.RemoveFirst();
        }

        [Test]
        public void Remove_NotContainedItem()
        {
            Assert.False(List.Remove(0));
        }

        [Test]
        public void Remove_ContainedItem()
        {
            List.Add(0);
            Assert.True(List.Remove(0));
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Remove_DifferentItems_SameOrder(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddFirst(i);
            }
            for (var i = itemCount - 1; i > 0; --i)
            {
                Assert.True(List.Remove(i));
                Assert.AreEqual(i - 1, List.First);
            }
            Assert.True(List.Remove(0));
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Remove_DifferentItems_RandomOrder(int itemCount)
        {
            var heap = new SortedList<int, int>();
            var rand = new Random();
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddFirst(i);
                heap.Add(rand.Next(), i);
            }
            foreach (var i in heap)
            {
                Assert.True(List.Remove(i.Value));
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Remove_SameItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddFirst(0);
            }
            for (var i = itemCount - 1; i > 0; --i)
            {
                Assert.True(List.Remove(0));
                Assert.AreEqual(0, List.First);
            }
            Assert.True(List.Remove(0));
        }

        sealed class DummyEqualityComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return true;
            }

            public int GetHashCode(int obj)
            {
                return 0;
            }
        }
    }

    [TestFixture]
sealed class ThinLinkedListTests : ThinLinkedListTests<IThinLinkedList<int>>
    {
    }
}
