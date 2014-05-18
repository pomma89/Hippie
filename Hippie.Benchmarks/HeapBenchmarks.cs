// 
// HeapBenchmarks.cs
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

namespace Hippie.Benchmarks
{
    using System;
    using System.Linq;
    using Benjamin.Framework;
    using Tests;

    [BenchmarkFixture]
    public abstract class BaseHeapBenchmarks<TVal>
    {
        const string AddComparison = "Add";
        const string ChangePriorityComparison = "UpdatePriorityOf";
        const string MergeComparison = "Remove";
        const string RemoveComparison = "Remove";
        const string RemoveMinComparison = "RemoveMin";
        const int MinExp = 3;
        const int MaxExp = 5;
        const int Fraction = 4;
        const int StepCount = (MaxExp - MinExp)*Fraction;
        protected readonly TVal[] MergeValues;
        protected readonly TVal[] Values;
        readonly Random rand = new Random();
        readonly int[] steps = new int[StepCount];
        protected IHeap<TVal, int> Heap;

        protected BaseHeapBenchmarks()
        {
            for (var i = 0; i < (MaxExp - MinExp); ++i) {
                var p = (int) Math.Pow(10, i + MinExp);
                for (var j = 0; j < Fraction; ++j) {
                    steps[Fraction*i + j] = p + p*j*10/Fraction;
                }
            }

            Values = new TVal[steps[StepCount - 1]];
            MergeValues = new TVal[steps[StepCount - 1]];
        }

        protected abstract IHeap<TVal, int> GetHeap();

        [SetUp]
        public void SetUp()
        {
            Heap = GetHeap();
        }

        [TearDown]
        public void TearDown()
        {
            Heap.Clear();
        }

        /**********************************************************************
		 * Min
		 **********************************************************************/

        [Benchmark(StepCount)]
        public void Min_OrderedValues(IBenchmarkContext ctx)
        {
            Min_Benchmark(ctx, AddOrderedValues);
        }

        [Benchmark(StepCount)]
        public void Min_ReverseOrderedValues(IBenchmarkContext ctx)
        {
            Min_Benchmark(ctx, AddReverseOrderedValues);
        }

        [Benchmark(StepCount)]
        public void Min_RandomValues(IBenchmarkContext ctx)
        {
            Min_Benchmark(ctx, AddRandomValues);
        }

        void Min_Benchmark(IBenchmarkContext ctx, AddMethod addMethod)
        {
            addMethod(GetValueCount(ctx));
            ctx.StartMonitors();
            var min = Heap.Min;
            ctx.StopMonitors();
        }

        /**********************************************************************
		 * Add
		 **********************************************************************/

        [Benchmark(StepCount)]
        [Comparison(AddComparison, Visibility.Fixture)]
        public void Add_OrderedValues(IBenchmarkContext ctx)
        {
            Add_Benchmark(ctx, AddOrderedValues);
        }

        [Benchmark(StepCount)]
        [Comparison(AddComparison, Visibility.Fixture)]
        public void Add_ReverseOrderedValues(IBenchmarkContext ctx)
        {
            Add_Benchmark(ctx, AddReverseOrderedValues);
        }

        [Benchmark(StepCount)]
        [Comparison(AddComparison, Visibility.Fixture & Visibility.Assembly)]
        public void Add_RandomValues(IBenchmarkContext ctx)
        {
            Add_Benchmark(ctx, AddRandomValues);
        }

        void Add_Benchmark(IBenchmarkContext ctx, AddMethod addMethod)
        {
            var valueCount = GetValueCount(ctx);
            ctx.StartMonitors();
            addMethod(valueCount);
            ctx.StopMonitors();
        }

        /**********************************************************************
		 * UpdatePriorityOf
		 **********************************************************************/

        [Benchmark(StepCount)]
        [Comparison(ChangePriorityComparison)]
        public void ChangePriority_RandomValues(IBenchmarkContext ctx)
        {
            var valueCount = GetValueCount(ctx);
            AddRandomValues(valueCount);
            var values = Heap.Select(p => p.Value).ToArray();
            ctx.StartMonitors();
            for (var i = 0; i < valueCount; ++i) {
                Heap.UpdatePriorityOf(values[i], rand.Next());
            }
            ctx.StopMonitors();
        }

        /**********************************************************************
		 * Merge
		 **********************************************************************/

