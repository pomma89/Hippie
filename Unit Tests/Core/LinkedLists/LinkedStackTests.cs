// LinkedStackTests.cs
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
    [TestFixture]
sealed class LinkedStackTests
    {
        [SetUp]
        public void SetUp()
        {
            _stack = new LinkedStack<int>();
        }

        [TearDown]
        public void TearDown()
        {
            _stack = null;
        }

        const int SmallCount = 10;
        const int MediumCount = 100;
        const int BigCount = 1000;

        ILinkedStack<int> _stack;

        [TestCase(SmallCount)]
        [TestCase(MediumCount)]
        [TestCase(BigCount)]
        public void SimpleTest(int itemCount)
        {
            for (var i = 0; i < itemCount; ++i)
            {
                _stack.Push(i);
            }
            Assert.AreEqual(itemCount, _stack.Count);
            for (var i = itemCount - 1; i >= 0; --i)
            {
                Assert.AreEqual(i, _stack.Top());
                Assert.AreEqual(i, _stack.Pop());
                Assert.AreEqual(i, _stack.Count);
            }
        }
    }
}
