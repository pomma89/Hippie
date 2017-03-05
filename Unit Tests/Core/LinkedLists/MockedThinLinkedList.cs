// MockedThinLinkedList.cs
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
using System.Collections;
using System.Collections.Generic;
using PommaLabs.CodeServices.Common.Collections;
using NUnit.Framework;

namespace PommaLabs.CodeServices.UnitTests.Common.Collections
{
    class MockedThinLinkedList<TList, TItem> : IThinLinkedList<TItem> where TList : class, IThinLinkedList<TItem>
    {
        protected readonly LinkedList<TItem> RefList;
        protected readonly TList TestList;

        public MockedThinLinkedList(TList testList)
        {
            TestList = testList;
            RefList = new LinkedList<TItem>();
        }

        public int Count
        {
            get { return TestList.Count; }
        }

        public IEqualityComparer<TItem> EqualityComparer
        {
            get { return TestList.EqualityComparer; }
        }

        public bool IsReadOnly
        {
            get { return TestList.IsReadOnly; }
        }

        public TItem First
        {
            get { return TestList.First; }
        }

        public virtual void Add(TItem item)
        {
            AddFirst(item);
        }

        public void AddFirst(TItem item)
        {
            TestList.AddFirst(item);
            RefList.AddFirst(item);
            Assert.AreEqual(RefList.Count, TestList.Count);
            Assert.AreEqual(RefList.First.Value, TestList.First);
        }

        public void Clear()
        {
            TestList.Clear();
            RefList.Clear();
        }

        public bool Contains(TItem item)
        {
            var r1 = TestList.Contains(item);
            var r2 = TestList.Contains(item);
            Assert.AreEqual(r2, r1);
            Assert.AreEqual(RefList.Count, TestList.Count);
            return r1;
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return TestList.GetEnumerator();
        }

        public virtual bool Remove(TItem item)
        {
            var r1 = TestList.Remove(item);
            var r2 = RefList.Remove(item);
            Assert.AreEqual(r2, r1);
            Assert.AreEqual(RefList.Count, TestList.Count);
            if (TestList.Count > 0)
            {
                Assert.AreEqual(RefList.First.Value, TestList.First);
            }
            return r1;
        }

        public TItem RemoveFirst()
        {
            var first = TestList.RemoveFirst();
            RefList.RemoveFirst();
            Assert.AreEqual(RefList.Count, TestList.Count);
            if (TestList.Count > 0)
            {
                Assert.AreEqual(RefList.First.Value, TestList.First);
            }
            return first;
        }
    }
}
