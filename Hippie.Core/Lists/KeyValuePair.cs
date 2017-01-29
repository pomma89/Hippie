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

using System.Collections.Generic;
using System.Diagnostics;

namespace PommaLabs.CodeServices.Common
{
    /// <summary>
    ///   A creation method for <see cref="KeyValuePair{TKey,TValue}"/>.
    /// </summary>
    public static class KeyValuePair
    {
        /// <summary>
        ///   Creates a new key-value pair with the specified key and value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>A new key-value pair with the specified key and value.</returns>
        public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
        {
            var result = new KeyValuePair<TKey, TValue>(key, value);

            // Postconditions
            Debug.Assert(Equals(key, result.Key));
            Debug.Assert(Equals(value, result.Value));
            return result;
        }
    }
}
