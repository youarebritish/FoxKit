using System;
using System.IO;
using Vector3 = UnityEngine.Vector3;
using Color = UnityEngine.Color;
using Quaternion = UnityEngine.Quaternion;
using FoxKit.Utils;
using UnityEngine;
using UnityEditor;

namespace FoxKit.GrxArray.GrxArrayTool
{
    [Serializable]
    public class ComponentSpotLight : MonoBehaviour
    {
        public uint Flags1;
        public uint LightFlags;
        public uint Flags2;
        public Vector3 ReachPoint;
        public float OuterRange;
        public float InnerRange;
        public float UmbraAngle;
        public float PenumbraAngle;
        public float AttenuationExponent;
        public float Dimmer;
        public Color Color;
        public float Temperature;
        public float ColorDeflection; // inconsistency with pointlight having it as a float makes me doubt this is colordeflection too
        public float Lumen;
        public float LightSize;
        public float ShadowUmbraAngle;
        public float ShadowPenumbraAngle;
        public float ShadowAttenuationExponent;
        public float ShadowBias;
        public float ViewBias;
        public float PowerScale;
        public float LodFarSize;
        public float LodNearSize;
        public float LodShadowDrawRate;
        public uint LodRadiusLevel;
        public uint LodFadeType;
        public ComponentLightArea LightArea;
        public ComponentIrradiationPoint IrradiationPoint;
        void DrawShape(Color colorHard)
        {
            Gizmos.color = colorHard;
            Gizmos.DrawLine(transform.position + ReachPoint + Vector3.down, transform.position + ReachPoint + Vector3.up);
            Gizmos.DrawLine(transform.position + ReachPoint + Vector3.right, transform.position + ReachPoint + Vector3.left);
            Gizmos.DrawLine(transform.position + ReachPoint + Vector3.back, transform.position + ReachPoint + Vector3.forward);
        }
        void OnDrawGizmosSelected()
        {
            DrawShape(Color.blue + new Color(0.25f, 0.25f, 0.25f, 1));
        }
        void OnDrawGizmos()
        {
            DrawShape(Color.blue);
            Handles.Label(transform.position, gameObject.name);
        }
    }

    public class LightTypeSpotLight
    {
        public ulong HashName { get; set; }
        public string StringName { get; set; }
        public uint Flags1 { get; set; } // Different in GZ
        public uint LightFlags { get; set; }
        public uint Flags2 { get; set; } // Sometimes different in GZ?
        public Vector3 Translation { get; set; }
        public Vector3 ReachPoint { get; set; }
        public Quaternion Rotation { get; set; }
        public float OuterRange { get; set; }
        public float InnerRange { get; set; }
        public float UmbraAngle { get; set; }
        public float PenumbraAngle { get; set; }
        public float AttenuationExponent { get; set; }
        public float Dimmer { get; set; }
        public Color Color { get; set; }
        public float Temperature { get; set; }
        public float ColorDeflection { get; set; }
        public float Lumen { get; set; }
        public float LightSize { get; set; }
        public float ShadowUmbraAngle { get; set; }
        public float ShadowPenumbraAngle { get; set; }
        public float ShadowAttenuationExponent { get; set; }
        public float ShadowBias { get; set; }
        public float ViewBias { get; set; }
        public float PowerScale { get; set; }
        public float LodFarSize { get; set; }
        public float LodNearSize { get; set; }
        public float LodShadowDrawRate { get; set; }
        public uint LodRadiusLevel { get; set; }
        public uint LodFadeType { get; set; }
        public ExtraTransform LightArea { get; set; }
        public ExtraTransform IrradiationPoint { get; set; }
        public void Read(BinaryReader reader)
        {
            HashName = reader.ReadUInt64(); //Doesn't look like the PathCode64 of the .fox2?
            uint offsetToString = reader.ReadUInt32();
            Flags1 = reader.ReadUInt32();
            LightFlags = reader.ReadUInt32();
            Flags2 = reader.ReadUInt32();
            uint offsetToLightArea = reader.ReadUInt32();

            Translation = FoxUtils.FoxToUnity(new FoxLib.Core.Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));

            ReachPoint = FoxUtils.FoxToUnity(new FoxLib.Core.Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));

