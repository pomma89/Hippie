// MockedDoublyLinkedList.cs
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

using DIBRIS.Hippie.Core.LinkedLists;
using NUnit.Framework;
using System.Collections.Generic;

namespace DIBRIS.Hippie.UnitTests.Core.LinkedLists
{
    internal class MockedDoublyLinkedList<TList, TItem> : MockedLinkedList<TList, TItem>, IDoublyLinkedList<TItem>
        where TList : class, IDoublyLinkedList<TItem>
    {
        public MockedDoublyLinkedList(TList testList)
            : base(testList)
        {
        }

        public void Append(IDoublyLinkedList<TItem> list)
        {
            if (!ReferenceEquals(list, null))
            {
                foreach (var item in list)
                {
                    RefList.AddLast(item);
                }
            }
            TestList.Append(list);
            Assert.AreEqual(RefList.Count, TestList.Count);
            Assert.AreEqual(RefList.First.Value, TestList.First);
            Assert.AreEqual(RefList.Last.Value, TestList.Last);
        }

        public IEnumerator<TItem> GetReversedEnumerator()
        {
            return TestList.GetReversedEnumerator();
        }

        public TItem RemoveLast()
        {
            var last = TestList.RemoveLast();
            RefList.RemoveLast();
            Assert.AreEqual(TestList.Count, RefList.Count);
            if (TestList.Count > 0)
            {
                Assert.AreEqual(RefList.Last.Value, TestList.Last);
            }
            return last;
        }

        public void Reverse()
        {
            TestList.Reverse();
            var testEn = TestList.GetReversedEnumerator();
            var refEn = RefList.GetEnumerator();
            while (testEn.MoveNext() && refEn.MoveNext())
            {
                Assert.AreEqual(testEn.Current, refEn.Current);
            }
            Assert.AreEqual(testEn.MoveNext(), refEn.MoveNext());
            TestList.Reverse();
        }
    }
}