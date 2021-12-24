using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FoxKit.GrxArray.GrxArrayTool
{
    [Serializable]
    public class ComponentOccluderArray : MonoBehaviour
    {
        public ulong DataSetNameHash;
        public string DataSetPath;
        public List<ComponentOccluderEntry> Occluders = new List<ComponentOccluderEntry>();
    }
    [Serializable]
    public class ComponentLightArray : MonoBehaviour
    {
        public ulong DataSetNameHash;
        public string DataSetPath;
        public List<ComponentPointLight> PointLights = new List<ComponentPointLight>();
        public List<ComponentSpotLight> SpotLights = new List<ComponentSpotLight>();
    }
    [Serializable]
    public class ComponentLightProbeArray : MonoBehaviour
    {
        public ulong DataSetNameHash;
        public string DataSetPath;
        public List<ComponentLightProbe> LightProbes = new List<ComponentLightProbe>();
    }
    public class ComponentLightArea : MonoBehaviour
    {
        void DrawShape(Color colorHard, Color colorSoft)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = colorHard;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.color = colorSoft;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Handles.Label(transform.position, gameObject.name);
        }
        void OnDrawGizmosSelected()
        {
            DrawShape(Color.yellow + new Color(0.25f, 0.25f, 0.25f, 0.5f), new Color(1, 1, 0.5F, 0.25f));
        }
        void OnDrawGizmos()
        {
            DrawShape(Color.yellow, new Color(1, 1, 0, 0.1f));
        }
    }
    public class ComponentIrradiationPoint : MonoBehaviour
    {
        void DrawShape(Color colorHard)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = colorHard;
            Gizmos.DrawLine(Vector3.down, Vector3.up);
            Gizmos.DrawLine(Vector3.right, Vector3.left);
            Gizmos.DrawLine(Vector3.back, Vector3.forward);
            Handles.Label(transform.position, gameObject.name);
        }
        void OnDrawGizmosSelected()
        {
            DrawShape(Color.red + new Color(0.25f, 0.25f, 0.25f, 1));
        }
        void OnDrawGizmos()
        {
            DrawShape(Color.red);
        }
    }
    public enum FoxLightType
    {
        DataSetDefinition00 = 0x30304D43,
        NullTerminator = 0x00000000,
        DirectLight00 = 0x30304C44, //Referenced in exe, never used
        PointLight01 = 0x31304C50, //PL01 and SL01 refereced in exe but never used, GZ uses 02, TPP uses 03, seemingly no difference
        PointLight02 = 0x32304C50,
        PointLight03 = 0x33304C50,
        SpotLight01 = 0x31304C53,
        SpotLight02 = 0x32304C53,
        SpotLight03 = 0x33304C53,
        UnknownLP00 = 0x3030504C, //"LP00" referenced in exe but never used
        LightProbe00 = 0x30305045, //"EP00" but actually the light probes
        Occluder00 = 0x3030434F, //"OC00".grxoc
    }
    public class GrxArrayFile
    {
        public ulong DataSetNameHash { get; set; }
        public string DataSetPath { get; set; }
        public List<LightTypePointLight> PointLights = new List<LightTypePointLight>();
        public List<LightTypeSpotLight> SpotLights = new List<LightTypeSpotLight>();
        public List<LightTypeLightProbe> LightProbes = new List<LightTypeLightProbe>();
        public List<LightTypeOccluder> Occluders = new List<LightTypeOccluder>();
        /// <summary>
        /// Reads and populates data from a binary lba file.
        /// </summary>
        public void Read(BinaryReader reader)
        {
            // Read header
            uint signature = reader.ReadUInt32(); //FGxL or FGxO
            if (signature!=1282950982 && signature!=1333282630)
            {
                Console.WriteLine("Wrong signature!!! Not a FGx file?");
                throw new ArgumentOutOfRangeException();
            }
            //if (signature == 1333282630)

            reader.BaseStream.Position += 12;

            reader.ReadUInt32(); //Entry type CM00/header dataset entry
            reader.BaseStream.Position += 4; //0
            DataSetNameHash = reader.ReadUInt64(); //Doesn't look like the PathCode64 of the .fox2?
            reader.BaseStream.Position += 8; //uint offsetToArray uint n2
            DataSetPath = reader.ReadCString(); //.fox2 path
            if (reader.BaseStream.Position % 0x4 != 0)
                reader.BaseStream.Position += 0x4 - reader.BaseStream.Position % 0x4;

            Console.WriteLine("Dataset entry");
            Console.WriteLine($"    NameHash {DataSetNameHash}");
            Console.WriteLine($"    DataSetPath {DataSetPath}");

            // Read locators
            while (reader.BaseStream.Position!=reader.BaseStream.Length-8) //Last 8 bytes is the terminator entry
            {
                uint entryType = reader.ReadUInt32(); //Entry type
                reader.BaseStream.Position += 4; //Entry Size
                Console.WriteLine($"entryType {(FoxLightType)entryType}");
                switch (entryType)
                {
                    case (uint)FoxLightType.DataSetDefinition00:
                        throw new ArgumentOutOfRangeException(); //Should already be parsed
                    case (uint)FoxLightType.NullTerminator:
                        Console.WriteLine("Null entry type should not be parsed !!!");
                        throw new ArgumentOutOfRangeException();
                    case (uint)FoxLightType.DirectLight00:
                        Console.WriteLine("DL00 entry type is unknown !!!");
                        throw new ArgumentOutOfRangeException();
                    case (uint)FoxLightType.PointLight01: //01 is mentioned in the exe but never seen in the wild
                    case (uint)FoxLightType.PointLight02: //02 is used in GZ, in structure seems identical to 03
                    case (uint)FoxLightType.PointLight03: //03 is TPP+
                        LightTypePointLight pl = new LightTypePointLight();
                        pl.Read(reader);
                        PointLights.Add(pl);
                        break;
                    case (uint)FoxLightType.SpotLight01: //Same as PointLight
                    case (uint)FoxLightType.SpotLight02:
                    case (uint)FoxLightType.SpotLight03:
                        LightTypeSpotLight sl = new LightTypeSpotLight();
                        sl.Read(reader);
                        SpotLights.Add(sl);
                        break;
                    case (uint)FoxLightType.UnknownLP00: //Mentioned in exe but never seen in the wild
                        Console.WriteLine("LP00 entry type is unknown !!!");
                        throw new ArgumentOutOfRangeException();
                    case (uint)FoxLightType.LightProbe00: 
                        LightTypeLightProbe ep = new LightTypeLightProbe();
                        ep.Read(reader);
                        LightProbes.Add(ep);
                        break;
                    case (uint)FoxLightType.Occluder00:
                        LightTypeOccluder oc = new LightTypeOccluder();
                        oc.Read(reader);
                        Occluders.Add(oc);
                        break;
                    default:
                        Console.WriteLine("Unrecognized entry type!!!");
                        throw new ArgumentOutOfRangeException();
                }
                if (entryType==(uint)FoxLightType.PointLight02||entryType==(uint)FoxLightType.SpotLight02)
                {
                    //is gz type
                }
            }
            
        }

        /// <summary>
        /// Writes data to a binary lba file.
        /// </summary>
        public void Write(BinaryWriter writer)
        {
            if (Occluders.Count == 0)
                writer.WriteCString("FGxL");
            else
                writer.WriteCString("FGxO");
            writer.WriteZeroes(4);
            writer.Write(0x10);
            writer.Write(1);

            // write dataset entry
            writer.WriteCString("CM00");//CM00
            //calculate entry size
            int dataSetPathLength = (DataSetPath.Length + 1);
            if (dataSetPathLength % 0x4 != 0)
                dataSetPathLength += 0x4 - dataSetPathLength % 0x4; //align stream to 4 bytes based on string's length
            writer.Write(24 + dataSetPathLength); // entry size
            writer.Write(DataSetNameHash); //not actually the hash of the dataset path?
            writer.Write(8); //unknown
            writer.WriteZeroes(4);

            // write dataset string
            writer.WriteCString(DataSetPath); writer.WriteZeroes(1);
            if (writer.BaseStream.Position % 0x4 != 0)
                writer.WriteZeroes(0x4 - (int)writer.BaseStream.Position % 0x4);

            // write lights
            if (PointLights.Count + SpotLights.Count > 0)
            {
                foreach (var light in PointLights)
                {
                    writer.WriteCString("PL03");
                    int entryLength = 0x60;
                    if (light.StringName != string.Empty)
                        entryLength += light.StringName.Length + 1;
                    if (entryLength % 0x4 != 0)
                        entryLength += (0x4 - entryLength % 0x4);
                    if (light.LightArea != null)
                        entryLength += 0x28;
                    if (light.IrradiationPoint != null)
                        entryLength += 0x28;
                    writer.Write(entryLength); //entry size
                    light.Write(writer);
                }
                foreach (var light in SpotLights)
                {
                    writer.WriteCString("SL03");
                    int entryLength = 0x88;
                    if (light.StringName != string.Empty)
                        entryLength += light.StringName.Length + 1;
                    if (entryLength % 0x4 != 0)
                        entryLength += (0x4 - entryLength % 0x4);
                    if (light.LightArea != null)
                        entryLength += 0x28;
                    if (light.IrradiationPoint != null)
                        entryLength += 0x28;
                    writer.Write(entryLength); //entry size
                    light.Write(writer);
                }
            }
            else if (LightProbes.Count > 0)
                foreach (var light in LightProbes)
                {
                    writer.WriteCString("EP00");
                    int entryLength = 0x68;
                    if (light.StringName != string.Empty)
                        entryLength += light.StringName.Length + 1;
                    if (entryLength % 0x4 != 0)
                        entryLength += (0x4 - entryLength % 0x4); 
                    writer.Write(entryLength); //entry size
                    light.Write(writer);
                }
            else if (Occluders.Count > 0)
                foreach (var light in Occluders)
                {
                    writer.WriteCString("OC00");
                    writer.Write(0x1C + (light.Faces.Length * 0x8) + (light.Vertices.Length * 0x10)); //entry size
                    light.Write(writer);
                }
            //write footer terminator entry
            writer.WriteZeroes(4);
            writer.Write(8);

        }
    }
}
