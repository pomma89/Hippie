// File name: RawMessage.cs
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

namespace Examples
{
    using DIBRIS.Hippie;
    using System;

    sealed class RawMessage
    {
        RawMessage(string msg)
        {
            Message = msg;
        }

        string Message { get; set; }

        public static void Main()
        {
            var raw = HeapFactory.NewRawBinaryHeap<RawMessage, int>();
            var h1 = raw.Add(new RawMessage("Urgent message"), -100);
            var h2 = raw.Add(new RawMessage("Urgent message"), -50);
            raw.Add(new RawMessage("Silly message"), 10);

            // We update m1, it is not urgent anymore
            h1.Value.Message = "Not urgent anymore";
            raw.UpdatePriorityOf(h1, 200);

            // We update m2, it is more urgent!
            raw.UpdatePriorityOf(h2, -2000);

            while (raw.Count > 0)
            {
                var min = raw.RemoveMin();
                Console.WriteLine("Msg: ({0}), Weight: {1}", min.Value.Message, min.Priority);
                // Expected output:
                // Msg: (Urgent message), Weight: -2000;
                // Msg: (Silly message), Weight: 10
                // Msg: (Not urgent anymore), Weight: 200
            }
        }
    }
}
