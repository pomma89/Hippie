// MockedLinkedList.cs
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

using PommaLabs.CodeServices.Common.Collections;
using NUnit.Framework;

namespace PommaLabs.CodeServices.UnitTests.Common.Collections
{
    class MockedLinkedList<TList, TItem> : MockedThinLinkedList<TList, TItem>, ILinkedList<TItem>
        where TList : class, ILinkedList<TItem>
    {
        public MockedLinkedList(TList testList)
            : base(testList)
        {
        }

        public TItem Last
        {
            get { return TestList.Last; }
        }

        public override sealed void Add(TItem item)
        {
            AddLast(item);
        }

        public void AddLast(TItem item)
        {
            TestList.AddLast(item);
            RefList.AddLast(item);
            Assert.AreEqual(RefList.Count, TestList.Count);
            Assert.AreEqual(RefList.Last.Value, TestList.Last);
        }

        public void Append(ILinkedList<TItem> list)
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

        public override bool Remove(TItem item)
        {
            var r1 = TestList.Remove(item);
            var r2 = RefList.Remove(item);
            Assert.AreEqual(r2, r1);
            Assert.AreEqual(RefList.Count, TestList.Count);
            if (TestList.Count > 0)
            {
                Assert.AreEqual(RefList.First.Value, TestList.First);
                Assert.AreEqual(RefList.Last.Value, TestList.Last);
            }
            return r1;
        }
    }
}
