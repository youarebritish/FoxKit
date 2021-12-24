using System;
using System.IO;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using FoxKit.Utils;
using UnityEditor;

namespace FoxKit.GrxArray.GrxArrayTool
{
    [Serializable]
    public class ComponentLightProbe : MonoBehaviour
	{
		public uint Flags1;
		public uint LightFlags;
		public uint Flags2;
        public Vector3 InnerScalePositive;
        public Vector3 InnerScaleNegative;
		public float Exposure;
		public short Priority;
		public short ShapeType;
		public short RelatedLightIndex;
		public ushort SHDataIndex;
		public float OcclusionModeOpenRate;
		public float u5;
        void DrawCubes(Color colorOuterEdge, Color colorOuterSide, Color colorInnerEdge, Color colorInnerFace)
        {
            //Draw outer box face
            Gizmos.color = colorOuterEdge;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            //Draw outer box edge
            Gizmos.color = colorOuterSide;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);

            //Draw inner box edge
            Gizmos.color = colorInnerEdge;
            float xOffsetPos = InnerScalePositive.x / 4;
            float xOffsetNeg = InnerScaleNegative.x / 4;
            float yOffsetPos = InnerScalePositive.y / 4;
            float yOffsetNeg = InnerScaleNegative.y / 4;
            float zOffsetPos = InnerScalePositive.z / 4;
            float zOffsetNeg = InnerScaleNegative.z / 4;
            Vector3 innerCenterOffset = new Vector3(
                (xOffsetPos > xOffsetNeg ? xOffsetPos - xOffsetNeg : -(xOffsetNeg - xOffsetPos)),
                (yOffsetPos > yOffsetNeg ? yOffsetPos - yOffsetNeg : -(yOffsetNeg - yOffsetPos)),
                (zOffsetPos > zOffsetNeg ? zOffsetPos - zOffsetNeg : -(zOffsetNeg - zOffsetPos))
            );
            Gizmos.DrawWireCube(
                innerCenterOffset,
                new Vector3(
                    (InnerScalePositive.x + InnerScaleNegative.x) / 2,
                    (InnerScalePositive.y + InnerScaleNegative.y) / 2,
                    (InnerScalePositive.z + InnerScaleNegative.z) / 2
                )
            );

            //Draw inner box face
            Gizmos.color = colorInnerFace;
            Gizmos.DrawCube(
                innerCenterOffset,
                new Vector3(
                    (InnerScalePositive.x + InnerScaleNegative.x) / 2,
                    (InnerScalePositive.y + InnerScaleNegative.y) / 2,
                    (InnerScalePositive.z + InnerScaleNegative.z) / 2
                )
            );

