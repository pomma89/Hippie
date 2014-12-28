// 
// HeapSort.cs
//  
// Author:
//       Alessio Parma <alessio.parma@gmail.com>
// 
// Copyright (c) 2012-2014 Alessio Parma <alessio.parma@gmail.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Hippie.Examples
{
    using System;
    using System.Collections.Generic;

    static class HeapSort
    {
        public static void Main()
        {
            var ints = Sort(new[] {9, 8, 7, 6, 5, 4, 3, 2, 1});
            foreach (var i in ints) {
                Console.Write(i + " "); // Expected output: 1 2 3 4 5 6 7 8 9
            }
            Console.WriteLine();

            var cmp = new ReversedIntComparer();
            ints = Sort(new[] {1, 2, 3, 4, 5, 6, 7, 8, 9}, cmp);
            foreach (var i in ints) {
                Console.Write(i + " "); // Expected output: 9 8 7 6 5 4 3 2 1
            }
            Console.WriteLine();
        }

        static IList<T> Sort<T>(IEnumerable<T> elems) where T : IComparable<T>
        {
            var heap = HeapFactory.NewBinaryHeap<T>();
            foreach (var elem in elems) {
                heap.Add(elem);
            }
            var list = new List<T>(heap.Count);
            while (heap.Count != 0) {
                list.Add(heap.RemoveMin());
            }
            return list;
        }

        static IList<T> Sort<T>(IEnumerable<T> elems, IComparer<T> cmp)
        {
            var heap = HeapFactory.NewBinaryHeap(cmp);
            foreach (var elem in elems) {
                heap.Add(elem);
            }
            var list = new List<T>(heap.Count);
            while (heap.Count != 0) {
                list.Add(heap.RemoveMin());
            }
            return list;
        }

        sealed class ReversedIntComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return -1*x.CompareTo(y);
            }
        }
    }
}