        [Benchmark(StepCount)]
        [Comparison(MergeComparison)]
        public void Merge_RandomValues(IBenchmarkContext ctx)
        {
            var valueCount = GetValueCount(ctx);
            AddRandomValues(valueCount);
            var otherHeap = GetHeap();
            for (var i = 0; i < valueCount; ++i) {
                otherHeap.Add(MergeValues[i], rand.Next());
            }
            ctx.StartMonitors();
            Heap.Merge(otherHeap);
            ctx.StopMonitors();
        }

        /**********************************************************************
		 * Remove
		 **********************************************************************/

        [Benchmark(StepCount)]
        [Comparison(RemoveComparison, Visibility.Fixture)]
        public void Remove_OrderedValues(IBenchmarkContext ctx)
        {
            Remove_Benchmark(ctx, AddOrderedValues);
        }

        [Benchmark(StepCount)]
        [Comparison(RemoveComparison, Visibility.Fixture)]
        public void Remove_ReverseOrderedValues(IBenchmarkContext ctx)
        {
            Remove_Benchmark(ctx, AddReverseOrderedValues);
        }

        [Benchmark(StepCount)]
        [Comparison(RemoveComparison, Visibility.Fixture & Visibility.Assembly)]
        public void Remove_RandomValues(IBenchmarkContext ctx)
        {
            Remove_Benchmark(ctx, AddRandomValues);
        }

        void Remove_Benchmark(IBenchmarkContext ctx, AddMethod addMethod)
        {
            var valueCount = GetValueCount(ctx);
            addMethod(valueCount);
            ctx.StartMonitors();
            for (var i = 0; i < valueCount; ++i) {
                Heap.Remove(Values[i]);
            }
            ctx.StopMonitors();
        }

        /**********************************************************************
		 * RemoveMin
		 **********************************************************************/

        [Benchmark(StepCount)]
        [Comparison(RemoveMinComparison, Visibility.Fixture)]
        public void RemoveMin_OrderedValues(IBenchmarkContext ctx)
        {
            RemoveMin_Benchmark(ctx, AddOrderedValues);
        }

        [Benchmark(StepCount)]
        [Comparison(RemoveMinComparison, Visibility.Fixture)]
        public void RemoveMin_ReverseOrderedValues(IBenchmarkContext ctx)
        {
            RemoveMin_Benchmark(ctx, AddReverseOrderedValues);
        }

        [Benchmark(StepCount)]
        [Comparison(RemoveMinComparison, Visibility.Fixture & Visibility.Assembly)]
        public void RemoveMin_RandomValues(IBenchmarkContext ctx)
        {
            RemoveMin_Benchmark(ctx, AddRandomValues);
        }

        void RemoveMin_Benchmark(IBenchmarkContext ctx, AddMethod addMethod)
        {
            addMethod(GetValueCount(ctx));
            ctx.StartMonitors();
            while (Heap.Count != 0) {
                Heap.RemoveMin();
            }
            ctx.StopMonitors();
        }

        /**********************************************************************
		 * Dijkstra
		 **********************************************************************/

        [Benchmark(500, 3500, 500)]
        public void Dijkstra_DenseGraph(IBenchmarkContext ctx)
        {
            Dijkstra_Benchmark(ctx, 0.21);
        }

        [Benchmark(1000, 6000, 500)]
        public void Dijkstra_SparseGraph(IBenchmarkContext ctx)
        {
            Dijkstra_Benchmark(ctx, 0.07);
        }

        protected abstract void Dijkstra_Benchmark(IBenchmarkContext ctx, double edgeProb);

        /**********************************************************************
		 * HeapSort
		 **********************************************************************/

        [Benchmark(StepCount)]
        public void HeapSort_OrderedValues(IBenchmarkContext ctx)
        {
            HeapSort_Benchmark(ctx, AddOrderedValues);
        }

        [Benchmark(StepCount)]
        public void HeapSort_RandomValues(IBenchmarkContext ctx)
        {
            HeapSort_Benchmark(ctx, AddRandomValues);
        }

        void HeapSort_Benchmark(IBenchmarkContext ctx, AddMethod addMethod)
        {
            var valueCount = GetValueCount(ctx);
            ctx.StartMonitors();
            addMethod(valueCount);
            while (Heap.Count != 0) {
                Heap.RemoveMin();
            }
            ctx.StopMonitors();
        }

        /**********************************************************************
		 * Protected methods
		 **********************************************************************/

        /**********************************************************************
		 * Private methods
		 **********************************************************************/

