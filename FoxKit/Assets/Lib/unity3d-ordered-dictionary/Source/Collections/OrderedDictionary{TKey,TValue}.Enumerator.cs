// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Rotorz.Games.Collections
{
    public partial class OrderedDictionary<TKey, TValue>
    {
        /// <summary>
        /// Enumerator for enumerating through the key/value pairs of an ordered dictionary.
        /// </summary>
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
        {
            private readonly OrderedDictionary<TKey, TValue> dictionary;
            private readonly int version;
            private readonly bool returnDictionaryEntry;

            private int index;
            private KeyValuePair<TKey, TValue> current;


            /// <summary>
            /// Initializes a new instance of the <see cref="OrderedDictionary{TKey, TValue}.Enumerator"/> structure.
            /// </summary>
            /// <param name="dictionary">The associated dictionary.</param>
            public Enumerator(OrderedDictionary<TKey, TValue> dictionary, bool returnDictionaryEntry)
            {
                this.dictionary = dictionary;
                this.version = dictionary.version;
                this.returnDictionaryEntry = returnDictionaryEntry;

                this.index = 0;
                this.current = default(KeyValuePair<TKey, TValue>);
            }


            /// <inheritdoc/>
            void IDisposable.Dispose()
            {
            }

            /// <inheritdoc/>
            public KeyValuePair<TKey, TValue> Current {
                get { return this.current; }
            }

            /// <inheritdoc/>
            object IEnumerator.Current {
                get {
                    if (this.returnDictionaryEntry) {
                        return new DictionaryEntry(this.current.Key, this.current.Value);
                    }
                    else {
                        return this.current;
                    }
                }
            }

            /// <summary>
            /// Gets the key of the current entry.
            /// </summary>
            public TKey Key {
                get { return this.current.Key; }
            }

            /// <summary>
            /// Gets the value of the current entry.
            /// </summary>
            public TValue Value {
                get { return this.current.Value; }
            }

            /// <inheritdoc/>
            DictionaryEntry IDictionaryEnumerator.Entry {
                get { return new DictionaryEntry(this.current.Key, this.current.Value); }
            }

            /// <inheritdoc/>
            object IDictionaryEnumerator.Key {
                get { return this.current.Key; }
            }

            /// <inheritdoc/>
            object IDictionaryEnumerator.Value {
                get { return this.current.Value; }
            }

            /// <inheritdoc/>
            public bool MoveNext()
            {
                this.dictionary.CheckVersion(version);

                if (this.index < this.dictionary.Count) {
                    this.current = new KeyValuePair<TKey, TValue>(this.dictionary.keys[this.index], this.dictionary.values[this.index]);
                    ++this.index;
                    return true;
                }

                this.current = default(KeyValuePair<TKey, TValue>);
                return false;
            }

            /// <inheritdoc/>
            void IEnumerator.Reset()
            {
                this.dictionary.CheckVersion(version);

                this.index = 0;
                this.current = default(KeyValuePair<TKey, TValue>);
            }
        }
    }
}
