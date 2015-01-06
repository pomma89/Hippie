// 
// Dijkstra.cs
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

namespace Examples
{
    using System.Collections.Generic;
    using DIBRIS.Hippie;

    static class Dijkstra
    {
        struct Edge
        {
            public int Length;
            public int Target;
        }

        static int[] Traverse(IRawHeap<int, int> heap, IList<IEnumerable<Edge>> edges, int start, int nodeCount)
        {
            var distances = new int[nodeCount];
            var visited = new bool[nodeCount];
            var nodes = new IHeapHandle<int, int>[nodeCount];
            for (var i = 0; i < nodeCount; ++i) {
                nodes[i] = heap.Add(i, int.MaxValue);
                distances[i] = int.MaxValue;              
            }
            heap[nodes[start]] = 0;

            while (heap.Count != 0) {
                var u = heap.RemoveMin();
                if (u.Priority == int.MaxValue) {
                    break;
                }
                var uId = u.Value;
                distances[uId] = u.Priority;
                visited[uId] = true;
                foreach (var e in edges[uId]) {
                    if (visited[e.Target]) {
                        continue;
                    }
                    var tmpDist = u.Priority + e.Length;
                    var v = nodes[e.Target];
                    if (tmpDist < v.Priority) {
                        heap[v] = tmpDist;
                    }
                }
            }

            return distances;
        }
    }
}