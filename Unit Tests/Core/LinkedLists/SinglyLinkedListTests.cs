// SinglyLinkedListTests.cs
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
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using NUnit.Framework;
using DIBRIS.Hippie.Core.LinkedLists;
using System;
using System.Collections.Generic;

namespace DIBRIS.Hippie.UnitTests.Core.LinkedLists
{
    internal abstract class SinglyLinkedListTests<TList> : ThinLinkedListTests<TList> where TList : class, ILinkedList<int>
    {
        protected override TList GetList()
        {
            var lst = (TList) (new SinglyLinkedList<int>() as ILinkedList<int>);
            ILinkedList<int> ret = new MockedLinkedList<TList, int>(lst);
            return (TList) ret;
        }

        protected override TList GetList(IEqualityComparer<int> equalityComparer)
        {
            var lst = (TList) (new SinglyLinkedList<int>(equalityComparer) as ILinkedList<int>);
            ILinkedList<int> ret = new MockedLinkedList<TList, int>(lst);
            return (TList) ret;
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void AddLast_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddLast(i);
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void AddLast_SameItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddLast(0);
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Append_DifferentItems(int itemCount)
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
        public void Append_SameItems(int itemCount)
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void Append_NullList()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            List.Append(null);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void Append_EmptyList()
        {
            List.Add(1);
            List.Append(new SinglyLinkedList<int>());
            Assert.AreEqual(1, List.Count);
            Assert.AreEqual(1, List.First);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Last_NoItems()
        {
            // ReSharper disable UnusedVariable
            var l = List.Last;
            // ReSharper restore UnusedVariable
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Last_NoItems_AfterAddRemove()
        {
            List.Add(5);
            List.RemoveFirst();
            // ReSharper disable UnusedVariable
            var l = List.Last;
            // ReSharper restore UnusedVariable
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void Remove_DifferentItems_ReverseOrder(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.AddFirst(i);
            }
            for (var i = 0; i < itemCount - 1; ++i)
            {
                Assert.True(List.Remove(i));
                Assert.AreEqual(i + 1, List.Last);
            }
            Assert.True(List.Remove(itemCount - 1));
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void RemoveFirst_DifferentItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.Add(i);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                Assert.AreEqual(i, List.First);
                Assert.AreEqual(itemCount - i, List.Count);
                List.RemoveFirst();
            }
        }

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void RemoveFirst_SameItems(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                List.Add(0);
            }
            for (var i = 0; i < itemCount; ++i)
            {
                Assert.AreEqual(0, List.First);
                Assert.AreEqual(itemCount - i, List.Count);
                List.RemoveFirst();
            }
        }
    }

    [TestFixture]
    internal sealed class SinglyLinkedListTests : SinglyLinkedListTests<ILinkedList<int>>
    {
    }
}