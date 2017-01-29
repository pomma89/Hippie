// ExtensionMethods.cs
// 
// Author: Alessio Parma <alessio.parma@gmail.com>
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

namespace DIBRIS.Hippie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class ExtensionMethods
    {
        public static void CommonCopyTo<TEn, TArr>(this ICollection<TEn> collection, TArr[] array, int idx,
                                                   Func<TEn, TArr> transform)
        {
            var enumerator = collection.GetEnumerator();
            var lastIndex = collection.Count + idx;
            for (var i = idx; i < lastIndex; ++i)
            {
                enumerator.MoveNext();
                array[i] = transform(enumerator.Current);
            }
        }

        public static void CommonMerge<TV1, TP1, TV2, TP2>(this IThinHeap<TV1, TP1> heap, IThinHeap<TV2, TP2> other)
            where TV2 : TV1
            where TP2 : TP1
        {
            if (ReferenceEquals(heap, other) || other.Count == 0)
            {
                return;
            }
            foreach (var handle in other)
            {
                heap.Add(handle.Value, handle.Priority);
            }
            other.Clear();
        }

        public static IEnumerable<IReadOnlyTree<TV2, TP2>> CommonToForest<TV1, TP1, TV2, TP2>(
            this IThinHeap<TV1, TP1> heap,
            Func<IReadOnlyTree<TV1, TP1>, ReadOnlyTree<TV2, TP2>, ReadOnlyTree<TV2, TP2>> transform)
        {
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var t in heap.ToReadOnlyForest())
            {
                var tt = t.BreadthFirstVisit(transform, null);
                yield return tt.ToList()[0];
            }
            // ReSharper restore LoopCanBeConvertedToQuery
        }

        public static string CommonToString<TV, TP>(this IThinHeap<TV, TP> heap)
        {
            return (heap.Count == 0) ? "[Count: 0]" : string.Format("[Count: {0}; Min: {1}]", heap.Count, heap.Min);
        }
    }
}