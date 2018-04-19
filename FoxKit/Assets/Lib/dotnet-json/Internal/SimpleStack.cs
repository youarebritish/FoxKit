// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using System.Collections.Generic;

namespace Rotorz.Json.Internal
{
    internal sealed class SimpleStack<T>
    {
        private readonly List<T> stack = new List<T>();


        public int Count {
            get { return this.stack.Count; }
        }


        public void Push(T value)
        {
            this.stack.Add(value);
        }

        public T Pop()
        {
            var top = Peek();
            this.stack.RemoveAt(this.stack.Count - 1);
            return top;
        }

        public T Peek()
        {
            if (this.stack.Count == 0) {
                throw new InvalidOperationException("Stack is empty.");
            }

            return this.stack[this.stack.Count - 1];
        }
    }
}