            Rotation = FoxUtils.FoxToUnity(new FoxLib.Core.Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle())); ;
            Rotation *= Quaternion.Euler(new Vector3(90, 0, 0));

            OuterRange = Half.ToHalf(reader.ReadUInt16());
            InnerRange = Half.ToHalf(reader.ReadUInt16());
            UmbraAngle = Half.ToHalf(reader.ReadUInt16());
            PenumbraAngle = Half.ToHalf(reader.ReadUInt16());
            AttenuationExponent = Half.ToHalf(reader.ReadUInt16());
            Dimmer = Half.ToHalf(reader.ReadUInt16());

            Color = new Color(
                Half.ToHalf(reader.ReadUInt16()),
                Half.ToHalf(reader.ReadUInt16()),
                Half.ToHalf(reader.ReadUInt16()),
                Half.ToHalf(reader.ReadUInt16())
            );

            Temperature = Half.ToHalf(reader.ReadUInt16());
            ColorDeflection = Half.ToHalf(reader.ReadUInt16());
            Lumen = reader.ReadSingle();
            LightSize = Half.ToHalf(reader.ReadUInt16());
            ShadowUmbraAngle = Half.ToHalf(reader.ReadUInt16());
            ShadowPenumbraAngle = Half.ToHalf(reader.ReadUInt16());
            ShadowAttenuationExponent = Half.ToHalf(reader.ReadUInt16());
            ShadowBias = Half.ToHalf(reader.ReadUInt16());
            ViewBias = Half.ToHalf(reader.ReadUInt16());
            PowerScale = Half.ToHalf(reader.ReadUInt16());
            LodFarSize = Half.ToHalf(reader.ReadUInt16());
            LodNearSize = Half.ToHalf(reader.ReadUInt16());
            LodShadowDrawRate = Half.ToHalf(reader.ReadUInt16());
            LodRadiusLevel = reader.ReadUInt32();
            LodFadeType = reader.ReadUInt32();

            uint offsetToIrraditationTransform = reader.ReadUInt32();

            StringName = string.Empty;
            //PS3 files don't use strings for light objects (however there's no way to tell byte sex apart so tool won't parse them anyway)
            if (offsetToString > 0)
            {
                StringName = reader.ReadCString();
                if (reader.BaseStream.Position % 0x4 != 0)
                    reader.BaseStream.Position += 0x4 - reader.BaseStream.Position % 0x4;
            }

            LightArea = new ExtraTransform();
            if (offsetToLightArea > 0)
                LightArea.Read(reader);
            else
                LightArea = null;

            IrradiationPoint = new ExtraTransform();
            if (offsetToIrraditationTransform > 0)
                IrradiationPoint.Read(reader);
            else
                IrradiationPoint = null;

            Log();
        }
        public void Write(BinaryWriter writer)
        {
            int offsetToTransforms = 0x78;
            if (StringName != string.Empty)
            {
                writer.Write(HashManager.StrCode64(StringName));
                writer.Write(offsetToTransforms);
                offsetToTransforms += StringName.Length + 1;
                if (offsetToTransforms % 0x4 != 0)
                    offsetToTransforms += (0x4 - offsetToTransforms % 0x4);
            }
            else
            {
                writer.Write(HashName);
                writer.Write(0);
            }
            writer.Write(Flags1);
            writer.Write(LightFlags);
            writer.Write(Flags2);
            if (LightArea!=null)
                writer.Write(offsetToTransforms-0x10);
            else
                writer.Write(0);

            FoxLib.Core.Vector3 newTranslation = FoxUtils.UnityToFox(Translation);
            writer.Write(newTranslation.X); writer.Write(newTranslation.Y); writer.Write(newTranslation.Z);

            FoxLib.Core.Vector3 newReachPoint = FoxUtils.UnityToFox(ReachPoint);
            writer.Write(newReachPoint.X); writer.Write(newReachPoint.Y); writer.Write(newReachPoint.Z);

            FoxLib.Core.Quaternion newQuat = FoxUtils.UnityToFox(Rotation * Quaternion.Euler(new Vector3(-90, 0, 0)));
            writer.Write(newQuat.X); writer.Write(newQuat.Y); writer.Write(newQuat.Z); writer.Write(newQuat.W);

            writer.Write(Half.GetBytes((Half)OuterRange));
            writer.Write(Half.GetBytes((Half)InnerRange));
            writer.Write(Half.GetBytes((Half)UmbraAngle));
            writer.Write(Half.GetBytes((Half)PenumbraAngle));
            writer.Write(Half.GetBytes((Half)AttenuationExponent));
            writer.Write(Half.GetBytes((Half)Dimmer));

            writer.Write(Half.GetBytes(-(Half)Color.r));
            writer.Write(Half.GetBytes(-(Half)Color.g));
            writer.Write(Half.GetBytes(-(Half)Color.b));
            writer.Write(Half.GetBytes(-(Half)Color.a));

            writer.Write(Half.GetBytes((Half)Temperature));
            writer.Write(Half.GetBytes((Half)ColorDeflection));
            writer.Write(Lumen);
            writer.Write(Half.GetBytes((Half)LightSize));
            writer.Write(Half.GetBytes((Half)ShadowUmbraAngle));
            writer.Write(Half.GetBytes((Half)ShadowPenumbraAngle));
            writer.Write(Half.GetBytes((Half)ShadowAttenuationExponent));
            writer.Write(Half.GetBytes((Half)ShadowBias));
            writer.Write(Half.GetBytes((Half)ViewBias));
            writer.Write(Half.GetBytes((Half)PowerScale));
            writer.Write(Half.GetBytes((Half)LodFarSize));
            writer.Write(Half.GetBytes((Half)LodNearSize));
            writer.Write(Half.GetBytes((Half)LodShadowDrawRate));
            writer.Write(LodRadiusLevel);
            writer.Write(LodFadeType);

            if (IrradiationPoint != null)
                writer.Write((offsetToTransforms + 0x28) - 0x74);
            else
                writer.Write(0);

            if (StringName != string.Empty)
            {
                writer.WriteCString(StringName); writer.WriteZeroes(1);
                if (writer.BaseStream.Position % 0x4 != 0)
                    writer.WriteZeroes(0x4 - (int)writer.BaseStream.Position % 0x4);
            }

            if (LightArea != null)
                LightArea.Write(writer);

            if (IrradiationPoint != null)
                IrradiationPoint.Write(writer);

            Log();
        }
        public void Log()
        {
            Console.WriteLine($"Spotlight entry StrCode64={HashName} StringName='{StringName}'");
            Console.WriteLine($"    vals4_2={Flags1} LightFlags={LightFlags} vals4_4={Flags2}");
            Console.WriteLine($"    Translation X={Translation.x} Y={Translation.y} Z={Translation.z}");
            Console.WriteLine($"    ReachPoint X={ReachPoint.x} Y={ReachPoint.y} Z={ReachPoint.z}");
            Console.WriteLine($"    Rotation X={Rotation.x} Y={Rotation.y} Z={Rotation.z} W={Rotation.w}");
            Console.WriteLine($"    OuterRange={OuterRange} InnerRange={InnerRange}");
            Console.WriteLine($"    UmbraAngle={UmbraAngle} PenumbraAngle={PenumbraAngle}");
            Console.WriteLine($"    AttenuationExponent={AttenuationExponent} vals14_6={Dimmer}");
            Console.WriteLine($"    Color X={Color.r} Y={Color.g} Z={Color.b} W={Color.a}");
            Console.WriteLine($"    Temperature={Temperature} ColorDeflection={ColorDeflection} Lumen={Lumen} vals10={LightSize}");
            Console.WriteLine($"    ShadowUmbraAngle={ShadowUmbraAngle} ShadowPenumbraAngle={ShadowPenumbraAngle} ");
            Console.WriteLine($"    Dimmer={ShadowBias} ShadowBias={ViewBias} ViewBias={PowerScale}");
            Console.WriteLine($"    vals11_1={LodFarSize} vals11_2={LodNearSize} vals11_3={LodShadowDrawRate}");
            Console.WriteLine($"    LodRadiusLevel={LodRadiusLevel} vals12_2={LodFadeType} vals11_3={LodShadowDrawRate}");

            if (LightArea != null)
            {
                Console.WriteLine("        LightArea");
                LightArea.Log();
            }
            if (IrradiationPoint != null)
            {
                Console.WriteLine("        IrradiationPoint");
                IrradiationPoint.Log();
            }
        }
    }
}
