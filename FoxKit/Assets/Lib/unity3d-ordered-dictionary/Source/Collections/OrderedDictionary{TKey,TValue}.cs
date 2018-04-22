// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rotorz.Games.Collections
{
    /// <summary>
    /// Base class for a serializable ordered dictionary asset. Custom dictionary asset
    /// classes should inherit from this class since Unity is currently unable to
    /// serialize instances of generic types.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public abstract partial class OrderedDictionary<TKey, TValue> : OrderedDictionary, ISerializationCallbackReceiver, IDictionary<TKey, TValue>, IDictionary, IList<KeyValuePair<TKey, TValue>>
    {
        [SerializeField, HideInInspector]
        private int version;
        [SerializeField]
        protected List<TKey> keys = new List<TKey>();
        [SerializeField]
        protected List<TValue> values = new List<TValue>();


        protected readonly Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
        protected readonly HashSet<object> keysWithDuplicateValues = new HashSet<object>();


        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedDictionary"/> class.
        /// </summary>
        public OrderedDictionary() : base(typeof(TKey), typeof(TValue))
        {
            this.Keys = new KeyCollection(this);
            this.Values = new ValueCollection(this);
        }


        #region Unity Serialization

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.OnAfterDeserialize();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.OnBeforeSerialize();
        }

        protected virtual void OnAfterDeserialize()
        {
            if (this.keys.Count != this.values.Count) {
                Debug.LogError("Inconsistent quantity of keys and values.");
            }

            this.dictionary.Clear();
            this.keysWithDuplicateValues.Clear();

            int count = Mathf.Min(this.keys.Count, this.values.Count);
            for (int i = 0; i < count; ++i) {
                var key = this.keys[i];

                if (key == null) {
                    if (!this.suppressErrors) {
                        // Only show these warning messages when playing.
                        Debug.LogError("Encountered invalid null key.");
                    }
                    continue;
                }

                if (!this.dictionary.ContainsKey(key)) {
                    this.dictionary.Add(key, this.values[i]);
                }
                else {
                    this.keysWithDuplicateValues.Add(key);
                }
            }

            // Only show these warning messages when playing.
            foreach (var key in this.keysWithDuplicateValues) {
                Debug.Log(string.Format("Has multiple values for the key '{0}'.", key));
            }
        }

        protected virtual void OnBeforeSerialize()
        {
        }

        #endregion

        #region Integrity Checks

        private void CheckVersion(int version)
        {
            if (version != this.version) {
                throw new InvalidOperationException("Cannot make changes to the collection whilst the collection is being enumerated.");
            }
        }

        private void CheckIndexArgument(int index)
        {
            if ((uint)index >= this.Count) {
                throw new ArgumentOutOfRangeException("index", index, null);
            }
        }

        private void CheckInsertIndexArgument(int index)
        {
            if ((uint)index > this.keys.Count) {
                throw new ArgumentOutOfRangeException("index", index, null);
            }
        }

        private void CheckKeyArgument(object key)
        {
            if (key == null) {
                throw new ArgumentNullException("key");
            }
            if (!(key is TKey)) {
                throw new ArgumentException("Incompatible type.", "key");
            }
        }

        private void CheckKeyArgument(TKey key)
        {
            if (key == null) {
                throw new ArgumentNullException("key");
            }
        }

        private void CheckValueArgument(object value)
        {
            if (value is TValue || (value == null && !typeof(TValue).IsValueType)) {
                return;
            }

            throw new ArgumentException("Incompatible type.", "value");
        }

        private void CheckUniqueKeyArgument(TKey key)
        {
            if (this.ContainsKey(key)) {
                throw new ArgumentException(string.Format("Already contains key '{0}'.", key), "key");
            }
        }

        private static void ThrowReadOnlyException()
        {
            throw new InvalidOperationException("Collection is read-only.");
        }

        #endregion

        #region Non-Applicable Interfaces

        /// <inheritdoc/>
        bool IDictionary.IsReadOnly {
            get { return false; }
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly {
            get { return false; }
        }

        /// <inheritdoc/>
        bool IDictionary.IsFixedSize {
            get { return false; }
        }

        /// <inheritdoc/>
        object ICollection.SyncRoot {
            get { return (this.dictionary as ICollection).SyncRoot; }
        }

        /// <inheritdoc/>
        bool ICollection.IsSynchronized {
            get { return (this.dictionary as ICollection).IsSynchronized; }
        }

        #endregion


        /// <inheritdoc/>
        public sealed override int Count {
            get { return this.keys.Count; }
        }


        /// <summary>
        /// Gets the read-only ordered collection of keys.
        /// </summary>
        /// <see cref="Values"/>
        public KeyCollection Keys { get; private set; }

        /// <summary>
        /// Gets the read-only ordered collection of values.
        /// </summary>
        /// <see cref="Keys"/>
        public ValueCollection Values { get; private set; }

        /// <inheritdoc/>
        ICollection<TKey> IDictionary<TKey, TValue>.Keys {
            get { return this.Keys; }
        }

        /// <inheritdoc/>
        ICollection<TValue> IDictionary<TKey, TValue>.Values {
            get { return this.Values; }
        }

        /// <inheritdoc/>
        ICollection IDictionary.Keys {
            get { return this.Keys; }
        }

        /// <inheritdoc/>
        ICollection IDictionary.Values {
            get { return this.Values; }
        }


        /// <inheritdoc/>
        public TValue this[TKey key] {
            get { return this.dictionary[key]; }
            set {
                if (this.dictionary.ContainsKey(key)) {
                    this.dictionary[key] = value;

                    int index = this.IndexOf(key);
                    if (index == -1) {
                        throw new InvalidOperationException();
                    }
                    this.values[index] = value;
                }
                else {
                    this.dictionary.Add(key, value);
                    this.keys.Add(key);
                    this.values.Add(value);
                }
            }
        }

        /// <inheritdoc/>
        object IDictionary.this[object key] {
            get {
                this.CheckKeyArgument(key);

                return this[(TKey)key];
            }
            set {
                this.CheckKeyArgument(key);
                this.CheckValueArgument(value);

                this[(TKey)key] = (TValue)value;
            }
        }

        /// <inheritdoc/>
        KeyValuePair<TKey, TValue> IList<KeyValuePair<TKey, TValue>>.this[int index] {
            get {
                return new KeyValuePair<TKey, TValue>(this.keys[index], this.values[index]);
            }
            set {
                var existingKey = this.keys[index];

                if (!EqualityComparer<TKey>.Default.Equals(existingKey, value.Key)) {
                    if (this.ContainsKey(value.Key)) {
                        throw new InvalidOperationException();
                    }

                    this.dictionary.Remove(existingKey);
                    this.dictionary[value.Key] = value.Value;

                    this.keys[index] = value.Key;
                }

                this.values[index] = value.Value;
            }
        }


        /// <inheritdoc/>
        public override IEnumerable<object> KeysWithDuplicateValues {
            get { return this.keysWithDuplicateValues; }
        }


        /// <inheritdoc/>
        protected override object GetKeyFromIndexInternal(int index)
        {
            this.CheckIndexArgument(index);

            return this.keys[index];
        }

        /// <inheritdoc/>
        protected override object GetValueFromIndexInternal(int index)
        {
            this.CheckIndexArgument(index);

            return this.values[index];
        }

        /// <inheritdoc/>
        public sealed override bool ContainsKey(object key)
        {
            this.CheckKeyArgument(key);

            if (!this.KeyType.IsAssignableFrom(key.GetType())) {
                return false;
            }

            return this.dictionary.ContainsKey((TKey)key);
        }

        /// <summary>
        /// Gets the key of the entry at the specified index.
        /// </summary>
        /// <param name="index">Zero-based index of entry in ordered dictionary.</param>
        /// <returns>
        /// The key.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If <paramref name="index"/> is out of range.
        /// </exception>
        /// <see cref="GetValueFromIndex(int)"/>
        /// <see cref="Count"/>
        new public TKey GetKeyFromIndex(int index)
        {
            this.CheckIndexArgument(index);

            return this.keys[index];
        }

        /// <summary>
        /// Gets the value of the entry at the specified index.
        /// </summary>
        /// <param name="index">Zero-based index of entry in ordered dictionary.</param>
        /// <returns>
        /// The key.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If <paramref name="index"/> is out of range.
        /// </exception>
        /// <see cref="GetKeyFromIndex(int)"/>
        /// <see cref="Count"/>
        new public TValue GetValueFromIndex(int index)
        {
            this.CheckIndexArgument(index);

            return this.values[index];
        }

        /// <inheritdoc/>
        void IList<KeyValuePair<TKey, TValue>>.Insert(int index, KeyValuePair<TKey, TValue> item)
        {
            this.Insert(index, item.Key, item.Value);
        }

        /// <summary>
        /// Insert new entry into the ordered dictionary.
        /// </summary>
        /// <param name="index">Zero-based index at which to insert the new entry.</param>
        /// <param name="key">Unique key for the new entry.</param>
        /// <param name="value">Value for the new entry.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// If <paramref name="index"/> is out of the range of the ordered dictionary.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="key"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// If dictionary already contains an entry with the specified key.
        /// </exception>
        public void Insert(int index, TKey key, TValue value)
        {
            this.CheckInsertIndexArgument(index);
            this.CheckUniqueKeyArgument(key);

            ++this.version;

            this.dictionary.Add(key, value);
            this.keys.Insert(index, key);
            this.values.Insert(index, value);
        }

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            this.Insert(Count, key, value);
        }

        /// <inheritdoc/>
        void IDictionary.Add(object key, object value)
        {
            this.CheckKeyArgument(key);
            this.CheckValueArgument(value);

            var k = (TKey)key;

            if (this.ContainsKey(k)) {
                throw new ArgumentException(string.Format("Already contains key '{0}'", k), "key");
            }

            this.Add(k, (TValue)value);
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            ++this.version;

            this.keys.Clear();
            this.values.Clear();
            this.dictionary.Clear();
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            this.CheckKeyArgument(key);

            if (this.ContainsKey(key)) {
                this.RemoveAt(this.IndexOf(key));
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        /// <inheritdoc/>
        void IDictionary.Remove(object key)
        {
            this.CheckKeyArgument(key);

            this.Remove((TKey)key);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            this.CheckIndexArgument(index);

            ++this.version;

            this.dictionary.Remove(this.keys[index]);
            this.keys.RemoveAt(index);
            this.values.RemoveAt(index);
        }

        /// <summary>
        /// Gets an object for enumerating over the ordered collection of keys and values.
        /// </summary>
        /// <returns>
        /// The new <see cref="Enumerator"/>.
        /// </returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this, false);
        }

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new Enumerator(this, true);
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
        {
            return this.dictionary.ContainsKey(key);
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            return this.TryGetValue(item.Key, out value) && EqualityComparer<TValue>.Default.Equals(item.Value, value);
        }

        /// <inheritdoc/>
        bool IDictionary.Contains(object key)
        {
            if (key == null) {
                throw new ArgumentNullException("key");
            }

            if (key is TKey) {
                return this.dictionary.ContainsKey((TKey)key);
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Determines the index of an item with a aspecific key in the <see cref="OrderedDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the entry to locate in the <see cref="OrderedDictionary{TKey, TValue}"/>.</param>
        /// <returns>
        /// The zero-based index of the entry when found; otherwise, a value of <c>-1</c>.
        /// </returns>
        public int IndexOf(TKey key)
        {
            return this.keys.IndexOf(key);
        }

        /// <inheritdoc/>
        int IList<KeyValuePair<TKey, TValue>>.IndexOf(KeyValuePair<TKey, TValue> item)
        {
            int index = this.IndexOf(item.Key);

            if (index != -1 && !EqualityComparer<TValue>.Default.Equals(item.Value, this.values[index])) {
                return -1;
            }

            return index;
        }

        /// <inheritdoc/>
        void ICollection.CopyTo(Array array, int index)
        {
            (this.dictionary as ICollection).CopyTo(array, index);
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            (this.dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
        }
    }
}
