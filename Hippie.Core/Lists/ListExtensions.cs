// Copyright 2015-2025 Alessio Parma <alessio.parma@gmail.com>
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
// in compliance with the License. You may obtain a copy of the License at:
// 
// "http://www.apache.org/licenses/LICENSE-2.0"
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License
// is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
// or implied. See the License for the specific language governing permissions and limitations under
// the License.

using System;
using System.Collections;
using System.Collections.Generic;

namespace PommaLabs.CodeServices.Common.Collections
{
#if !NETSTD11

    public static class ListExtensions
    {
        // Sorts an IList<T> in place.
        public static void Sort<T>(this IList<T> list, Comparison<T> comparison)
        {
            ArrayList.Adapter((IList) list).Sort(new ComparisonComparer<T>(comparison));
        }
    }

#endif

    // Wraps a generic Comparison<T> delegate in an IComparer to make it easy to use a lambda
    // expression for methods that take an IComparer or IComparer<T>
    public class ComparisonComparer<T> : IComparer<T>, IComparer
    {
        private readonly Comparison<T> _comparison;

        public ComparisonComparer(Comparison<T> comparison)
        {
            _comparison = comparison;
        }

        public int Compare(T x, T y) => _comparison(x, y);

        public int Compare(object o1, object o2) => _comparison((T) o1, (T) o2);
    }
}
