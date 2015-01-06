// 
// WeightedMessage.cs
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
    using System;
    using DIBRIS.Hippie;

    sealed class WeightedMessage : IComparable<WeightedMessage>
    {
        WeightedMessage(string msg, int weight)
        {
            Message = msg;
            Weight = weight;
        }

        string Message { get; set; }
        int Weight { get; set; }

        public int CompareTo(WeightedMessage other)
        {
            return Weight.CompareTo(other.Weight);
        }

        bool Equals(WeightedMessage other)
        {
            return string.Equals(Message, other.Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            return ReferenceEquals(this, obj) || Equals(obj as WeightedMessage);
        }

        public override int GetHashCode()
        {
            return Message.GetHashCode();
        }

        public static void Main()
        {
            var multi = HeapFactory.NewBinaryHeap<WeightedMessage>();
            multi.Add(new WeightedMessage("Important message", 0));
            multi.Add(new WeightedMessage("Silly message", 100));
            multi.Add(new WeightedMessage("Urgent message", -10));
            multi.Add(new WeightedMessage("Urgent message", -50));
            multi.Add(new WeightedMessage("Important message", 0));
            while (multi.Count > 0) {
                var min = multi.RemoveMin();
                Console.WriteLine("Msg: ({0}), Weight: {1}", min.Message, min.Weight);
                // Expected output:
                // Msg: (Urgent message), Weight: -50
                // Msg: (Urgent message), Weight: -10
                // Msg: (Important message), Weight: 0
                // Msg: (Important message), Weight: 0
                // Msg: (Silly message), Weight: 100
            }
        }
    }
}