            //Draw tesseract connection to scale
            Gizmos.color = colorInnerEdge;
            Vector3 xPositive = new Vector3(InnerScalePositive.x / 2, 0, 0);
            Vector3 yPositive = new Vector3(0, InnerScalePositive.y / 2, 0);
            Vector3 zPositive = new Vector3(0, 0, InnerScalePositive.z / 2);
            Vector3 xNegative = new Vector3(-(InnerScaleNegative.x / 2), 0, 0);
            Vector3 yNegative = new Vector3(0, -(InnerScaleNegative.y / 2), 0);
            Vector3 zNegative = new Vector3(0, 0, -(InnerScaleNegative.z / 2));
            Vector3 xPositive_yPositive_zPositive = xPositive + yPositive + zPositive;
            Vector3 xPositive_yNegative_zPositive = xPositive + yNegative + zPositive;
            Vector3 xPositive_yPositive_zNegative = xPositive + yPositive + zNegative;
            Vector3 xPositive_yNegative_zNegative = xPositive + yNegative + zNegative;
            Vector3 xNegative_yNegative_zNegative = xNegative + yNegative + zNegative;
            Vector3 xNegative_yNegative_zPositive = xNegative + yNegative + zPositive;
            Vector3 xNegative_yPositive_zPositive = xNegative + yPositive + zPositive;
            Vector3 xNegative_yPositive_zNegative = xNegative + yPositive + zNegative;
            Vector3 xPositiveOuter = new Vector3(0.5f, 0, 0);
            Vector3 yPositiveOuter = new Vector3(0, 0.5f, 0);
            Vector3 zPositiveOuter = new Vector3(0, 0, 0.5f);
            Vector3 xNegativeOuter = new Vector3(-0.5f, 0, 0);
            Vector3 yNegativeOuter = new Vector3(0, -0.5f, 0);
            Vector3 zNegativeOuter = new Vector3(0, 0, -0.5f);
            Vector3 xPositive_yPositive_zPositiveOuter = xPositiveOuter + yPositiveOuter + zPositiveOuter;
            Vector3 xPositive_yNegative_zPositiveOuter = xPositiveOuter + yNegativeOuter + zPositiveOuter;
            Vector3 xPositive_yPositive_zNegativeOuter = xPositiveOuter + yPositiveOuter + zNegativeOuter;
            Vector3 xPositive_yNegative_zNegativeOuter = xPositiveOuter + yNegativeOuter + zNegativeOuter;
            Vector3 xNegative_yNegative_zNegativeOuter = xNegativeOuter + yNegativeOuter + zNegativeOuter;
            Vector3 xNegative_yNegative_zPositiveOuter = xNegativeOuter + yNegativeOuter + zPositiveOuter;
            Vector3 xNegative_yPositive_zPositiveOuter = xNegativeOuter + yPositiveOuter + zPositiveOuter;
            Vector3 xNegative_yPositive_zNegativeOuter = xNegativeOuter + yPositiveOuter + zNegativeOuter;
            Gizmos.DrawLine(xNegative_yPositive_zNegative, xNegative_yPositive_zNegativeOuter);
            Gizmos.DrawLine(xPositive_yPositive_zNegative, xPositive_yPositive_zNegativeOuter);
            Gizmos.DrawLine(xPositive_yPositive_zPositive, xPositive_yPositive_zPositiveOuter);
            Gizmos.DrawLine(xNegative_yPositive_zPositive, xNegative_yPositive_zPositiveOuter);
            Gizmos.DrawLine(xNegative_yNegative_zNegative, xNegative_yNegative_zNegativeOuter);
            Gizmos.DrawLine(xPositive_yNegative_zNegative, xPositive_yNegative_zNegativeOuter);
            Gizmos.DrawLine(xPositive_yNegative_zPositive, xPositive_yNegative_zPositiveOuter);
            Gizmos.DrawLine(xNegative_yNegative_zPositive, xNegative_yNegative_zPositiveOuter);

        }
        void OnDrawGizmosSelected()
        {
            Color outerEdge = Color.white;
            Color outerFace = new Color(0, 1, 0, 0.5f);
            Color innerEdge = Color.white;
            Color innerFace = new Color(1, 1, 0, 0.75f);
            DrawCubes(outerEdge, outerFace, innerEdge, innerFace);
        }
        void OnDrawGizmos()
        {
            Color outerEdge = new Color(0, 1, 0, 1);
            Color outerFace = new Color(0, 1, 0, 0.5f);
            Color innerEdge = new Color(1, 1, 0, 1);
            Color innerFace = new Color(1, 1, 0, 0.75f);
            DrawCubes(outerEdge, outerFace, innerEdge, innerFace);
            Handles.Label(transform.position, gameObject.name);
        }
	}

    public class LightTypeLightProbe
    {
        public ulong HashName { get; set; }
        public uint Flags1 { get; set; }
        public uint LightFlags { get; set; } //Bitfield: 0x1 is Enable
        public uint Flags2 { get; set; }
        public float InnerScaleXPositive { get; set; } //To-do: figure out which one is positive and which is negative
        public float InnerScaleYPositive { get; set; }
        public float InnerScaleZPositive { get; set; }
        public float InnerScaleXNegative { get; set; }
        public float InnerScaleYNegative { get; set; }
        public float InnerScaleZNegative { get; set; }
        public Vector3 Scale { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Translation { get; set; }
        public float Exposure { get; set; } // translation's w?
        public short Priority { get; set; }
        public short ShapeType { get; set; } // shapeType? 0 - square, 1 - triangular prism, 2 - semi-cylindiral, 3 - half-square
        public short RelatedLightIndex { get; set; } // relatedLights index
        public ushort SHDataIndex { get; set; }
        public float OcclusionModeOpenRate { get; set; } // most time is 1
        public float u5 { get; set; } //most time is 0
        public string StringName { get; set; }
        public void Read(BinaryReader reader)
        {
            HashName = reader.ReadUInt64();
            uint offsetToString = reader.ReadUInt32();
            Flags1 = reader.ReadUInt32();
            LightFlags = reader.ReadUInt32();
            Flags2 = reader.ReadUInt32();
            InnerScaleXPositive = Half.ToHalf(reader.ReadUInt16());
            InnerScaleYPositive = Half.ToHalf(reader.ReadUInt16());
            InnerScaleZPositive = Half.ToHalf(reader.ReadUInt16());
            InnerScaleXNegative = Half.ToHalf(reader.ReadUInt16());
            InnerScaleYNegative = Half.ToHalf(reader.ReadUInt16());
            InnerScaleZNegative = Half.ToHalf(reader.ReadUInt16());

            Scale = FoxUtils.FoxToUnity(new FoxLib.Core.Vector3(-reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            Rotation = FoxUtils.FoxToUnity(new FoxLib.Core.Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            Translation = FoxUtils.FoxToUnity(new FoxLib.Core.Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));

            Exposure = reader.ReadSingle();
            Priority = reader.ReadInt16();
            ShapeType = reader.ReadInt16();
            RelatedLightIndex = reader.ReadInt16(); // related light id (TppLightProbeArray)
            SHDataIndex = reader.ReadUInt16(); // probe index used by shDatas and drawRejectionLevels (TppLightProbeArray)
            OcclusionModeOpenRate = reader.ReadSingle();
            u5 = reader.ReadSingle();

            StringName = string.Empty;
            if (offsetToString > 0)
            {
                StringName = reader.ReadCString();
                if (reader.BaseStream.Position % 0x4 != 0)
                    reader.BaseStream.Position += 0x4 - reader.BaseStream.Position % 0x4;
            }

            Log();
        }
        public void Write(BinaryWriter writer)
        {
            if (StringName != string.Empty)
            {
                writer.Write(HashManager.StrCode64(StringName));
                writer.Write(0x58);
            }
            else
            {
                writer.Write(HashName);
                writer.Write(0);
            }
            writer.Write(Flags1);
            writer.Write(LightFlags);
            writer.Write(Flags2);

            writer.Write(Half.GetBytes((Half)InnerScaleXPositive)); writer.Write(Half.GetBytes((Half)InnerScaleYPositive));
            writer.Write(Half.GetBytes((Half)InnerScaleZPositive)); writer.Write(Half.GetBytes((Half)InnerScaleXNegative));
            writer.Write(Half.GetBytes((Half)InnerScaleYNegative)); writer.Write(Half.GetBytes((Half)InnerScaleZNegative));

            FoxLib.Core.Vector3 newScale = FoxUtils.UnityToFox(Scale);
            writer.Write(-newScale.X); writer.Write(newScale.Y); writer.Write(newScale.Z);
            FoxLib.Core.Quaternion newQuat = FoxUtils.UnityToFox(Rotation);
            writer.Write(newQuat.X); writer.Write(newQuat.Y); writer.Write(newQuat.Z); writer.Write(newQuat.W);
            FoxLib.Core.Vector3 newTranslation = FoxUtils.UnityToFox(Translation);
            writer.Write(newTranslation.X); writer.Write(newTranslation.Y); writer.Write(newTranslation.Z);

            writer.Write(Exposure);
            writer.Write(Priority);
            writer.Write(ShapeType);
            writer.Write(RelatedLightIndex);
            writer.Write(SHDataIndex);
            writer.Write(OcclusionModeOpenRate);
            writer.Write(u5);
            if (StringName != string.Empty)
            {
                writer.WriteCString(StringName);
                writer.WriteZeroes(1);//null byte for readcstring
                if (writer.BaseStream.Position % 0x4 != 0)
                    writer.WriteZeroes(0x4 - (int)writer.BaseStream.Position % 0x4);
            }

            Log();
        }
        public void Log()
        {

        }
    }
}
