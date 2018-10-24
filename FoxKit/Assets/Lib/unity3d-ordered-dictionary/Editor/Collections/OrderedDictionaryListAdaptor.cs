// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Games.UnityEditorExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rotorz.Games.Collections
{
    /// <summary>
    /// Ordered dictionary asset adaptor for a <see cref="ReorderableListControl"/> can
    /// be subclassed to override its behavior.
    /// </summary>
    /// <remarks>
    /// <para>The <see cref="OrderedDictionaryListAdaptor"/> class can also be subclassed
    /// allowing you to initialize a custom <see cref="OrderedDictionaryListAdaptor"/>
    /// subclass to be instantiated.</para>
    /// </remarks>
    public class OrderedDictionaryListAdaptor : IOrderedDictionaryListAdaptor, IReorderableListDropTarget
    {
        protected readonly OrderedDictionary Target;
        protected readonly SerializedPropertyAdaptor KeysPropertyAdaptor;
        protected readonly SerializedPropertyAdaptor ValuesPropertyAdaptor;


        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedDictionaryListAdaptor"/> class.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="keysPropertyAdaptor">The adaptor for the ordered dictionary's keys.</param>
        /// <param name="valuesPropertyAdaptor">The adaptor for the ordered dictionary's values.</param>
        public OrderedDictionaryListAdaptor(OrderedDictionary target, SerializedPropertyAdaptor keysPropertyAdaptor, SerializedPropertyAdaptor valuesPropertyAdaptor)
        {
            this.Target = target;
            this.KeysPropertyAdaptor = keysPropertyAdaptor;
            this.ValuesPropertyAdaptor = valuesPropertyAdaptor;
        }


        #region Manipulation

        /// <inheritdoc/>
        public virtual bool CanDrag(int index)
        {
            return this.KeysPropertyAdaptor.CanDrag(index) && this.ValuesPropertyAdaptor.CanDrag(index);
        }

        /// <inheritdoc/>
        public virtual bool CanRemove(int index)
        {
            return this.KeysPropertyAdaptor.CanRemove(index) && this.ValuesPropertyAdaptor.CanRemove(index);
        }

        /// <inheritdoc/>
        public int Count {
            get { return this.KeysPropertyAdaptor.Count; }
        }

        /// <inheritdoc/>
        public virtual void Clear()
        {
            this.KeysPropertyAdaptor.Clear();
            this.ValuesPropertyAdaptor.Clear();
        }

        /// <inheritdoc/>
        void IReorderableListAdaptor.Add()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual void Add(SerializedProperty inputKeyProperty, SerializedProperty inputValueProperty)
        {
            if (this.KeysPropertyAdaptor.ArrayProperty.arraySize != this.ValuesPropertyAdaptor.ArrayProperty.arraySize) {
                throw new InvalidOperationException("Cannot add entry because of inconsistent count of keys and values.");
            }

            int count = this.KeysPropertyAdaptor.ArrayProperty.arraySize + 1;
            this.KeysPropertyAdaptor.ArrayProperty.arraySize = count;
            this.ValuesPropertyAdaptor.ArrayProperty.arraySize = count;

            var addedKeyProperty = this.KeysPropertyAdaptor.ArrayProperty.GetArrayElementAtIndex(count - 1);
            var addedValueProperty = this.ValuesPropertyAdaptor.ArrayProperty.GetArrayElementAtIndex(count - 1);
            SerializedPropertyUtility.CopyPropertyValue(addedKeyProperty, inputKeyProperty);
            SerializedPropertyUtility.CopyPropertyValue(addedValueProperty, inputValueProperty);
        }

        /// <inheritdoc/>
        void IReorderableListAdaptor.Duplicate(int index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        void IReorderableListAdaptor.Insert(int index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual void Move(int sourceIndex, int destIndex)
        {
            this.KeysPropertyAdaptor.Move(sourceIndex, destIndex);
            this.ValuesPropertyAdaptor.Move(sourceIndex, destIndex);
        }

        /// <inheritdoc/>
        public virtual void Remove(int index)
        {
            this.KeysPropertyAdaptor.Remove(index);
            this.ValuesPropertyAdaptor.Remove(index);
        }

        #endregion

        #region Drawing

        /// <inheritdoc/>
        public bool HadNullKeyErrorOnLastRepaint { get; private set; }

        /// <inheritdoc/>
        public virtual float GetItemHeight(int index)
        {
            float keyHeight = this.KeysPropertyAdaptor.GetItemHeight(index);
            float valueHeight = this.ValuesPropertyAdaptor.GetItemHeight(index);
            return Mathf.Max(keyHeight, valueHeight);
        }

        /// <inheritdoc/>
        public virtual void BeginGUI()
        {
            if (Event.current.type == EventType.Repaint) {
                this.HadNullKeyErrorOnLastRepaint = false;
            }
        }

        /// <inheritdoc/>
        public virtual void EndGUI()
        {
        }

        /// <inheritdoc/>
        public virtual void DrawItemBackground(Rect position, int index)
        {
        }

        /// <inheritdoc/>
        public virtual void DrawItem(Rect position, int index)
        {
            // Intercept context click before sub-controls have a chance to avoid
            // revealing undesirable commands such as item insertion/removal.
            if (Event.current.type == EventType.ContextClick && position.Contains(Event.current.mousePosition)) {
                this.OnContextClickItem(index);
                Event.current.Use();
            }

            Color restoreColor = GUI.color;
            if (!this.Target.suppressErrors) {
                var key = this.Target.GetKeyFromIndex(index);

                if (this.Target.KeysWithDuplicateValues.Contains(key))
                    GUI.color = Color.red;

                if (key == null) {
                    this.HadNullKeyErrorOnLastRepaint = true;
                    GUI.color = new Color(1f, 0f, 1f);
                }
            }

            Rect keyPosition = position;
            keyPosition.width /= 3f;
            keyPosition.height = this.KeysPropertyAdaptor.GetItemHeight(index);
            this.KeysPropertyAdaptor.DrawItem(keyPosition, index);

            GUI.color = restoreColor;

            Rect valuePosition = position;
            valuePosition.xMin = keyPosition.xMax + 5f;
            valuePosition.height = this.ValuesPropertyAdaptor.GetItemHeight(index);
            this.ValuesPropertyAdaptor.DrawItem(valuePosition, index);
        }

        /// <summary>
        /// Occurs allowing a list item to respond to a context click.
        /// </summary>
        /// <param name="index">Zero-based index of the list item.</param>
        protected virtual void OnContextClickItem(int index)
        {
        }

        #endregion

        #region Drop Insertion

        private Rect DropTargetPosition {
            get {
                // Expand size of drop target slightly so that it is easier to drop.
                Rect dropPosition = ReorderableListGUI.CurrentListPosition;
                dropPosition.y -= 10;
                dropPosition.height += 15;
                return dropPosition;
            }
        }

        /// <inheritdoc/>
        public bool CanDropInsert(int insertionIndex)
        {
            if (!typeof(string).IsAssignableFrom(this.Target.KeyType))
                return false;
            if (!typeof(Object).IsAssignableFrom(this.Target.ValueType))
                return false;
            if (!this.DropTargetPosition.Contains(Event.current.mousePosition))
                return false;

            var valueType = this.Target.ValueType;

            foreach (var obj in DragAndDrop.objectReferences) {
                if (!EditorUtility.IsPersistent(obj)) {
                    continue;
                }

                if (valueType.IsAssignableFrom(obj.GetType())) {
                    return true;
                }
                else {
                    string assetPath = AssetDatabase.GetAssetPath(obj);
                    if (AssetDatabase.LoadAllAssetsAtPath(assetPath).Any(o => valueType.IsAssignableFrom(o.GetType()))) {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public void ProcessDropInsertion(int insertionIndex)
        {
            if (Event.current.type == EventType.DragPerform) {
                foreach (var objectReference in this.GetDraggedObjectReferences()) {
                    if (this.Target.ContainsKey(objectReference.name)) {
                        continue;
                    }
                    this.InsertObjectReferenceEntry(insertionIndex++, objectReference);
                }
            }
        }

        private IEnumerable<Object> GetDraggedObjectReferences()
        {
            var objectReferences = new HashSet<Object>();
            var valueType = this.Target.ValueType;

            foreach (var obj in DragAndDrop.objectReferences) {
                if (!EditorUtility.IsPersistent(obj)) {
                    continue;
                }

                if (valueType.IsAssignableFrom(obj.GetType())) {
                    objectReferences.Add(obj);
                }
                else {
                    string assetPath = AssetDatabase.GetAssetPath(obj);
                    objectReferences.UnionWith(AssetDatabase.LoadAllAssetsAtPath(assetPath).Where(o => valueType.IsAssignableFrom(o.GetType())));
                }
            }

            return objectReferences.OrderBy(sprite => sprite.name);
        }

        private void InsertObjectReferenceEntry(int insertionIndex, Object objectReference)
        {
            this.KeysPropertyAdaptor.Insert(insertionIndex);
            this.ValuesPropertyAdaptor.Insert(insertionIndex);

            var keyProperty = this.KeysPropertyAdaptor.ArrayProperty.GetArrayElementAtIndex(insertionIndex);
            var valueProperty = this.ValuesPropertyAdaptor.ArrayProperty.GetArrayElementAtIndex(insertionIndex);

            keyProperty.stringValue = objectReference.name;
            valueProperty.objectReferenceValue = objectReference;
        }

        #endregion
    }
}
