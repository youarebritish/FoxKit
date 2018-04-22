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
        /// A read-only ordered collection of keys from the associated <see cref="OrderedDictionary{TKey, TValue}"/> instance.
        /// </summary>
        public sealed class KeyCollection : IList<TKey>, ICollection
        {
            private readonly OrderedDictionary<TKey, TValue> dictionary;


            /// <summary>
            /// Initializes a new instance of the <see cref="OrderedDictionary{TKey, TValue}.KeyCollection"/> class.
            /// </summary>
            /// <param name="dictionary">The associated dictionary.</param>
            public KeyCollection(OrderedDictionary<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
            }


            /// <inheritdoc/>
            public int Count {
                get { return this.dictionary.keys.Count; }
            }

            /// <inheritdoc/>
            bool ICollection<TKey>.IsReadOnly {
                get { return true; }
            }

            /// <inheritdoc/>
            bool ICollection.IsSynchronized {
                get { return (this.dictionary.dictionary as ICollection).IsSynchronized; }
            }

            /// <inheritdoc/>
            object ICollection.SyncRoot {
                get { return (this.dictionary.dictionary as ICollection).SyncRoot; }
            }

            /// <summary>
            /// Gets key of entry at a specific index in the ordered dictionary.
            /// </summary>
            /// <param name="index">Zero-based index of entry.</param>
            /// <returns>
            /// The <see cref="TKey"/>.
            /// </returns>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// If <paramref name="index"/> is out of range of the collection.
            /// </exception>
            public TKey this[int index] {
                get {
                    this.dictionary.CheckIndexArgument(index);

                    return this.dictionary.keys[index];
                }
            }

            /// <inheritdoc/>
            TKey IList<TKey>.this[int index] {
                get { return this[index]; }
                set { ThrowReadOnlyException(); }
            }

            /// <inheritdoc/>
            public bool Contains(TKey item)
            {
                return this.dictionary.ContainsKey(item);
            }

            /// <inheritdoc/>
            void ICollection.CopyTo(Array array, int index)
            {
                (this.dictionary.keys as ICollection).CopyTo(array, index);
            }

            /// <inheritdoc/>
            public void CopyTo(TKey[] array, int arrayIndex)
            {
                this.dictionary.keys.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// Gets an object for enumerating over the ordered collection of keys.
            /// </summary>
            /// <returns>
            /// The new <see cref="Enumerator"/>.
            /// </returns>
            public Enumerator GetEnumerator()
            {
                return new Enumerator(this.dictionary);
            }

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <inheritdoc/>
            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <inheritdoc/>
            void ICollection<TKey>.Add(TKey item)
            {
                ThrowReadOnlyException();
            }

            /// <inheritdoc/>
            bool ICollection<TKey>.Remove(TKey item)
            {
                ThrowReadOnlyException();
                return false;
            }

            /// <inheritdoc/>
            void ICollection<TKey>.Clear()
            {
                ThrowReadOnlyException();
            }

            /// <inheritdoc/>
            public int IndexOf(TKey item)
            {
                return this.dictionary.keys.IndexOf(item);
            }

            /// <inheritdoc/>
            void IList<TKey>.Insert(int index, TKey item)
            {
                ThrowReadOnlyException();
            }

            /// <inheritdoc/>
            void IList<TKey>.RemoveAt(int index)
            {
                ThrowReadOnlyException();
            }


            /// <summary>
            /// Enumerator for enumerating through the keys of an ordered dictionary.
            /// </summary>
            public struct Enumerator : IEnumerator<TKey>
            {
                private readonly OrderedDictionary<TKey, TValue> dictionary;
                private readonly int version;

                private int index;
                private TKey current;

                /// <summary>
                /// Initializes a new instance of the <see cref="OrderedDictionary{TKey, TValue}.KeyCollection.Enumerator"/> structure.
                /// </summary>
                /// <param name="dictionary">The associated dictionary.</param>
                public Enumerator(OrderedDictionary<TKey, TValue> dictionary)
                {
                    this.dictionary = dictionary;
                    this.version = dictionary.version;

                    this.index = 0;
                    this.current = default(TKey);
                }

                /// <inheritdoc/>
                void IDisposable.Dispose()
                {
                }

                /// <inheritdoc/>
                public TKey Current {
                    get { return this.current; }
                }

                /// <inheritdoc/>
                object IEnumerator.Current {
                    get { return this.current; }
                }

                /// <inheritdoc/>
                public bool MoveNext()
                {
                    this.dictionary.CheckVersion(this.version);

                    if (this.index < this.dictionary.keys.Count) {
                        this.current = this.dictionary.keys[this.index];
                        ++this.index;
                        return true;
                    }
                    return false;
                }

                /// <inheritdoc/>
                public void Reset()
                {
                    this.dictionary.CheckVersion(this.version);

                    this.current = default(TKey);
                    this.index = 0;
                }
            }
        }
    }
}
