// 
// ErrorMessages.cs
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

namespace Hippie
{
    static class ErrorMessages
    {
        public const string DifferentComparers = "Merge is not allowed when heaps have not the same comparer.";
        public const string HandleContained = "Heap already contains given handle.";
        public const string InefficientMethod = "Implementation of this method would be inefficient.";
        public const string MergeConflict = "Some values are already held in the heap.";
        public const string NoHandle = "Heap does not contain given handle.";
        public const string NoMin = "Heap is empty, there is no minimum.";
        public const string NoValue = "Heap does not contain given value.";
        public const string NullComparer = "Comparer cannot be null.";
        public const string NullHandle = "Handle cannot be null.";
        public const string NullOther = "Other heap cannot be null.";
        public const string NullPriority = "Priority cannot be null, in order to avoid issues with comparers.";
        public const string NullValue = "Value cannot be null, in order to avoid issues with equality comparers.";
        public const string ValueContained = "Heap already contains given value.";
        public const string WrongChildCount = "Child count must be greater than or equal to two.";
    }
}