// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Games.UnityEditorExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Rotorz.Games.Collections
{
    /// <summary>
    /// Inspector that is assumed by default for all <see cref="OrderedDictionary"/>
    /// subclasses but can be subclassed to override its behavior.
    /// </summary>
    /// <see cref="OrderedDictionaryListAdaptor"/>
    [CustomPropertyDrawer(typeof(OrderedDictionary), useForChildren: true)]
    public class OrderedDictionaryPropertyDrawer : PropertyDrawer, IEditableOrderedDictionaryContext
    {
        protected const float TitleHeight = 21f;
        protected const float NewInputPlaceholderHeight = 23f;
        protected const float NewInputSpacing = 2f;
        protected const float EndSpacing = 5f;

        private const float AddButtonWidth = 32f;
        private const float VerticalPadding = 8f;
        private const float HalfVerticalPadding = VerticalPadding / 2f;


        private readonly Guid controlID;
        private readonly List<GUIContent> errors = new List<GUIContent>();

        private SerializedProperty property;
        private bool deferFocusNewInput;


        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedDictionaryPropertyDrawer"/> class.
        /// </summary>
        public OrderedDictionaryPropertyDrawer()
        {
            this.controlID = Guid.NewGuid();
        }


        /// <inheritdoc/>
        public Guid ControlID {
            get { return this.controlID; }
        }

        /// <inheritdoc/>
        public OrderedDictionary OrderedDictionary { get; private set; }

        /// <inheritdoc/>
        public Type OrderedDictionaryType { get; private set; }


        /// <inheritdoc/>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            this.Initialize(property);

            this.UpdateDictionaryErrors();

            float listHeight = this.ListControl.CalculateListHeight(this.ListAdaptor);
            float newInputHeight = NewInputPlaceholderHeight;
            float errorOutputHeight = this.errors.Sum(error => EditorStyles.helpBox.CalcHeight(error, 0f));

            if (this.controlID == OrderedDictionaryNewEntryManager.ActiveControlID) {
                newInputHeight = ReorderableListGUI.CalculateListFieldHeight(OrderedDictionaryNewEntryManager.NewEntryListAdaptor, this.ListControl.Flags);
            }

            return TitleHeight + listHeight + NewInputSpacing + newInputHeight + errorOutputHeight + EndSpacing;
        }

        /// <inheritdoc/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            this.Initialize(property);

            float newInputHeight = NewInputPlaceholderHeight;
            float errorOutputHeight = this.errors.Sum(error => EditorStyles.helpBox.CalcHeight(error, 0f));

            if (this.controlID == OrderedDictionaryNewEntryManager.ActiveControlID) {
                newInputHeight = ReorderableListGUI.CalculateListFieldHeight(OrderedDictionaryNewEntryManager.NewEntryListAdaptor, this.ListControl.Flags);
            }

            Rect titlePosition = position;
            titlePosition.height = TitleHeight;
            ReorderableListGUI.Title(titlePosition, label);

            Rect listPosition = position;
            listPosition.yMin += TitleHeight - 1f;
            listPosition.height -= NewInputSpacing;
            listPosition.height -= newInputHeight;
            listPosition.height -= errorOutputHeight;
            listPosition.height -= EndSpacing;
            this.DrawDictionaryListControl(listPosition);

            Rect newInputPosition = position;
            newInputPosition.yMin = NewInputSpacing + listPosition.yMax;
            newInputPosition.height = newInputHeight;

            if (this.controlID == OrderedDictionaryNewEntryManager.ActiveControlID) {
                DrawDictionaryNewInput(newInputPosition);
            }
            else {
                int activateInputAreaControlID = EditorGUIUtility.GetControlID(FocusType.Passive);
                switch (Event.current.GetTypeForControl(activateInputAreaControlID)) {
                    case EventType.MouseDown:
                        if (Event.current.button == 0 && newInputPosition.Contains(Event.current.mousePosition)) {
                            OrderedDictionaryNewEntryManager.SetActiveNewEntry(this);
                            this.deferFocusNewInput = true;
                            Event.current.Use();
                        }
                        break;

                    case EventType.Repaint:
                        var style = new GUIStyle(GUI.skin.box);
                        style.normal.textColor = GUI.skin.button.normal.textColor;
                        style.Draw(newInputPosition, new GUIContent("« Click to add new entry »"), activateInputAreaControlID);
                        break;
                }
            }

            Rect errorOutputPosition = newInputPosition;
            foreach (var error in this.errors) {
                errorOutputPosition.yMin = errorOutputPosition.yMax;
                errorOutputPosition.height = EditorStyles.helpBox.CalcHeight(error, 0f);
                EditorGUI.HelpBox(errorOutputPosition, error.text, MessageType.Error);
            }

            EditorGUI.EndProperty();
        }


        private void Initialize(SerializedProperty property)
        {
            if (property == this.property) {
                return;
            }

            this.OrderedDictionary = this.FindTargetOrderedDictionary(property);
            this.property = property;

            this.OrderedDictionaryType = this.OrderedDictionary.GetType();

            this.InitializeListControl();
        }

        private OrderedDictionary FindTargetOrderedDictionary(SerializedProperty property)
        {
            var targetObject = property.serializedObject.targetObject;
            var propertyPath = property.propertyPath;

            var targetObjectType = targetObject.GetType();
            var targetField = GetFieldDerived(targetObjectType, propertyPath, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            return (OrderedDictionary)targetField.GetValue(targetObject);
        }

        private static FieldInfo GetFieldDerived(Type type, string name, BindingFlags bindingAttr)
        {
            while (type != null) {
                var field = type.GetField(name, bindingAttr);
                if (field != null) {
                    return field;
                }
                type = type.BaseType;
            }
            return null;
        }


        #region Dictionary List Control

        protected ReorderableListControl ListControl { get; private set; }
        protected IOrderedDictionaryListAdaptor ListAdaptor { get; private set; }


        private void InitializeListControl()
        {
            this.ListControl = this.CreateListControl();
            this.ListAdaptor = this.CreateListAdaptor(this.property);
        }

        /// <summary>
        /// Creates the <see cref="ReorderableListControl"/> that will be used to draw
        /// and manipulate the list of ordered dictionary entries.
        /// </summary>
        /// <returns>
        /// The new <see cref="ReorderableListControl"/> instance.
        /// </returns>
        protected virtual ReorderableListControl CreateListControl()
        {
            var flags =
                  ReorderableListFlags.DisableDuplicateCommand
                | ReorderableListFlags.HideAddButton
                ;
            return new ReorderableListControl(flags);
        }

        /// <inheritdoc/>
        public virtual IOrderedDictionaryListAdaptor CreateListAdaptor(SerializedProperty dictionaryProperty)
        {
            var target = this.FindTargetOrderedDictionary(dictionaryProperty);

            var keysProperty = dictionaryProperty.FindPropertyRelative("keys");
            var valuesProperty = dictionaryProperty.FindPropertyRelative("values");

            var keysAdaptor = new SerializedPropertyAdaptor(keysProperty);
            var valuesAdaptor = new SerializedPropertyAdaptor(valuesProperty);
            return new OrderedDictionaryListAdaptor(target, keysAdaptor, valuesAdaptor);
        }

        #endregion

        #region Dictionary GUI

        private static GUIStyle s_AddButtonStyle;

        private static GUIStyle AddButtonStyle {
            get {
                if (s_AddButtonStyle == null) {
                    s_AddButtonStyle = new GUIStyle(ReorderableListStyles.Instance.FooterButton2);
                    s_AddButtonStyle.fixedHeight = 0f;
                }
                return s_AddButtonStyle;
            }
        }

        /// <summary>
        /// Draws the <see cref="ReorderableListControl"/> using the <see cref="ListAdaptor"/>.
        /// </summary>
        protected virtual void DrawDictionaryListControl(Rect position)
        {
            this.ListControl.Draw(position, this.ListAdaptor);
        }

        /// <summary>
        /// Draws new input controls.
        /// </summary>
        protected virtual void DrawDictionaryNewInput(Rect position)
        {
            OrderedDictionaryNewEntryManager.NewEntryObject.Update();

            EditorGUI.BeginChangeCheck();

            var adaptor = OrderedDictionaryNewEntryManager.NewEntryListAdaptor;

            // Intercept context click before sub-controls have a chance to avoid
            // revealing undesirable commands such as item insertion/removal.
            if (Event.current.type == EventType.ContextClick && position.Contains(Event.current.mousePosition)) {
                this.OnNewInputContextClick();
                Event.current.Use();
            }

            // Background behind input controls excluding add button.
            Rect backgroundPosition = position;
            backgroundPosition.width -= AddButtonWidth + 5f;
            this.DrawAddNewBackground(backgroundPosition);

            Rect newInputPosition = position;
            newInputPosition.xMin += 24f;
            newInputPosition.x -= 12f;
            newInputPosition.y += HalfVerticalPadding;
            newInputPosition.width -= AddButtonWidth;
            newInputPosition.height -= VerticalPadding;

            adaptor.BeginGUI();
            adaptor.DrawItemBackground(newInputPosition, 0);
            GUI.SetNextControlName(this.controlID.ToString());
            adaptor.DrawItem(newInputPosition, 0);
            adaptor.EndGUI();

            Rect addButtonPosition = position;
            addButtonPosition.xMin = addButtonPosition.xMax - AddButtonWidth;
            addButtonPosition.xMax -= 1f;
            this.DrawAddNewInputButton(addButtonPosition);

            if (EditorGUI.EndChangeCheck()) {
                // This is necessary so that changes made to the 'new entry' field are
                // repainted immediately.
                EditorUtility.SetDirty(this.property.serializedObject.targetObject);
            }

            if (this.deferFocusNewInput) {
                this.deferFocusNewInput = false;
                GUI.FocusControl(this.controlID.ToString());
                EditorGUIUtility.editingTextField = true;
                EditorWindow.focusedWindow.Repaint();
            }

            OrderedDictionaryNewEntryManager.NewEntryObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draws background behind new input controls.
        /// </summary>
        /// <param name="position">Absolute position of the background.</param>
        protected virtual void DrawAddNewBackground(Rect position)
        {
            if (Event.current.type == EventType.Repaint) {
                ReorderableListStyles.Instance.Container.Draw(position, GUIContent.none, false, false, false, false);
            }
        }

        /// <summary>
        /// Draws button for adding new input.
        /// </summary>
        /// <param name="position">Absolute position of button in GUI.</param>
        protected virtual void DrawAddNewInputButton(Rect position)
        {
            EditorGUI.BeginDisabledGroup(!this.CanAddNewInput);

            var addButtonNormal = ReorderableListStyles.Skin.Icon_Add_Normal;
            var addButtonActive = ReorderableListStyles.Skin.Icon_Add_Active;
            if (ExtraEditorGUI.IconButton(position, addButtonNormal, addButtonActive, AddButtonStyle)) {
                this.OnAddNewInputButtonClick();
            }

            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// Gets a value indicating whether the user can click the add new input button.
        /// </summary>
        protected virtual bool CanAddNewInput {
            get {
                return OrderedDictionaryNewEntryManager.CanAddNewEntry(this.ControlID);
            }
        }

        /// <summary>
        /// Occurs when the add new input button is clicked.
        /// </summary>
        protected virtual void OnAddNewInputButtonClick()
        {
            this.ListAdaptor.Add(OrderedDictionaryNewEntryManager.NewEntryKeyProperty, OrderedDictionaryNewEntryManager.NewEntryValueProperty);
            OrderedDictionaryNewEntryManager.ResetActiveNewEntry();
        }

        /// <summary>
        /// Occurrs when the user context clicks on the new input controls.
        /// </summary>
        /// <remarks>
        /// <para>Intercepts context click before sub-controls have a chance to display
        /// a context menu to avoid exposing undesirable commands which could otherwise
        /// cause corruption by allowing individual keys or values to be removed.</para>
        /// </remarks>
        protected virtual void OnNewInputContextClick()
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Reset Input"), false, OrderedDictionaryNewEntryManager.ResetActiveNewEntry);
            menu.ShowAsContext();
        }

        private bool hasNullKeyErrorOnLayout;

        /// <summary>
        /// Draws error feedback relating to the ordered dictionary.
        /// </summary>
        protected virtual void UpdateDictionaryErrors()
        {
            this.errors.Clear();
            if (this.OrderedDictionary.KeysWithDuplicateValues.Any()) {
                this.errors.Add(new GUIContent("Multiple values have been assigned to the same key."));
            }

            if (Event.current.type == EventType.Layout) {
                this.hasNullKeyErrorOnLayout = this.ListAdaptor.HadNullKeyErrorOnLastRepaint;
            }
            if (this.hasNullKeyErrorOnLayout) {
                this.errors.Add(new GUIContent("One or more null keys were encountered."));
            }
        }

        #endregion
    }
}
