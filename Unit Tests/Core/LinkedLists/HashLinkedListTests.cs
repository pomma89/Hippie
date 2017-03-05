// HashLinkedListTests.cs
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
using PommaLabs.CodeServices.Common.Collections;
using NUnit.Framework;

namespace PommaLabs.CodeServices.UnitTests.Common.Collections
{
    [TestFixture]
sealed class HashLinkedListTests
    {
        [SetUp]
        public void SetUp()
        {
            _list = new HashLinkedList<int>();
        }

        [TearDown]
        public void TearDown()
        {
            _list = null;
        }

        const int SmallCount = 10;
        const int MediumCount = 100;
        const int BigCount = 1000;

        IHashLinkedList<int> _list;

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Add_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _list.Add(i);
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void AddFirst_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _list.AddFirst(i);
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void AddAfter_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _list.AddLast(i);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                _list.AddAfter(i, itemCount + i);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                Assert.AreEqual(i, _list.First);
                _list.RemoveFirst();
                Assert.AreEqual(itemCount + i, _list.First);
                _list.RemoveFirst();
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void AddBefore_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _list.AddLast(i);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                _list.AddBefore(i, itemCount + i);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                Assert.AreEqual(itemCount + i, _list.First);
                _list.RemoveFirst();
                Assert.AreEqual(i, _list.First);
                _list.RemoveFirst();
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Remove_DifferentItems_SameOrder(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _list.AddFirst(i);
            }
            for (var i = itemCount - 1; i > 0; --i)
            {
                Assert.True(_list.Remove(i));
                Assert.AreEqual(i - 1, _list.First);
            }
            Assert.True(_list.Remove(0));
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
                _list.AddFirst(i);
                heap.Add(rand.Next(), i);
            }
            foreach (var i in heap)
            {
                Assert.True(_list.Remove(i.Value));
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void RemoveAfter_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _list.AddLast(i);
                _list.AddLast(itemCount + i);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                _list.RemoveAfter(i);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                Assert.AreEqual(i, _list.First);
                Assert.AreEqual(itemCount - i, _list.Count);
                _list.RemoveFirst();
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void RemoveBefore_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _list.AddLast(itemCount + i);
                _list.AddLast(i);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                _list.RemoveBefore(i);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                Assert.AreEqual(i, _list.First);
                Assert.AreEqual(itemCount - i, _list.Count);
                _list.RemoveFirst();
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void AddLast_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _list.AddLast(i);
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void RemoveFirst_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _list.Add(i);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                Assert.AreEqual(i, _list.First);
                Assert.AreEqual(itemCount - i, _list.Count);
                _list.RemoveFirst();
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void GetReversedEnumerator_NotNull_FullList(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _list.Add(i);
            }
            Assert.NotNull(_list.GetReversedEnumerator());
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void GetReversedEnumerator_FullList(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _list.Add(i);
            }
            var revEn = _list.GetReversedEnumerator();
            for (var i = itemCount - 1; i >= 0; --i)
            {
                Assert.True(revEn.MoveNext());
                Assert.AreEqual(i, revEn.Current);
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void RemoveLast_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _list.Add(i);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                Assert.AreEqual(itemCount - i - 1, _list.Last);
                Assert.AreEqual(itemCount - i, _list.Count);
                _list.RemoveLast();
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Reverse_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _list.Add(i);
            }
            _list.Reverse();
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

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAfter_EmptyList()
        {
            _list.AddAfter(5, 7);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAfter_ItemNotContained()
        {
            _list.Add(1);
            _list.Add(3);
            _list.Add(5);
            _list.AddAfter(2, 8);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAfter_NewItemContained()
        {
            _list.Add(1);
            _list.Add(3);
            _list.Add(5);
            _list.AddAfter(1, 5);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddBefore_EmptyList()
        {
            _list.AddBefore(5, 7);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddBefore_ItemNotContained()
        {
            _list.Add(1);
            _list.Add(3);
            _list.Add(5);
            _list.AddBefore(2, 8);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddBefore_NewItemContained()
        {
            _list.Add(1);
            _list.Add(3);
            _list.Add(5);
            _list.AddBefore(5, 1);
        }

        [Test]
        public void Factory_CustomEqualityComparer()
        {
            var eqCmp = new DummyEqualityComparer();
            Assert.AreSame(new HashLinkedList<int>(eqCmp).EqualityComparer, eqCmp);
        }

        [Test]
        public void Factory_DefaultEqualityComparer()
        {
            Assert.AreSame(new HashLinkedList<int>().EqualityComparer, EqualityComparer<int>.Default);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Factory_NullEqualityComparer()
        {
            // ReSharper disable AssignNullToNotNullAttribute ReSharper disable UnusedVariable
            var l = new HashLinkedList<int>(null);
            // ReSharper restore UnusedVariable ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void First_NoItems()
        {
            // ReSharper disable UnusedVariable
            var f = _list.First;
            // ReSharper restore UnusedVariable
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void First_NoItems_AfterAddRemove()
        {
            _list.Add(5);
            _list.RemoveFirst();
            // ReSharper disable UnusedVariable
            var f = _list.First;
            // ReSharper restore UnusedVariable
        }

        [Test]
        public void GetReversedEnumerator_EmptyList()
        {
            Assert.False(_list.GetReversedEnumerator().MoveNext());
        }

        [Test]
        public void GetReversedEnumerator_NotNull_EmptyList()
        {
            Assert.NotNull(_list.GetReversedEnumerator());
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Last_NoItems()
        {
            // ReSharper disable UnusedVariable
            var l = _list.Last;
            // ReSharper restore UnusedVariable
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Last_NoItems_AfterAddRemove()
        {
            _list.Add(5);
            _list.RemoveFirst();
            // ReSharper disable UnusedVariable
            var l = _list.Last;
            // ReSharper restore UnusedVariable
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveAfter_EmptyList()
        {
            _list.RemoveAfter(5);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveAfter_ItemNotContained()
        {
            _list.Add(1);
            _list.Add(3);
            _list.Add(5);
            _list.RemoveAfter(2);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveAfter_LastItem()
        {
            _list.Add(1);
            _list.Add(3);
            _list.Add(5);
            _list.RemoveAfter(5);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveBefore_EmptyList()
        {
            _list.RemoveBefore(5);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveBefore_FirstItem()
        {
            _list.Add(1);
            _list.Add(3);
            _list.Add(5);
            _list.RemoveBefore(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveBefore_ItemNotContained()
        {
            _list.Add(1);
            _list.Add(3);
            _list.Add(5);
            _list.RemoveBefore(2);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveFirst_NoItems()
        {
            _list.RemoveFirst();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveLast_NoItems()
        {
            _list.RemoveLast();
        }

        [Test]
        public void Remove_ContainedItem()
        {
            _list.Add(0);
            Assert.True(_list.Remove(0));
        }

        [Test]
        public void Remove_NotContainedItem()
        {
            Assert.False(_list.Remove(0));
        }
    }
}
