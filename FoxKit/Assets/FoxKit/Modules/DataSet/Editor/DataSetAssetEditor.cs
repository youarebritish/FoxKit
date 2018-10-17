namespace FoxKit.Modules.DataSet.Editor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using FoxKit.Core;
    using FoxKit.Modules.DataSet.Editor.DataListWindow;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using Rotorz.Games.Collections;

    using Sirenix.Utilities;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    [CustomEditor(typeof(DataSetAsset))]
    public class DataSetAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var asset = this.target as DataSetAsset;

            if (asset.IsReadOnly)
            {
                GUI.enabled = true;
                EditorGUILayout.HelpBox(
                    "Unity marks imported assets as read-only. To make changes to this asset, create an editable copy.",
                    MessageType.Warning);
                if (GUILayout.Button("Create Editable Copy", GUILayout.Width(200)))
                {
                    var duplicate = Instantiate(this.target) as DataSetAsset;
                    duplicate.IsReadOnly = false;

                    var path = EditorUtility.SaveFilePanelInProject(
                        "Create editable copy",
                        this.target.name,
                        "asset",
                        "Create editable copy");

                    if (!string.IsNullOrEmpty(path))
                    {
                        AssetDatabase.CreateAsset(duplicate, path);
                    }
                }

                EditorGUILayout.Separator();
                GUI.enabled = false;
            }

            var entity = FoxKitEditor.InspectedEntity
                         ?? ((DataSetAsset)this.serializedObject.targetObject).GetDataSet();

            var fields = GetPropertyFields(entity);
            DrawStaticProperties(fields, entity, asset.IsReadOnly);
            this.Repaint();

            EditorUtility.SetDirty(this.target);
        }

        private static void DrawStaticProperties(
            IEnumerable<Tuple<FieldInfo, PropertyInfoAttribute>> fields,
            Entity entity,
            bool isReadOnly)
        {
            foreach (var field in fields)
            {
                if (field.Item2.Readable == PropertyExport.Never)
                {
                    continue;
                }

                DrawStaticProperty(entity, field, isReadOnly);

                // Draw nested fields.
                // TODO: Handle array conditions.
                if (field.Item2.Type != Core.PropertyInfoType.EntityPtr)
                {
                    continue;
                }

                var pointedEntity = field.Item1.GetValue(entity) as Entity;
                if (pointedEntity == null)
                {
                    continue;
                }

                DrawStaticProperties(GetPropertyFields(pointedEntity), pointedEntity, isReadOnly);
            }
        }

        private static void DrawStaticProperty(
            Entity entity,
            Tuple<FieldInfo, PropertyInfoAttribute> field,
            bool isReadOnly)
        {
            GUI.enabled = !isReadOnly && field.Item2.Writable != PropertyExport.Never;

            var currentValue = field.Item1.GetValue(entity);

            if (field.Item2.Container == Core.ContainerType.StaticArray && field.Item2.ArraySize == 1)
            {
                var newValue = DrawField(entity, field, currentValue);
                field.Item1.SetValue(entity, newValue);
            }
            else if (field.Item2.Container == Core.ContainerType.StaticArray)
            {
                DrawListField(
                    field.Item1.Name,
                    field.Item2.Type,
                    field.Item2.Enum,
                    field.Item2.PtrType,
                    currentValue as IList,
                    false);
            }
            else if (field.Item2.Container == Core.ContainerType.DynamicArray)
            {
                DrawListField(
                    field.Item1.Name,
                    field.Item2.Type,
                    field.Item2.Enum,
                    field.Item2.PtrType,
                    currentValue as IList,
                    true);
            }
            else if (field.Item2.Container == Core.ContainerType.List)
            {
                DrawListField(
                    field.Item1.Name,
                    field.Item2.Type,
                    field.Item2.Enum,
                    field.Item2.PtrType,
                    currentValue as IList,
                    true);
            }

            // TODO Handle StringMaps
        }

        private static void DrawListField(
            string fieldName,
            Core.PropertyInfoType type,
            Type @enum,
            Type ptrType,
            IList list,
            bool isResizable)
        {
            ReorderableListGUI.Title(fieldName);

            ReorderableListFlags flags = 0;
            if (!isResizable)
            {
                flags = ReorderableListFlags.HideAddButton | ReorderableListFlags.HideRemoveButtons;
            }

            switch (type)
            {
                case Core.PropertyInfoType.Int8:
                    ReorderableListGUI.ListField(
                        list as IList<sbyte>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.UInt8:
                    ReorderableListGUI.ListField(
                        list as IList<byte>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.Int16:
                    ReorderableListGUI.ListField(
                        list as IList<short>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.UInt16:
                    ReorderableListGUI.ListField(
                        list as IList<ushort>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.Int32:
                    ReorderableListGUI.ListField(
                        list as IList<int>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.UInt32:
                    ReorderableListGUI.ListField(
                        list as IList<uint>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.Int64:
                    ReorderableListGUI.ListField(
                        list as IList<long>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.UInt64:
                    ReorderableListGUI.ListField(
                        list as IList<ulong>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.Float:
                    ReorderableListGUI.ListField(
                        list as IList<float>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.Double:
                    ReorderableListGUI.ListField(
                        list as IList<double>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.Bool:
                    ReorderableListGUI.ListField(
                        list as IList<bool>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.String:
                    ReorderableListGUI.ListField(
                        list as IList<string>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.Path:
                    ReorderableListGUI.ListField(
                        list as IList<UnityEngine.Object>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.EntityPtr:
                    var adapter = new EntityPtrListAdapter(list as IList<Entity>, ptrType);
                    ReorderableListGUI.ListField(adapter);
                    break;
                case Core.PropertyInfoType.Vector3:
                    ReorderableListGUI.ListField(
                        list as IList<UnityEngine.Vector3>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.Vector4:
                    ReorderableListGUI.ListField(
                        list as IList<UnityEngine.Vector4>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.Quat:
                    ReorderableListGUI.ListField(
                        list as IList<UnityEngine.Quaternion>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.Matrix3:
                    break;
                case Core.PropertyInfoType.Matrix4:
                    ReorderableListGUI.ListField(
                        list as IList<UnityEngine.Matrix4x4>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.Color:
                    ReorderableListGUI.ListField(
                        list as IList<Color>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.FilePtr:
                    ReorderableListGUI.ListField(
                        list as IList<UnityEngine.Object>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.EntityHandle:
                    ReorderableListGUI.ListField(
                        list as IList<Entity>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.EntityLink:
                    ReorderableListGUI.ListField(
                        list as IList<EntityLink>,
                        (position, itemValue) => DrawListItem(position, itemValue, type, @enum, ptrType),
                        DrawEmpty,
                        flags);
                    break;
                case Core.PropertyInfoType.PropertyInfo:
                    break;
                case Core.PropertyInfoType.WideVector3:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static T DrawListItem<T>(
            Rect position,
            T itemValue,
            Core.PropertyInfoType type,
            Type @enum,
            Type ptrType,
            Action<Type> createEntityCallback = null)
        {
            object newValue = null;

            switch (type)
            {
                case Core.PropertyInfoType.Int8:
                    newValue = FoxKitUiUtils.SbyteField(position, (sbyte)(object)itemValue);
                    break;
                case Core.PropertyInfoType.UInt8:
                    newValue = FoxKitUiUtils.ByteField(position, (byte)(object)itemValue);
                    break;
                case Core.PropertyInfoType.Int16:
                    newValue = FoxKitUiUtils.ShortField(position, (short)(object)itemValue);
                    break;
                case Core.PropertyInfoType.UInt16:
                    newValue = FoxKitUiUtils.UShortField(position, (ushort)(object)itemValue);
                    break;
                case Core.PropertyInfoType.Int32:
                    if (@enum != null)
                    {
                        newValue = EditorGUI.EnumPopup(position, (Enum)(object)itemValue);
                    }
                    else
                    {
                        newValue = EditorGUI.IntField(position, (int)(object)itemValue);
                    }

                    break;
                case Core.PropertyInfoType.UInt32:
                    newValue = FoxKitUiUtils.UIntField(position, (uint)(object)itemValue);
                    break;
                case Core.PropertyInfoType.Int64:
                    newValue = EditorGUI.LongField(position, (long)(object)itemValue);
                    break;
                case Core.PropertyInfoType.UInt64:
                    newValue = FoxKitUiUtils.ULongField(position, (ulong)(object)itemValue);
                    break;
                case Core.PropertyInfoType.Float:
                    newValue = EditorGUI.FloatField(position, (float)(object)itemValue);
                    break;
                case Core.PropertyInfoType.Double:
                    newValue = EditorGUI.DoubleField(position, (double)(object)itemValue);
                    break;
                case Core.PropertyInfoType.Bool:
                    newValue = EditorGUI.Toggle(position, (bool)(object)itemValue);
                    break;
                case Core.PropertyInfoType.String:
                    newValue = EditorGUI.TextField(position, (string)(object)itemValue);
                    break;
                case Core.PropertyInfoType.Path:
                    newValue = EditorGUI.ObjectField(
                        position,
                        itemValue as UnityEngine.Object,
                        typeof(UnityEngine.Object),
                        false);
                    break;
                case Core.PropertyInfoType.EntityPtr:
                    newValue = FoxKitUiUtils.EntityPtrField(
                        position,
                        itemValue,
                        ptrType,
                        () => AddEntityWindow.Create(ptrType, true, createEntityCallback));
                    break;
                case Core.PropertyInfoType.Vector3:
                    newValue = EditorGUI.Vector3Field(position, string.Empty, (UnityEngine.Vector3)(object)itemValue);
                    break;
                case Core.PropertyInfoType.Vector4:
                    newValue = EditorGUI.Vector4Field(position, string.Empty, (UnityEngine.Vector4)(object)itemValue);
                    break;
                case Core.PropertyInfoType.Quat:
                    newValue = FoxKitUiUtils.QuaternionField(position, (UnityEngine.Quaternion)(object)itemValue);
                    break;
                case Core.PropertyInfoType.Matrix3:
                    Assert.IsTrue(false, "There shouldn't be any Matrix3 properties. Report this.");
                    break;
                case Core.PropertyInfoType.Matrix4:
                    // TODO
                    Debug.LogError("Matrix4 properties not currently supported.");
                    break;
                case Core.PropertyInfoType.Color:
                    newValue = EditorGUI.ColorField(position, (Color)(object)itemValue);
                    break;
                case Core.PropertyInfoType.FilePtr:
                    newValue = EditorGUI.ObjectField(
                        position,
                        itemValue as UnityEngine.Object,
                        typeof(UnityEngine.Object),
                        false);
                    break;
                case Core.PropertyInfoType.EntityHandle:
                    newValue = FoxKitUiUtils.EntityHandleField(position, itemValue, typeof(Entity));
                    break;
                case Core.PropertyInfoType.EntityLink:
                    // TODO
                    Debug.LogError("EntityLink properties not currently supported.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return (T)newValue;
        }

        private static void DrawEmpty()
        {
            GUILayout.Label("No items in array.", EditorStyles.miniLabel);
        }

        private static object DrawField(
            Entity entity,
            Tuple<FieldInfo, PropertyInfoAttribute> field,
            object currentValue)
        {
            object newValue = null;

            switch (field.Item2.Type)
            {
                case Core.PropertyInfoType.Int8:
                    newValue = FoxKitUiUtils.SbyteField(field.Item1.Name, (sbyte)currentValue);
                    break;
                case Core.PropertyInfoType.UInt8:
                    newValue = FoxKitUiUtils.ByteField(field.Item1.Name, (byte)currentValue);
                    break;
                case Core.PropertyInfoType.Int16:
                    newValue = FoxKitUiUtils.ShortField(field.Item1.Name, (short)currentValue);
                    break;
                case Core.PropertyInfoType.UInt16:
                    newValue = FoxKitUiUtils.UShortField(field.Item1.Name, (ushort)currentValue);
                    break;
                case Core.PropertyInfoType.Int32:
                    if (field.Item2.Enum != null)
                    {
                        newValue = EditorGUILayout.EnumPopup(field.Item1.Name, (Enum)currentValue);
                    }
                    else
                    {
                        newValue = EditorGUILayout.IntField(field.Item1.Name, (int)currentValue);
                    }

                    break;
                case Core.PropertyInfoType.UInt32:
                    newValue = FoxKitUiUtils.UIntField(field.Item1.Name, (uint)currentValue);
                    break;
                case Core.PropertyInfoType.Int64:
                    newValue = EditorGUILayout.LongField(field.Item1.Name, (long)currentValue);
                    break;
                case Core.PropertyInfoType.UInt64:
                    newValue = FoxKitUiUtils.ULongField(field.Item1.Name, (ulong)currentValue);
                    break;
                case Core.PropertyInfoType.Float:
                    newValue = EditorGUILayout.FloatField(field.Item1.Name, (float)currentValue);
                    break;
                case Core.PropertyInfoType.Double:
                    newValue = EditorGUILayout.DoubleField(field.Item1.Name, (double)currentValue);
                    break;
                case Core.PropertyInfoType.Bool:
                    newValue = EditorGUILayout.Toggle(field.Item1.Name, (bool)currentValue);
                    break;
                case Core.PropertyInfoType.String:
                    newValue = EditorGUILayout.TextField(field.Item1.Name, field.Item1.GetValue(entity) as string);
                    break;
                case Core.PropertyInfoType.Path:
                    newValue = EditorGUILayout.ObjectField(
                        field.Item1.Name,
                        field.Item1.GetValue(entity) as UnityEngine.Object,
                        typeof(UnityEngine.Object),
                        false);
                    break;
                case Core.PropertyInfoType.EntityPtr:
                    newValue = FoxKitUiUtils.EntityPtrField(
                        field.Item1.Name,
                        currentValue,
                        field.Item2.PtrType,
                        () => AddEntityWindow.Create(field.Item2.PtrType, true, type => field.Item1.SetValue(entity, Activator.CreateInstance(type))));
                    break;
                case Core.PropertyInfoType.Vector3:
                    newValue = EditorGUILayout.Vector3Field(
                        field.Item1.Name,
                        (UnityEngine.Vector3)field.Item1.GetValue(entity));
                    break;
                case Core.PropertyInfoType.Vector4:
                    newValue = EditorGUILayout.Vector4Field(
                        field.Item1.Name,
                        (UnityEngine.Vector4)field.Item1.GetValue(entity));
                    break;
                case Core.PropertyInfoType.Quat:
                    newValue = FoxKitUiUtils.QuaternionField(field.Item1.Name, (UnityEngine.Quaternion)currentValue);
                    break;
                case Core.PropertyInfoType.Matrix3:
                    Assert.IsTrue(false, "There shouldn't be any Matrix3 properties. Report this.");
                    break;
                case Core.PropertyInfoType.Matrix4:
                    // TODO
                    Debug.LogError("Matrix4 properties not currently supported.");
                    break;
                case Core.PropertyInfoType.Color:
                    newValue = EditorGUILayout.ColorField(field.Item1.Name, (Color)field.Item1.GetValue(entity));
                    break;
                case Core.PropertyInfoType.FilePtr:
                    newValue = EditorGUILayout.ObjectField(
                        field.Item1.Name,
                        field.Item1.GetValue(entity) as UnityEngine.Object,
                        typeof(UnityEngine.Object),
                        false);
                    break;
                case Core.PropertyInfoType.EntityHandle:
                    newValue = FoxKitUiUtils.EntityHandleField(field.Item1.Name, currentValue, typeof(Entity));
                    break;
                case Core.PropertyInfoType.EntityLink:
                    // TODO
                    Debug.LogError("EntityLink properties not currently supported.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return newValue;
        }

        private static IEnumerable<Tuple<FieldInfo, PropertyInfoAttribute>> GetPropertyFields(Entity entity)
        {
            var baseTypes = new HashSet<Type> { entity.GetType() };
            baseTypes.AddRange(ReflectionUtils.GetParentTypes(entity.GetType()));

            return from type in baseTypes.Reverse()
                   from field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                   let attribute = field.GetCustomAttribute<PropertyInfoAttribute>()
                   where attribute != null
                   select Tuple.Create(field, attribute);
        }

        private class EntityPtrListAdapter : GenericListAdaptor<Entity>
        {
            private Type ptrType;

            private readonly IList<Entity> list;

            public EntityPtrListAdapter(
                IList<Entity> list, Type ptrType)
                : base(list, Draw, 15.0f)
            {
                this.list = list;
                this.ptrType = ptrType;
            }

            public override void DrawItem(Rect position, int index)
            {
                var item = this[index];

                var editButtonPosition = position;
                editButtonPosition.width = position.width * 0.8f;

                FoxKitUiUtils.EntityPtrField(editButtonPosition, item, this.ptrType, () => AddEntityWindow.Create(this.ptrType, true, type => this.CreateNewEntityAtIndex(type, index)));

                var wasGuiEnabled = GUI.enabled;

                if (item == null)
                {
                    GUI.enabled = false;
                }

                var deleteButtonPosition = position;
                deleteButtonPosition.x += editButtonPosition.width;
                deleteButtonPosition.width = position.width * 0.2f;
                if (GUI.Button(deleteButtonPosition, new GUIContent("Delete"), EditorStyles.miniButton))
                {
                    this.list[index] = null;
                }

                GUI.enabled = wasGuiEnabled;

                base.DrawItem(position, index);
            }

            private static Entity Draw(Rect position, Entity item)
            {
                return item;
            }

            private void CreateNewEntityAtIndex(Type entityType, int index)
            {
                this.list[index] = Activator.CreateInstance(entityType) as Entity;
            }
        }
    }
}