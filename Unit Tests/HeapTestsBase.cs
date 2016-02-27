// File name: HeapTestsBase.cs
// 
// Author(s): Alessio Parma <alessio.parma@gmail.com>
// 
// Copyright (c) 2012-2016 Alessio Parma <alessio.parma@gmail.com>
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
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public abstract class HeapTestsBase
    {
        protected const int StringValueCount = 21; // Our birthdays... :)
        protected const int StringItemLength = 21;

        protected static readonly Random Rand = new Random();

        protected sealed class FakeIntEqualityComparer : IEqualityComparer<int>
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

        protected sealed class ReversedIntComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return -x.CompareTo(y);
            }
        }

        protected class A : IComparable<A>
        {
            private readonly int _x;

            public A(int x)
            {
                _x = x;
            }

            public int CompareTo(A other)
            {
                return _x.CompareTo(other._x);
            }
        }

        protected sealed class B : A
        {
            public B(int x)
                : base(x)
            {
            }
        }
    }

    public abstract class ValPrHeapTests : HeapTestsBase
    {
        protected const int IntValueCount = 6120;
        private const int DijkstraValueCount = 3060;

        protected static readonly HashSet<int> RandSet = new HashSet<int>();

        protected static readonly List<int> IntValues = new List<int>();
        protected static readonly List<int> RandomIntValues = new List<int>();
        protected static readonly List<string> StringValues = new List<string>();

        protected static readonly RandomGraph EmptyGraph = new RandomGraph(DijkstraValueCount, 0.0);
        protected static readonly RandomGraph SparseGraph = new RandomGraph(DijkstraValueCount, 0.15);
        protected static readonly RandomGraph MediumGraph = new RandomGraph(DijkstraValueCount, 0.3);

        static ValPrHeapTests()
        {
            for (var i = IntValueCount - 1; i >= 0; --i)
                IntValues.Add(i);

            for (var i = 0; i < IntValueCount; ++i)
                RandomIntValues.Add(NextRandInt());

            for (var i = StringValueCount - 1; i >= 0; --i)
                StringValues.Add(new string((char) ('a' + i), StringItemLength));
        }

        protected static int NextRandInt()
        {
            int r;
            while (RandSet.Contains(r = Rand.Next() + IntValueCount)) { }
            RandSet.Add(r);
            return r;
        }
    }
}
