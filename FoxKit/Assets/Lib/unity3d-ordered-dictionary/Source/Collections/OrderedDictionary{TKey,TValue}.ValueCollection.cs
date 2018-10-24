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
        /// A read-only ordered collection of values from the associated <see cref="OrderedDictionary{TKey, TValue}"/> instance.
        /// </summary>
        public sealed class ValueCollection : IList<TValue>, ICollection
        {
            private readonly OrderedDictionary<TKey, TValue> dictionary;


            /// <summary>
            /// Initializes a new instance of the <see cref="OrderedDictionary{TKey, TValue}.ValueCollection"/> class.
            /// </summary>
            /// <param name="dictionary">The associated dictionary.</param>
            public ValueCollection(OrderedDictionary<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
            }


            /// <inheritdoc/>
            public int Count {
                get { return this.dictionary.values.Count; }
            }

            /// <inheritdoc/>
            bool ICollection<TValue>.IsReadOnly {
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
            /// Gets value of entry at a specific index in the ordered dictionary.
            /// </summary>
            /// <param name="index">Zero-based index of entry.</param>
            /// <returns>
            /// The <see cref="TValue"/>.
            /// </returns>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// If <paramref name="index"/> is out of range of the collection.
            /// </exception>
            public TValue this[int index] {
                get {
                    this.dictionary.CheckIndexArgument(index);

                    return this.dictionary.values[index];
                }
            }

            /// <inheritdoc/>
            TValue IList<TValue>.this[int index] {
                get { return this[index]; }
                set { ThrowReadOnlyException(); }
            }

            /// <inheritdoc/>
            public bool Contains(TValue item)
            {
                return this.dictionary.values.Contains(item);
            }

            /// <inheritdoc/>
            void ICollection.CopyTo(Array array, int index)
            {
                (this.dictionary.values as ICollection).CopyTo(array, index);
            }

            /// <inheritdoc/>
            public void CopyTo(TValue[] array, int arrayIndex)
            {
                this.dictionary.values.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// Gets an object for enumerating over the ordered collection of values.
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
            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <inheritdoc/>
            void ICollection<TValue>.Add(TValue item)
            {
                ThrowReadOnlyException();
            }

            /// <inheritdoc/>
            bool ICollection<TValue>.Remove(TValue item)
            {
                ThrowReadOnlyException();
                return false;
            }

            /// <inheritdoc/>
            void ICollection<TValue>.Clear()
            {
                ThrowReadOnlyException();
            }

            /// <inheritdoc/>
            public int IndexOf(TValue item)
            {
                return this.dictionary.values.IndexOf(item);
            }

            /// <inheritdoc/>
            void IList<TValue>.Insert(int index, TValue item)
            {
                ThrowReadOnlyException();
            }

            /// <inheritdoc/>
            void IList<TValue>.RemoveAt(int index)
            {
                ThrowReadOnlyException();
            }


            /// <summary>
            /// Enumerator for enumerating through the values of an ordered dictionary.
            /// </summary>
            public struct Enumerator : IEnumerator<TValue>
            {
                private readonly OrderedDictionary<TKey, TValue> dictionary;
                private readonly int version;

                private int index;
                private TValue current;


                /// <summary>
                /// Initializes a new instance of the <see cref="OrderedDictionary{TKey, TValue}.KeyCollection.Enumerator"/> structure.
                /// </summary>
                /// <param name="dictionary">The associated dictionary.</param>
                public Enumerator(OrderedDictionary<TKey, TValue> dictionary)
                {
                    this.dictionary = dictionary;
                    this.version = dictionary.version;

                    this.index = 0;
                    this.current = default(TValue);
                }


                /// <inheritdoc/>
                void IDisposable.Dispose()
                {
                }

                /// <inheritdoc/>
                public TValue Current {
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

                    if (this.index < this.dictionary.values.Count) {
                        this.current = this.dictionary.values[this.index];
                        ++this.index;
                        return true;
                    }
                    return false;
                }

                /// <inheritdoc/>
                public void Reset()
                {
                    this.dictionary.CheckVersion(this.version);

                    this.current = default(TValue);
                    this.index = 0;
                }
            }
        }
    }
}
