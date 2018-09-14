namespace FoxKit.Modules.DataSet.Editor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using FoxKit.Core;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using Sirenix.Utilities;

    using UnityEditor;

    using UnityEngine;

    [CustomEditor(typeof(DataSetAsset))]
    public class DataSetAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            var entity = FoxKitEditor.InspectedEntity ?? ((DataSetAsset)this.serializedObject.targetObject).DataSet;
            var fields = GetPropertyFields(entity);

            DrawStaticProperties(fields, entity);

            this.serializedObject.ApplyModifiedProperties();
        }

        private static void DrawStaticProperties(IEnumerable<Tuple<FieldInfo, PropertyInfoAttribute>> fields, Entity entity)
        {
            foreach (var field in fields)
            {
                if (field.Item2.Readable == PropertyExport.Never)
                {
                    continue;
                }

                DrawStaticProperty(entity, field);

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

                DrawStaticProperties(GetPropertyFields(pointedEntity), pointedEntity);
            }
        }

        private static void DrawStaticProperty(Entity entity, Tuple<FieldInfo, PropertyInfoAttribute> field)
        {
            GUI.enabled = field.Item2.Writable != PropertyExport.Never;

            var currentValue = field.Item1.GetValue(entity);
            
            if (field.Item2.Container == Core.ContainerType.StaticArray && field.Item2.ArraySize == 1)
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
                        newValue = EditorGUILayout.IntField(field.Item1.Name, (int)currentValue);
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
                        // TODO
                        newValue = EditorGUILayout.TextField(field.Item1.Name, field.Item1.GetValue(entity) as string);
                        break;
                    case Core.PropertyInfoType.EntityPtr:
                        newValue = FoxKitUiUtils.EntityPtrField(field.Item1.Name, currentValue, field.Item2.PtrType);
                        // TODO: Show nested fields
                        break;
                    case Core.PropertyInfoType.Vector3:
                        newValue = EditorGUILayout.Vector3Field(field.Item1.Name, (UnityEngine.Vector3)field.Item1.GetValue(entity));
                        break;
                    case Core.PropertyInfoType.Vector4:
                        newValue = EditorGUILayout.Vector4Field(field.Item1.Name, (UnityEngine.Vector4)field.Item1.GetValue(entity));
                        break;
                    case Core.PropertyInfoType.Quat:
                        newValue = FoxKitUiUtils.QuaternionField(field.Item1.Name, (UnityEngine.Quaternion)currentValue);
                        break;
                    case Core.PropertyInfoType.Matrix3:
                        // TODO? There are no properties of this type, I think.
                        break;
                    case Core.PropertyInfoType.Matrix4:
                        // TODO
                        break;
                    case Core.PropertyInfoType.Color:
                        newValue = EditorGUILayout.ColorField(field.Item1.Name, (Color)field.Item1.GetValue(entity));
                        break;
                    case Core.PropertyInfoType.FilePtr:
                        newValue = EditorGUILayout.ObjectField(field.Item1.Name, field.Item1.GetValue(entity) as UnityEngine.Object, typeof(UnityEngine.Object), false);
                        break;
                    case Core.PropertyInfoType.EntityHandle:
                        newValue = FoxKitUiUtils.EntityHandleField(field.Item1.Name, currentValue, typeof(Entity));
                        break;
                    case Core.PropertyInfoType.EntityLink:
                        // TODO
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                field.Item1.SetValue(entity, newValue);
            }

            // TODO Handle arrays
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
    }
}