        int GetValueCount(IBenchmarkContext ctx)
        {
            var valueCount = steps[(int) ctx.Step];
            ctx.Step = valueCount;
            return valueCount;
        }

        void AddOrderedValues(int valueCount)
        {
            for (var i = 0; i < valueCount; ++i) {
                Heap.Add(Values[i], i);
            }
        }

        void AddReverseOrderedValues(int valueCount)
        {
            for (var i = valueCount - 1; i >= 0; --i) {
                Heap.Add(Values[i], i);
            }
        }

        void AddRandomValues(int valueCount)
        {
            for (var i = 0; i < valueCount; ++i) {
                Heap.Add(Values[i], rand.Next());
            }
        }

        delegate void AddMethod(int valueCount);
    }

    public abstract class RawHeapBenchmarks : BaseHeapBenchmarks<int>
    {
        protected RawHeapBenchmarks()
        {
            var length = Values.Length;
            for (var i = 0; i < length; ++i) {
                Values[i] = i;
                MergeValues[i] = i + length;
            }
        }

        protected override void Dijkstra_Benchmark(IBenchmarkContext ctx, double edgeProb)
        {
            var graph = new RandomGraph((int) ctx.Step, edgeProb);
            ctx.StartMonitors();
            graph.Dijkstra(Heap, 0);
            ctx.StopMonitors();
            graph.Clear();
        }
    }

    //public class RawBinaryHeapBenchmarks : RawHeapBenchmarks
    //{
    //    protected override IRawHeap<int, int> GetHeap()
    //    {
    //        return HeapFactory.NewRawBinaryHeap<int, int>();
    //    }
    //}

    //public class RawBinomialHeapBenchmarks : RawHeapBenchmarks
    //{
    //    protected override IRawHeap<int, int> GetHeap()
    //    {
    //        return HeapFactory.NewRawBinomialHeap<int, int>();
    //    }
    //}

    //public class RawFibonacciHeapBenchmarks : RawHeapBenchmarks
    //{
    //    protected override IRawHeap<int, int> GetHeap()
    //    {
    //        return HeapFactory.NewRawFibonacciHeap<int, int>();
    //    }
    //}

    //public class RawPairingHeapBenchmarks : RawHeapBenchmarks
    //{
    //    protected override IRawHeap<int, int> GetHeap()
    //    {
    //        return HeapFactory.NewRawPairingHeap<int, int>();
    //    }
    //}

    //public class RawTernaryHeapBenchmarks : RawHeapBenchmarks
    //{
    //    protected override IRawHeap<int, int> GetHeap()
    //    {
    //        return HeapFactory.NewRawArrayHeap<int, int>(3);
    //    }
    //}

    public abstract class UniqueHeapBenchmarks : BaseHeapBenchmarks<int>
    {
        protected UniqueHeapBenchmarks()
        {
            var length = Values.Length;
            for (var i = 0; i < length; ++i) {
                Values[i] = i;
                MergeValues[i] = i + length;
            }
        }

        protected override void Dijkstra_Benchmark(IBenchmarkContext ctx, double edgeProb)
        {
            var graph = new RandomGraph((int) ctx.Step, edgeProb);
            ctx.StartMonitors();
            graph.Dijkstra(Heap, 0);
            ctx.StopMonitors();
            Heap.Clear();
            graph.Clear();
        }
    }

    public class UniqueBinaryHeapBenchmarks : UniqueHeapBenchmarks
    {
        protected override IHeap<int, int> GetHeap()
        {
            return HeapFactory.NewBinaryHeap<int, int>();
        }
    }

    public class UniqueBinomialHeapBenchmarks : UniqueHeapBenchmarks
    {
        protected override IHeap<int, int> GetHeap()
        {
            return HeapFactory.NewBinomialHeap<int, int>();
        }
    }

    public class UniqueFibonacciHeapBenchmarks : UniqueHeapBenchmarks
    {
        protected override IHeap<int, int> GetHeap()
        {
            return HeapFactory.NewFibonacciHeap<int, int>();
        }
    }

    public class UniquePairingHeapBenchmarks : UniqueHeapBenchmarks
    {
        protected override IHeap<int, int> GetHeap()
        {
            return HeapFactory.NewPairingHeap<int, int>();
        }
    }

    public class UniqueTernaryHeapBenchmarks : UniqueHeapBenchmarks
    {
        protected override IHeap<int, int> GetHeap()
        {
            return HeapFactory.NewArrayHeap<int, int>(3);
        }
    }
}