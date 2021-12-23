using BezierSolution;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
// TODO: may not need serialized componenets here if arclength and tangentmagnitude can just be calculated.
// won't need them until the extra sections get figured out, that is

[Serializable]
public class FoxRails : MonoBehaviour
{
	[MenuItem("FoxKit/Rail/Export FoxRail")]
	private static void OnExportRail()
	{
		GameObject[] objs = Selection.gameObjects;
		if (objs.Length < 1)
			return;

		var frldPath = EditorUtility.SaveFilePanel("Export .frld Rail Data (found in .fpkd, optional)", "", "", "frld");
		if (!string.IsNullOrEmpty(frldPath))
			using (BinaryWriter writer = new BinaryWriter(new FileStream(frldPath, FileMode.Create)))
			{
				uint StrCode32(string text)
				{
					if (text == null) throw new ArgumentNullException("text");
					const ulong seed0 = 0x9ae16a3b2f90404f;
					ulong seed1 = text.Length > 0 ? (uint)((text[0]) << 16) + (uint)text.Length : 0;
					return (uint)(CityHash.CityHash.CityHash64WithSeeds(text + "\0", seed0, seed1) & 0xFFFFFFFFFFFF);
				}
				writer.Write(1279869266);
				writer.Write((ushort)2);
				writer.Write((ushort)objs.Length);
				foreach (GameObject rail in objs)
				{
					if (rail.GetComponent<BezierSpline>()!=null)
					{
						uint railId = 0;
						if (uint.TryParse(rail.name, out uint maybeHash))
							railId = maybeHash;
						else
							railId = StrCode32(rail.name);
						writer.Write(railId);
					}
				}
			}

		var frlPath = EditorUtility.SaveFilePanel("Export .frl", "", "", "frl");
		if (string.IsNullOrEmpty(frlPath))
			return;

		using (BinaryWriter writer = new BinaryWriter(new FileStream(frlPath, FileMode.Create)))
		{
			void WriteFoxVector3(Vector3 vector)
			{
				writer.Write(-vector.x);
				writer.Write(vector.y);
				writer.Write(vector.z);
			}
			void ReadFoxPaddedVector3(Vector3 vector)
			{
				writer.Write(-vector.x);
				writer.Write(vector.y);
				writer.Write(vector.z);
				writer.Write((uint)0);
			}

			writer.Write(1279869266);
			writer.Write((ushort)2);
			writer.Write((ushort)objs.Length);
			writer.Write((ulong)0);

			List<long> WriteLater_offsetToStartOfRailNodes = new List<long>();
			List<long> WriteLater_offsetToSec2 = new List<long>();
			List<long> WriteLater_offsetToSec3 = new List<long>();

			int index = 0;
			long nextRailPos = 0;
			int railCount = 0;
			foreach (GameObject rail in objs)
				if (rail.GetComponent<BezierSpline>() != null)
					railCount += 1;

			foreach (GameObject rail in objs)
			{
				if (rail.GetComponent<BezierSpline>() == null)
					return;

				BezierSpline spline = rail.GetComponent<BezierSpline>();
				writer.BaseStream.Position = 0x10 + (0x30 * index);

				Vector3 boundMin = spline[0].position;
				Vector3 boundMax = spline[0].position;

				foreach(BezierPoint seg in spline)
				{
					Vector3 pos = seg.position;
					if (pos.x > boundMin.x) //fox invert
						boundMin.x = pos.x;
					if (pos.y < boundMin.y)
						boundMin.y = pos.y;
					if (pos.z < boundMin.z)
						boundMin.z = pos.z;

					if (pos.x < boundMax.x) //fox invert
						boundMax.x = pos.x;
					if (pos.y > boundMax.y)
						boundMax.y = pos.y;
					if (pos.z > boundMax.z)
						boundMax.z = pos.z;
				}
				ReadFoxPaddedVector3(boundMin);
				ReadFoxPaddedVector3(boundMax);
				WriteLater_offsetToStartOfRailNodes.Add(writer.BaseStream.Position);
				writer.Write((uint)0);
				WriteLater_offsetToSec2.Add(writer.BaseStream.Position);
				writer.Write((uint)0);
				WriteLater_offsetToSec3.Add(writer.BaseStream.Position);
				writer.Write((uint)0);
				writer.Write((ushort)spline.Count);
				writer.Write((ushort)0); //extra sections count

				writer.BaseStream.Position = 0x10 + (0x30 * railCount);

				long startOfRail = writer.BaseStream.Position;
				if (nextRailPos > 0)
					startOfRail = nextRailPos;
				writer.BaseStream.Position = WriteLater_offsetToStartOfRailNodes[index];
				writer.Write((int)startOfRail);
				writer.BaseStream.Position = startOfRail;

				float railLength = 0.0f;

				int jndex = 0;
				foreach (BezierPoint seg in spline)
				{
					BezierPoint bezierPoint = seg.gameObject.GetComponent<BezierPoint>();
					Vector3 f_prime(Vector3 p0, Vector3 v0, Vector3 v1, Vector3 p1, float t)
					{
						return v0 + t * (6 * (p1 - p0) - 4 * v0 - 2 * v1) + t * t * (-6 * (p1 - p0) + 3 * v1 + 3 * v0);
					};
					Vector2[] coefficients = new Vector2[]
					{
					  new Vector2( 0, 0.5688889f ),
					  new Vector2( -0.5384693f, 0.47862867f ),
					  new Vector2( 0.5384693f, 0.47862867f ),
					  new Vector2( -0.90617985f, 0.23692688f ),
					  new Vector2( 0.90617985f, 0.23692688f ),
					};
					float segArcLength = 0.0f;
					BezierPoint prevPoint = bezierPoint;
					if (bezierPoint.previousPoint != null)
						prevPoint = bezierPoint.previousPoint;
					foreach (Vector2 c in coefficients)
						segArcLength += c.y * f_prime(
							prevPoint.position,
							prevPoint.followingControlPointLocalPosition * 3.0f,
							bezierPoint.followingControlPointLocalPosition * 3.0f,
							bezierPoint.position, 
							(1 + c.x) / 2
						).magnitude;
					railLength += segArcLength / 2.0f;
					float arcLength = railLength;
					WriteFoxVector3(seg.transform.position); writer.Write(arcLength);

					Vector3 tangent = bezierPoint.followingControlPointLocalPosition;
					tangent *= 3.0f;
					WriteFoxVector3(tangent); writer.Write(tangent.magnitude);
					jndex += 1;
				}
				long endOfAllRailCurves = writer.BaseStream.Position;
				nextRailPos = endOfAllRailCurves;

				jndex = 0;
				foreach (long offset in WriteLater_offsetToSec2)
                {
					writer.BaseStream.Position = offset;
					writer.Write((int)endOfAllRailCurves);
					writer.BaseStream.Position = WriteLater_offsetToSec3[jndex];
					writer.Write((int)endOfAllRailCurves);
					jndex += 1;
				}
				index = +1;
			}
		}
	}

