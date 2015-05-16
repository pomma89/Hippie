// ReadOnlyTree.cs
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
    using Finsa.CodeServices.Common;

    internal sealed class ReadOnlyTree<TVal, TPr> : IReadOnlyTree<TVal, TPr>
    {
        private readonly List<ReadOnlyTree<TVal, TPr>> _children = new List<ReadOnlyTree<TVal, TPr>>();
        private readonly ReadOnlyTree<TVal, TPr> _parent;
        private readonly TPr _priority;
        private readonly TVal _value;

        public ReadOnlyTree(TVal value, TPr priority, ReadOnlyTree<TVal, TPr> parent)
        {
            _value = value;
            _priority = priority;
            _parent = parent;
            if (parent != null)
            {
                parent._children.Add(this);
            }
        }

        public TVal Value
        {
            get { return _value; }
        }

        public TPr Priority
        {
            get { return _priority; }
        }

        public IReadOnlyTree<TVal, TPr> Parent
        {
            get { return _parent; }
        }

        public IEnumerable<IReadOnlyTree<TVal, TPr>> Children
        {
            get { return _children; }
        }

        public IEnumerable<IReadOnlyTree<TVal, TPr>> BreadthFirstVisit()
        {
            var queue = new Queue<ReadOnlyTree<TVal, TPr>>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var t = queue.Dequeue();
                yield return t;
                foreach (var c in t._children)
                {
                    queue.Enqueue(c);
                }
            }
        }

        public void BreadthFirstVisit(Action<IReadOnlyTree<TVal, TPr>> visitor)
        {
            var queue = new Queue<ReadOnlyTree<TVal, TPr>>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var t = queue.Dequeue();
                visitor(t);
                foreach (var c in t._children)
                {
                    queue.Enqueue(c);
                }
            }
        }

        public IEnumerable<TRet> BreadthFirstVisit<TRet>(Func<IReadOnlyTree<TVal, TPr>, TRet, TRet> visitor, TRet start)
        {
            var queue = new Queue<KeyValuePair<ReadOnlyTree<TVal, TPr>, TRet>>();
            queue.Enqueue(KeyValuePair.Create(this, start));
            while (queue.Count > 0)
            {
                var tuple = queue.Dequeue();
                var res = visitor(tuple.Key, tuple.Value);
                yield return res;
                foreach (var c in tuple.Key._children)
                {
                    queue.Enqueue(KeyValuePair.Create(c, res));
                }
            }
        }

        public IEnumerable<IReadOnlyTree<TVal, TPr>> DepthFirstVisit()
        {
            var stack = new Stack<ReadOnlyTree<TVal, TPr>>();
            stack.Push(this);
            while (stack.Count > 0)
            {
                var t = stack.Pop();
                yield return t;
                foreach (var c in t._children)
                {
                    stack.Push(c);
                }
            }
        }

        public void DepthFirstVisit(Action<IReadOnlyTree<TVal, TPr>> visitor)
        {
            var stack = new Stack<ReadOnlyTree<TVal, TPr>>();
            stack.Push(this);
            while (stack.Count > 0)
            {
                var t = stack.Pop();
                visitor(t);
                foreach (var c in t._children)
                {
                    stack.Push(c);
                }
            }
        }

        public IEnumerable<TRet> DepthFirstVisit<TRet>(Func<IReadOnlyTree<TVal, TPr>, TRet, TRet> visitor, TRet start)
        {
            var stack = new Stack<KeyValuePair<ReadOnlyTree<TVal, TPr>, TRet>>();
            stack.Push(KeyValuePair.Create(this, start));
            while (stack.Count > 0)
            {
                var tuple = stack.Pop();
                var res = visitor(tuple.Key, tuple.Value);
                yield return res;
                foreach (var c in tuple.Key._children)
                {
                    stack.Push(KeyValuePair.Create(c, res));
                }
            }
        }

        public override string ToString()
        {
            return string.Format("[Value: {0}; Priority: {1}; Children: {2}]", Value, Priority, Children.Count());
        }
    }

    internal sealed class ReadOnlyTree<T> : IReadOnlyTree<T>
    {
        private readonly List<ReadOnlyTree<T>> _children = new List<ReadOnlyTree<T>>();
        private readonly T _item;
        private readonly ReadOnlyTree<T> _parent;

        public ReadOnlyTree(T item, ReadOnlyTree<T> parent)
        {
            _item = item;
            _parent = parent;
            if (parent != null)
            {
                parent._children.Add(this);
            }
        }

        public T Item
        {
            get { return _item; }
        }

        public IReadOnlyTree<T> Parent
        {
            get { return _parent; }
        }

        public IEnumerable<IReadOnlyTree<T>> Children
        {
            get { return _children; }
        }

        public IEnumerable<IReadOnlyTree<T>> BreadthFirstVisit()
        {
            var queue = new Queue<ReadOnlyTree<T>>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var t = queue.Dequeue();
                yield return t;
                foreach (var c in t._children)
                {
                    queue.Enqueue(c);
                }
            }
        }

        public void BreadthFirstVisit(Action<IReadOnlyTree<T>> visitor)
        {
            var queue = new Queue<ReadOnlyTree<T>>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var t = queue.Dequeue();
                visitor(t);
                foreach (var c in t._children)
                {
                    queue.Enqueue(c);
                }
            }
        }

        public IEnumerable<TRet> BreadthFirstVisit<TRet>(Func<IReadOnlyTree<T>, TRet, TRet> visitor, TRet start)
        {
            var queue = new Queue<KeyValuePair<ReadOnlyTree<T>, TRet>>();
            queue.Enqueue(KeyValuePair.Create(this, start));
            while (queue.Count > 0)
            {
                var tuple = queue.Dequeue();
                var res = visitor(tuple.Key, tuple.Value);
                yield return res;
                foreach (var c in tuple.Key._children)
                {
                    queue.Enqueue(KeyValuePair.Create(c, res));
                }
            }
        }

        public IEnumerable<IReadOnlyTree<T>> DepthFirstVisit()
        {
            var stack = new Stack<ReadOnlyTree<T>>();
            stack.Push(this);
            while (stack.Count > 0)
            {
                var t = stack.Pop();
                yield return t;
                foreach (var c in t._children)
                {
                    stack.Push(c);
                }
            }
        }

        public void DepthFirstVisit(Action<IReadOnlyTree<T>> visitor)
        {
            var stack = new Stack<ReadOnlyTree<T>>();
            stack.Push(this);
            while (stack.Count > 0)
            {
                var t = stack.Pop();
                visitor(t);
                foreach (var c in t._children)
                {
                    stack.Push(c);
                }
            }
        }

        public IEnumerable<TRet> DepthFirstVisit<TRet>(Func<IReadOnlyTree<T>, TRet, TRet> visitor, TRet start)
        {
            var stack = new Stack<KeyValuePair<ReadOnlyTree<T>, TRet>>();
            stack.Push(KeyValuePair.Create(this, start));
            while (stack.Count > 0)
            {
                var tuple = stack.Pop();
                var res = visitor(tuple.Key, tuple.Value);
                yield return res;
                foreach (var c in tuple.Key._children)
                {
                    stack.Push(KeyValuePair.Create(c, res));
                }
            }
        }

        public override string ToString()
        {
            return string.Format("[Item: {0}; Children: {1}]", Item, Children.Count());
        }
    }
}