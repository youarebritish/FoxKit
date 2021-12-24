using BezierSolution;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FoxRailImport : MonoBehaviour
{
	[MenuItem("FoxKit/Fox Rail/Import .frld and .frl")]
	private static void OnImportRail()
	{
        bool useUntitledRailNames = false;
        var railDataPath = EditorUtility.OpenFilePanel("Import .frld (found in .fpkd, optional)", "", "frld");
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