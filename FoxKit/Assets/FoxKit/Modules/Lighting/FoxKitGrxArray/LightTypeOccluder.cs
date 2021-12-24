using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace FoxKit.GrxArray.GrxArrayTool
{
    [Serializable]
    public class ComponentOccluderEntry : MonoBehaviour
    {
        public uint valsOcc_1;
        public List<ComponentOccluderShape> OccluderShapes = new List<ComponentOccluderShape>();
    }
    [Serializable]
    public class ComponentOccluderShape : MonoBehaviour
    {
        public short value1;
        public short value2;
        public Vector3[] Vertices;
        void OnDrawGizmos()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = Vertices;
            mesh.uv = new Vector2[0];
            mesh.triangles = new int[0];
            mesh.RecalculateNormals();
            //Gizmos.color = Color.yellow;
            Gizmos.DrawWireMesh(mesh, Vector3.zero);//???????
        }
    }
    public class LightTypeOccluder
    {
        public uint valsOcc_1 { get; set; } // Different in GZ
        public Vector3[] Vertices { get; set; }
        public struct Face
        {
            public short value1 { get; set; }
            public short value2 { get; set; }
            public short VertexIndex { get; set; }
            public short VertexCount { get; set; }
        }
        public Face[] Faces { get; set; }
        public void Read(BinaryReader reader)
        {
            valsOcc_1 = reader.ReadUInt32();
            reader.BaseStream.Position += 4;
            uint facesCount = reader.ReadUInt32();
            reader.BaseStream.Position += 4;
            uint nodesCount = reader.ReadUInt32();
            Console.WriteLine($"Occluder entry");
            Console.WriteLine($"    valsOcc_1={valsOcc_1}");
            Console.WriteLine($"    edgesCount={facesCount}");
            Console.WriteLine($"    nodesCount={nodesCount}");
            Vertices = new Vector3[nodesCount];
            for (int i = 0; i < nodesCount; i++)
            {
                Vertices[i] = new Vector3(-reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                reader.ReadSingle();
                Console.WriteLine($"    Node#{i} X={Vertices[i].x}, Y={Vertices[i].y}, Z={Vertices[i].z}");
            }
            Faces = new Face[facesCount];
            for (int i = 0; i < facesCount; i++)
            {
                Faces[i].value1 = reader.ReadInt16();
                Faces[i].value2 = reader.ReadInt16();
                Faces[i].VertexIndex = reader.ReadInt16();
                Faces[i].VertexCount = reader.ReadInt16();
                Console.WriteLine($"    Face#{i} value1={Faces[i].value1}, value2={Faces[i].value2}, VertexIndex={Faces[i].VertexIndex}, Size={Faces[i].VertexCount}");
            }
        }
        public void Write(BinaryWriter writer)
        {
            writer.Write(valsOcc_1);
            int nodeCount = Vertices.Length;
            writer.Write(0x10 * (nodeCount + 1));
            writer.Write(Faces.Length);
            writer.Write(8);
            writer.Write(nodeCount);
            for (int i=0; i<nodeCount;i++)
            {
                writer.Write(-Vertices[i].x); writer.Write(Vertices[i].y); writer.Write(Vertices[i].z);
                writer.Write((float)1);
            }
            for (int i = 0; i < Faces.Length; i++)
            {
                writer.Write(Faces[i].value1);
                writer.Write(Faces[i].value2);
                writer.Write(Faces[i].VertexIndex);
                writer.Write(Faces[i].VertexCount);
            }
        }
    }
}
