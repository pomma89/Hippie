// File name: BetterComparer.cs
// 
// Author(s): Alessio Parma <alessio.parma@gmail.com>
// 
// Copyright (c) 2012-2014 Alessio Parma <alessio.parma@gmail.com>
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

namespace DIBRIS.Hippie
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using PommaLabs.Portability;

    /// <summary>
    ///   A class offering a better default comparer for given type parameter
    ///   <typeparamref name="T"/>. <br/> We need a particular version of <see cref="Comparer{T}"/>,
    ///   because in a situation like the next one: ~~~{.cs} class A : IComparable{A} { ... } class
    ///   B : A { ... } ~~~ We have that: ~~~{.cs} ReferenceEquals(Comparer{A}.Default,
    ///   Comparer{B}.Default) == false ~~~ When we need that statement to be true, in order to
    ///   allow meaningful merge operations. So, this class grants the following relation: ~~~{.cs}
    ///   ReferenceEquals(BetterComparer{A}.Default, BetterComparer{B}.Default) == true ~~~
    /// </summary>
    public static class BetterComparer<T> where T : IComparable<T>
    {
        private static readonly IComparer<T> DefaultCmp;

        static BetterComparer()
        {
            var type = typeof(T);
            var comparable = typeof(IComparable<T>);
            if (GTypeInfo.GetInterfaces(type).FirstOrDefault(i => ReferenceEquals(i, comparable)) != null)
            {
                DefaultCmp = new DefaultComparer();
                return;
            }
            var baseCmp = typeof(BetterComparer<>).MakeGenericType(new[] { GTypeInfo.GetBaseType(type) });
            var defCmp = GTypeInfo.GetProperties(baseCmp).First();
            DefaultCmp = (IComparer<T>) defCmp.GetValue(null, null);
        }

        /// <summary>
        ///   The default comparer for <typeparamref name="T"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static IComparer<T> Default
        {
            get { return DefaultCmp; }
        }

        private sealed class DefaultComparer : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return x.CompareTo(y);
            }
        }
    }
}