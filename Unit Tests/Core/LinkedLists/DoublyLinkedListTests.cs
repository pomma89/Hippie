// DoublyLinkedListTests.cs
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
    abstract class DoublyLinkedListTests<TList> : SinglyLinkedListTests<TList> where TList : class, IDoublyLinkedList<int>
    {
        protected override TList GetList()
        {
            var lst = (TList) (new DoublyLinkedList<int>() as IDoublyLinkedList<int>);
            IDoublyLinkedList<int> ret = new MockedDoublyLinkedList<TList, int>(lst);
            return (TList) ret;
        }

        protected override TList GetList(IEqualityComparer<int> equalityComparer)
        {
            var lst = (TList) (new DoublyLinkedList<int>(equalityComparer) as IDoublyLinkedList<int>);
            IDoublyLinkedList<int> ret = new MockedDoublyLinkedList<TList, int>(lst);
            return (TList) ret;
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Append_Doubly_DifferentItems(int itemCount)
        {
            var otherList = GetList();
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddLast(i);
                otherList.Add(itemCount + i);
            }
            List.Append(otherList);
            for (var i = 0; i < itemCount * 2; ++i)
            {
                Assert.AreEqual(i, List.First);
                Assert.AreEqual(2 * itemCount - i, List.Count);
                List.RemoveFirst();
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Append_Doubly_SameItems(int itemCount)
        {
            var otherList = GetList();
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddLast(i);
                otherList.Add(i);
            }
            List.Append(otherList);
            for (var i = 0; i < itemCount; ++i)
            {
                Assert.AreEqual(i, List.First);
                Assert.AreEqual(2 * itemCount - i, List.Count);
                List.RemoveFirst();
            }
            for (var i = 0; i < itemCount; ++i)
            {
                Assert.AreEqual(i, List.First);
                Assert.AreEqual(itemCount - i, List.Count);
                List.RemoveFirst();
            }
        }

        [Test]
        public void GetReversedEnumerator_NotNull_EmptyList()
        {
            Assert.NotNull(List.GetReversedEnumerator());
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void GetReversedEnumerator_NotNull_FullList(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.Add(i);
            }
            Assert.NotNull(List.GetReversedEnumerator());
        }

        [Test]
        public void GetReversedEnumerator_EmptyList()
        {
            Assert.False(List.GetReversedEnumerator().MoveNext());
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void GetReversedEnumerator_FullList(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.Add(i);
            }
            var revEn = List.GetReversedEnumerator();
            for (var i = itemCount - 1; i >= 0; --i)
            {
                Assert.True(revEn.MoveNext());
                Assert.AreEqual(i, revEn.Current);
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveLast_NoItems()
        {
            List.RemoveLast();
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void RemoveLast_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.Add(i);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                Assert.AreEqual(itemCount - i - 1, List.Last);
                Assert.AreEqual(itemCount - i, List.Count);
                List.RemoveLast();
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void RemoveLast_SameItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.Add(0);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                Assert.AreEqual(0, List.Last);
                Assert.AreEqual(itemCount - i, List.Count);
                List.RemoveFirst();
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Reverse_SameItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.Add(0);
            }
            List.Reverse();
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Reverse_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.Add(i);
            }
            List.Reverse();
        }
    }

    [TestFixture]
sealed class DoublyLinkedListTests : DoublyLinkedListTests<IDoublyLinkedList<int>>
    {
    }
}
