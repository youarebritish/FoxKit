// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;

namespace Rotorz.Json.Serialization
{
    /// <summary>
    /// Utility method supporting reflection when serializing or deserializing objects.
    /// </summary>
    internal static class ReflectionUtility
    {
        /// <summary>
        /// Gets a value indicating whether input type represents a numeric value.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>
        /// A value of <c>true</c> if specified type represents a numeric value.
        /// </returns>
        public static bool IsNumericType(Type type)
        {
            // Original Source:
            // http://stackoverflow.com/questions/124411/using-net-how-can-i-determine-if-a-type-is-a-numeric-valuetype#answer-5182747

            if (type == null) {
                return false;
            }

            switch (Type.GetTypeCode(type)) {
                case TypeCode.Char:
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;

                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether input type represents an integer value.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>
        /// A value of <c>true</c> if specified type represents an integer value.
        /// </returns>
        public static bool IsIntegralType(Type type)
        {
            switch (Type.GetTypeCode(type)) {
                case TypeCode.Char:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;

                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                        return IsIntegralType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether input type represents a boolean value.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>
        /// A value of <c>true</c> if specified type represents a boolean value.
        /// </returns>
        public static bool IsBooleanType(Type type)
        {
            if (type == null) {
                return false;
            }

            switch (Type.GetTypeCode(type)) {
                case TypeCode.Boolean:
                    return true;

                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                        return IsBooleanType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }

            return false;
        }
    }
}
