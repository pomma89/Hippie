// File name: TestUtils.cs
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
    using DIBRIS.Hippie;
    using System;
    using System.Collections.Generic;

    public sealed class RandomGraph
    {
        private const int MinEdgeLength = 1;
        private const int MaxEdgeLength = 1000;
        private readonly LinkedList<Edge>[] _edges;
        private readonly int _nodeCount;

        public RandomGraph(int nodeCount, double edgeProb)
        {
            _nodeCount = nodeCount;
            _edges = new LinkedList<Edge>[nodeCount];
            for (var i = 0; i < nodeCount; ++i)
            {
                _edges[i] = new LinkedList<Edge>();
            }

            var rand = new Random();
            for (var i = 0; i < nodeCount; ++i)
            {
                for (var j = 0; j < nodeCount; ++j)
                {
                    if (i == j || rand.NextDouble() > edgeProb)
                    {
                        continue;
                    }
                    var edgeLength = rand.Next(MinEdgeLength, MaxEdgeLength);
                    _edges[i].AddLast(new Edge(j, edgeLength));
                }
            }
        }

        public void Clear()
        {
            foreach (var e in _edges)
            {
                e.Clear();
            }
        }

        public int[] Dijkstra(IRawHeap<int, int> prQueue, int start)
        {
            var distances = new int[_nodeCount];
            var visited = new bool[_nodeCount];
            var nodes = new IHeapHandle<int, int>[_nodeCount];
            for (var i = 0; i < _nodeCount; ++i)
            {
                nodes[i] = prQueue.Add(i, int.MaxValue);
                distances[i] = int.MaxValue;
            }
            prQueue[nodes[start]] = 0;

            while (prQueue.Count != 0)
            {
                var u = prQueue.RemoveMin();
                if (u.Priority == int.MaxValue)
                {
                    break;
                }
                var uId = u.Value;
                distances[uId] = u.Priority;
                visited[uId] = true;
                foreach (var e in _edges[uId])
                {
                    if (visited[e.Target])
                    {
                        continue;
                    }
                    var tmpDist = u.Priority + e.Length;
                    var v = nodes[e.Target];
                    if (tmpDist < v.Priority)
                    {
                        prQueue[v] = tmpDist;
                    }
                }
            }

            return distances;
        }

        public int[] Dijkstra(IHeap<int, int> prQueue, int start)
        {
            var distances = new int[_nodeCount];
            var visited = new bool[_nodeCount];
            for (var i = 0; i < _nodeCount; ++i)
            {
                distances[i] = int.MaxValue;
                prQueue.Add(i, int.MaxValue);
            }
            prQueue.UpdatePriorityOf(start, 0);

            while (prQueue.Count != 0)
            {
                var u = prQueue.RemoveMin();
                if (u.Priority == int.MaxValue)
                {
                    break;
                }
                var uId = u.Value;
                distances[uId] = u.Priority;
                visited[uId] = true;
                foreach (var e in _edges[uId])
                {
                    if (visited[e.Target])
                    {
                        continue;
                    }
                    var tmpDist = u.Priority + e.Length;
                    var v = e.Target;
                    if (tmpDist < prQueue[v])
                    {
                        prQueue.UpdatePriorityOf(v, tmpDist);
                    }
                }
            }

            return distances;
        }

        private struct Edge
        {
            public readonly int Length;
            public readonly int Target;

            public Edge(int target, int length)
            {
                Target = target;
                Length = length;
            }
        }
    }

    /// <summary>
    ///   </summary>
    public static class HeapSort
    {
        public static T[] Sort<T>(IRawHeap<T, T> heap, IEnumerable<T> elems)
        {
            foreach (var elem in elems)
            {
                heap.Add(elem, elem);
            }
            var orderedElems = new T[heap.Count];
            for (var i = 0; heap.Count > 0; ++i)
            {
                orderedElems[i] = heap.RemoveMin().Value;
            }
            return orderedElems;
        }

        /// <summary>
        ///   </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="heap"></param>
        /// <param name="elems"></param>
        /// <returns></returns>
        public static T[] Sort<T>(IHeap<T, T> heap, IEnumerable<T> elems)
        {
            foreach (var elem in elems)
            {
                heap.Add(elem, elem);
            }
            var orderedElems = new T[heap.Count];
            for (var i = 0; heap.Count > 0; ++i)
            {
                orderedElems[i] = heap.RemoveMin().Value;
            }
            return orderedElems;
        }

        /// <summary>
        ///   </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="heap"></param>
        /// <param name="elems"></param>
        /// <returns></returns>
        public static T[] Sort<T>(IHeap<T> heap, IEnumerable<T> elems)
        {
            foreach (var elem in elems)
            {
                heap.Add(elem);
            }
            var orderedElems = new T[heap.Count];
            for (var i = 0; heap.Count > 0; ++i)
            {
                orderedElems[i] = heap.RemoveMin();
            }
            return orderedElems;
        }

        public static T[] Sort<T>(IThinHeap<T, T> heap, IEnumerable<T> elems) where T : struct
        {
            foreach (var elem in elems)
            {
                heap.Add(elem, elem);
            }
            var orderedElems = new T[heap.Count];
            for (var i = 0; heap.Count > 0; ++i)
            {
                orderedElems[i] = heap.RemoveMin().Value;
            }
            return orderedElems;
        }
    }
}
