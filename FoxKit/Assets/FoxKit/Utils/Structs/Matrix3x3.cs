// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

namespace FoxKit.Utils.Structs
{
    using System;
    using System.Runtime.InteropServices;

    using UnityEngine;
    
    // A standard 3x3 transformation matrix.
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3x3 : IEquatable<Matrix3x3>
    {
        // memory layout:
        // row no (=vertical)
        // |  0   1   2
        // ---+------------
        // 0  | m00 m10 m20
        // column no  1  | m01 m11 m21
        // (=horiz)   2  | m02 m12 m22
        
        public float m00;
        public float m10;
        public float m20;
        
        public float m01;
        public float m11;
        public float m21;
        
        public float m02;
        public float m12;
        public float m22;
        

        public Matrix3x3(Vector3 column0, Vector3 column1, Vector3 column2)
        {
            this.m00 = column0.x; this.m01 = column1.x; this.m02 = column2.x;
            this.m10 = column0.y; this.m11 = column1.y; this.m12 = column2.y;
            this.m20 = column0.z; this.m21 = column1.z; this.m22 = column2.z;
        }

        // Access element at [row, column].
        public float this[int row, int column]
        {
            get
            {
                return this[row + column * 4];
            }

            set
            {
                this[row + column * 4] = value;
            }
        }

        // Access element at sequential index (0..15 inclusive).
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return m00;
                    case 1: return m10;
                    case 2: return m20;
                    case 3: return m01;
                    case 4: return m11;
                    case 5: return m21;
                    case 6: return m02;
                    case 7: return m12;
                    case 8: return m22;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: m00 = value; break;
                    case 1: m10 = value; break;
                    case 2: m20 = value; break;
                    case 3: m01 = value; break;
                    case 4: m11 = value; break;
                    case 5: m21 = value; break;
                    case 6: m02 = value; break;
                    case 7: m12 = value; break;
                    case 8: m22 = value; break;

                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        // used to allow Matrix4x4s to be used as keys in hash tables
        public override int GetHashCode()
        {
            return GetColumn(0).GetHashCode() ^ (GetColumn(1).GetHashCode() << 2) ^ (GetColumn(2).GetHashCode() >> 2);
        }

        // also required for being able to use Matrix4x4s as keys in hash tables
        public override bool Equals(object other)
        {
            if (!(other is Matrix3x3)) return false;

            return Equals((Matrix3x3)other);
        }

        public bool Equals(Matrix3x3 other)
        {
            return GetColumn(0).Equals(other.GetColumn(0)) && GetColumn(1).Equals(other.GetColumn(1))
                   && GetColumn(2).Equals(other.GetColumn(2));
        }

        // Multiplies two matrices.
        public static Matrix3x3 operator *(Matrix3x3 lhs, Matrix3x3 rhs)
        {
            Matrix3x3 res;
            res.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20;
            res.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21;
            res.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22;

            res.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20;
            res.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21;
            res.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22;

            res.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20;
            res.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21;
            res.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22;
            
            return res;
        }

        // Transforms a [[Vector3]] by a matrix.
        public static Vector3 operator *(Matrix3x3 lhs, Vector3 vector)
        {
            var resX = lhs.m00 * vector.x + lhs.m01 * vector.y + lhs.m02 * vector.z;
            var resY = lhs.m10 * vector.x + lhs.m11 * vector.y + lhs.m12 * vector.z;
            var resZ = lhs.m20 * vector.x + lhs.m21 * vector.y + lhs.m22 * vector.z;
            return new Vector3(resX, resY, resZ);
        }

        // *undoc*
        public static bool operator ==(Matrix3x3 lhs, Matrix3x3 rhs)
        {
            // Returns false in the presence of NaN values.
            return lhs.GetColumn(0) == rhs.GetColumn(0)
                && lhs.GetColumn(1) == rhs.GetColumn(1)
                && lhs.GetColumn(2) == rhs.GetColumn(2);
        }

        // *undoc*
        public static bool operator !=(Matrix3x3 lhs, Matrix3x3 rhs)
        {
            // Returns true in the presence of NaN values.
            return !(lhs == rhs);
        }

        // Get a column of the matrix.
        public Vector3 GetColumn(int index)
        {
            switch (index)
            {
                case 0: return new Vector3(m00, m10, m20);
                case 1: return new Vector3(m01, m11, m21);
                case 2: return new Vector3(m02, m12, m22);
                default:
                    throw new IndexOutOfRangeException("Invalid column index!");
            }
        }

        // Returns a row of the matrix.
        public Vector3 GetRow(int index)
        {
            switch (index)
            {
                case 0: return new Vector4(m00, m01, m02);
                case 1: return new Vector4(m10, m11, m12);
                case 2: return new Vector4(m20, m21, m22);
                default:
                    throw new IndexOutOfRangeException("Invalid row index!");
            }
        }

        // Sets a column of the matrix.
        public void SetColumn(int index, Vector3 column)
        {
            this[0, index] = column.x;
            this[1, index] = column.y;
            this[2, index] = column.z;
        }

        // Sets a row of the matrix.
        public void SetRow(int index, Vector3 row)
        {
            this[index, 0] = row.x;
            this[index, 1] = row.y;
            this[index, 2] = row.z;
        }
        
        // Matrix4x4.zero is of questionable usefulness considering C# sets everything to 0 by default, however:
        // 1. it's consistent with other Math structs in Unity such as Vector2, Vector3 and Vector4,
        // 2. "Matrix4x4.zero" is arguably more readable than "new Matrix4x4()",
        // 3. it's already in the API ..

        // Returns a matrix with all elements set to zero (RO).
        public static Matrix3x3 zero { get; } = new Matrix3x3(new Vector3(0, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 0));

        // Returns the identity matrix (RO).
        public static Matrix3x3 identity { get; } = new Matrix3x3(
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, 1));
    }
} //namespace