	[MenuItem("FoxKit/Rail/Import FoxRail")]
	private static void OnImportRail()
	{
        bool useUntitledRailNames = false;
        var railDataPath = EditorUtility.OpenFilePanel("Import .frld Rail Data (found in .fpkd, optional)", "", "frld");
        if (string.IsNullOrEmpty(railDataPath))
            useUntitledRailNames = true;

        uint[] railIDs = null;
        if (!useUntitledRailNames)
        {
            using (BinaryReader reader = new BinaryReader(new FileStream(railDataPath, FileMode.Open)))
            {
                uint signature = reader.ReadUInt32();
                Debug.Assert(signature == 1279869266, "Invalid signature.");

                ushort version = reader.ReadUInt16();

                ushort railCount = reader.ReadUInt16();
                railIDs = new uint[railCount];
                for (int i = 0; i < railCount; i++)
                    railIDs[i] = reader.ReadUInt32();
            }
        }

        var railPath = EditorUtility.OpenFilePanel("Import .frl", "", "frl");
		if (string.IsNullOrEmpty(railPath))
			return;

		using (BinaryReader reader = new BinaryReader(new FileStream(railPath, FileMode.Open)))
		{
			Vector3 ReadFoxVector3()
            {
				return new Vector3(-reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			}
			Vector3 ReadFoxPaddedVector3()
			{
				var result = new Vector3(-reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
				reader.BaseStream.Position += 4;
				return result;
			}

			// Read header
			uint signature = reader.ReadUInt32(); 
			Debug.Assert(signature == 1279869266, "Invalid signature.");

			ushort version = reader.ReadUInt16();

			ushort railCount = reader.ReadUInt16();
			reader.BaseStream.Position += 8; //padding

			long resumePos = 0;
			for (int i = 0; i < railCount; i++)
			{
				reader.BaseStream.Position = 0x10 + 0x30 * i;

				Vector3 boundMin = ReadFoxPaddedVector3();
				Vector3 boundMax = ReadFoxPaddedVector3();

				uint railNodeDataOffset = reader.ReadUInt32();
				uint section2Offset = reader.ReadUInt32();
				uint section3Offset = reader.ReadUInt32();

				ushort nodeCount = reader.ReadUInt16();
				ushort section23UnknownCount = reader.ReadUInt16();

				resumePos = reader.BaseStream.Position;
				reader.BaseStream.Position = railNodeDataOffset;

				if (section23UnknownCount > 0)
					Debug.Log("Rail has unknown sections!!!");

				BezierSpline spline = new GameObject().AddComponent<BezierSpline>();

				string name = "dummy";
                if (!useUntitledRailNames)
                    name = railIDs[i].ToString();
                else
                    name = "FoxRail" + i.ToString("D4");
                spline.gameObject.name = name;
				spline.Initialize(nodeCount);

				for (int j = 0; j < nodeCount; j++)
				{
					Vector3 position = ReadFoxVector3();
					float arcLength = reader.ReadSingle(); 
					Vector3 tangent = ReadFoxVector3();
					float tangentMagnitude = reader.ReadSingle(); 

					Vector3 nextTangent = new Vector3(0, 0, 0); 
					if (j + 1 < nodeCount)
					{
						reader.BaseStream.Position += 16;
						nextTangent = ReadFoxVector3();
						reader.BaseStream.Position -= 28;
					}

					tangent /= 3.0f;
					nextTangent /= 3.0f;

					spline[j].gameObject.name = spline.gameObject.name + "_" + j.ToString("D4");
					spline[j].position = position;
					spline[j].followingControlPointLocalPosition = tangent;
					if (j + 1 < nodeCount)
						spline[j + 1].precedingControlPointLocalPosition = -nextTangent;
				}
			}
		}
	}
}