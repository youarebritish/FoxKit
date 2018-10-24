// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Games.UnityEditorExtensions;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rotorz.Games.Collections
{
    /// <summary>
    /// Manages the active ordered dictionary new entry editor.
    /// </summary>
    public static class OrderedDictionaryNewEntryManager
    {
        private static IEditableOrderedDictionaryContext s_ActiveContext;
        private static EditableEntry s_NewEntry;


        /// <summary>
        /// Gets the unique identifier of the active new entry control.
        /// </summary>
        public static Guid ActiveControlID {
            get { return s_ActiveContext != null ? s_ActiveContext.ControlID : Guid.Empty; }
        }

        /// <summary>
        /// Gets the ordered dictionary list adaptor that is used to draw the 'new entry' field.
        /// </summary>
        public static IOrderedDictionaryListAdaptor NewEntryListAdaptor { get; private set; }

        /// <summary>
        /// Gets a <see cref="SerializedObject"/> for the new entry object.
        /// </summary>
        public static SerializedObject NewEntryObject { get; private set; }
        /// <summary>
        /// Gets a <see cref="SerializedProperty"/> for the key property of the new entry.
        /// </summary>
        public static SerializedProperty NewEntryKeyProperty { get; private set; }
        /// <summary>
        /// Gets a <see cref="SerializedProperty"/> for the value property of the new entry.
        /// </summary>
        public static SerializedProperty NewEntryValueProperty { get; private set; }


        /// <summary>
        /// Activates the 'new entry' editor for a specified control.
        /// </summary>
        /// <param name="context">Context of the editable entry.</param>
        public static void SetActiveNewEntry(IEditableOrderedDictionaryContext context)
        {
            if (context == null) {
                throw new ArgumentNullException("context");
            }
            if (context.ControlID == Guid.Empty) {
                throw new ArgumentException("Invalid control identifier.", "context");
            }
            if (!typeof(OrderedDictionary).IsAssignableFrom(context.OrderedDictionaryType)) {
                throw new ArgumentException("Not a valid ordered dictionary type.", "context");
            }

            if (context.ControlID == ActiveControlID) {
                return;
            }

            DiscardActiveNewEntry();

            s_NewEntry = CreateEditableEntryObject(context.OrderedDictionaryType);

            NewEntryObject = new SerializedObject(s_NewEntry);
            var dictionaryProperty = NewEntryObject.FindProperty("dictionary");
            var keysProperty = dictionaryProperty.FindPropertyRelative("keys");
            var valuesProperty = dictionaryProperty.FindPropertyRelative("values");

            // Add a single key/value entry to the editable entry.
            NewEntryObject.Update();
            keysProperty.arraySize = 1;
            valuesProperty.arraySize = 1;
            NewEntryObject.ApplyModifiedPropertiesWithoutUndo();

            // Get a `SerializedProperty` for the key and value properties.
            NewEntryKeyProperty = keysProperty.GetArrayElementAtIndex(0);
            NewEntryValueProperty = valuesProperty.GetArrayElementAtIndex(0);

            NewEntryListAdaptor = context.CreateListAdaptor(dictionaryProperty);
            s_ActiveContext = context;
        }

        /// <summary>
        /// Discard any current active editable entry.
        /// </summary>
        public static void DiscardActiveNewEntry()
        {
            if (ActiveControlID == Guid.Empty) {
                return;
            }

            s_ActiveContext = null;

            Object.DestroyImmediate(s_NewEntry);
            s_NewEntry = null;

            NewEntryObject = null;
            NewEntryKeyProperty = null;
            NewEntryValueProperty = null;
        }

        /// <summary>
        /// Resets state of the new entry input controls.
        /// </summary>
        public static void ResetActiveNewEntry()
        {
            NewEntryObject.Update();
            SerializedPropertyUtility.ResetValue(NewEntryKeyProperty);
            SerializedPropertyUtility.ResetValue(NewEntryValueProperty);
            NewEntryObject.ApplyModifiedProperties();
        }


        /// <summary>
        /// Determines whether the current new entry can be added to the specified control.
        /// </summary>
        /// <param name="controlID">Unique identifier of the specified control.</param>
        /// <returns>
        /// A value of <c>true</c> if the current new entry can be added to the specified
        /// control; otherwise, a value of <c>false</c>.
        /// </returns>
        public static bool CanAddNewEntry(Guid controlID)
        {
            if (controlID != ActiveControlID) {
                return false;
            }

            var newKeyValue = s_NewEntry.Key;

            bool isNullKey = newKeyValue == null;
            bool dictionaryAlreadyContainsKey = newKeyValue != null && s_ActiveContext.OrderedDictionary.ContainsKey(newKeyValue);

            return !dictionaryAlreadyContainsKey && !isNullKey;
        }


        private static EditableEntry CreateEditableEntryObject(Type orderedDictionaryType)
        {
            var editableEntryAttribute = GetEditableEntryAttribute(orderedDictionaryType);
            if (editableEntryAttribute == null) {
                throw new InvalidOperationException("Custom ordered dictionary type is not annotated with an `EditableEntryAttribute`.");
            }

            var editableEntry = (EditableEntry)ScriptableObject.CreateInstance(editableEntryAttribute.EditableEntryType);
            editableEntry.hideFlags = HideFlags.DontSave;
            editableEntry.Dictionary.suppressErrors = true;
            return editableEntry;
        }

        private static EditableEntryAttribute GetEditableEntryAttribute(Type orderedDictionaryType)
        {
            return orderedDictionaryType.GetCustomAttributes(typeof(EditableEntryAttribute), true)
                    .Cast<EditableEntryAttribute>()
                    .FirstOrDefault();
        }
    }
}
