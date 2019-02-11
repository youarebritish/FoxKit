namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using FoxKit.Core;
    using OdinSerializer;

    public enum FormVariationCategory
    {
        STATIC = 0x100, //(0xFF + 0x1)#: STATIC
        BodyMesh = 0x1,   //0x1: Body meshes.
        HandMesh = 0x2,   //0x2: Hand meshes.
        LegMesh = 0x3,   //0x3: Leg meshes.
        PantsMesh = 0x4,   //0x4: Pants meshes.
        WaistBeltMesh_Pfs1_TopColorSvs2 = 0x6,   //0x6: Multiple observed uses. Waist and belt meshes on pfs, top color on svs.
        BottomColor_Svs = 0x7,   //0x7: Multiple observed uses. Bottom color on svs.
        Gloves_XOF1_HolsterHipEqpt2 = 0x8,   //0x8: Rubber gloves on the XOF soldier model. Holsters and other hip equipment.
        Stun_XOF = 0x9,   //0x9: Stun grenade on the XOF soldier model.
        Multiple0 = 0xA,   //0xA: Multiple observed uses.
        Medal_Pfs1_Goggles_Ddr2 = 0xB,   //0xB: Medal on pfs. Goggles on ddr.
        ShortSleeveShirtTexturePfs1_PF__A2 = 0xC,   //0xC: Short sleeve shirt textures on pfs, PF_A.
        ShortSleeveShirtTexturePfs1_PF__B2 = 0xD,   //0xD: Short sleeve shirt textures on pfs, PF_B.
        PantsTexture_Pfs = 0xE,   //0xE: Pants textures on pfs.
        BodyEqptTexture_Pfs = 0xF,   //0xF: Body equipment textures on pfs.
        LongSleeveShirtTexturePF__C = 0x10,  //0x10: Long sleeve PF_C shirt textures on pfs.
        BootsTexturePfs = 0x11,  //0x11: Boots textures on pfs.
        JacketTexturePfs = 0x12,  //0x12: Leather jacket textures on pfs.
        SkinTexture = 0x64,  //0x64*: Skin colour texture.
        EyeTexture = 0x6E,  //0x6E*: Eye colour texture.
        HatsBeretsPfsSvs = 0xC8,  //0xC8*: Hats and berets on pfs and svs.
        Headset_XOF1_Helmets_PfsSvsChd2 = 0xC9,  //0xC9*: Headset on the XOF soldier model. Helmets on pfs, svs and chd.
        NVG_PfsSvs = 0xCA,  //0xCA*: Night vision goggle meshes on pfs and svs.
        GasMask_PfsSvs = 0xCB,  //0xCB*: Gas masks on pfs and svs.
        ThroatParasites_Ddr = 0xCD,  //0xCD: Throat parasites on ddr.
        ChestEqpt_PfsSvs = 0xD2,  //0xD2*: Chest equipment on pfs and svs.
        SoftArmor_PfsSvs = 0xD3,  //0xD3*: Soft armor on pfs and svs.
                                  // # indicates that this is not actually an enum value.
                                  // * indicates that the enum value doesn't actually exist; we're counting the rest of the fv2 as another variable category.
    }

    [System.Serializable]
    public class FormVariationOptionSet
    {
        [NonSerialized, OdinSerialize]
        public FormVariationCategory Category; 

        [NonSerialized, OdinSerialize]
        public List<FormVariationOptionSetOption> Options = new List<FormVariationOptionSetOption>();

        /// <summary>
        /// Makes a FoxKit FormVariation from a FoxLib FormVariation.
        /// </summary>
        /// <param name="formVariation">The FoxLib FormVariation to convert.</param>
        /// <param name="nameHashManager">An StrCode32 hash manager used for hashing and unhashing names.</param>
        /// <param name="fileHashManager">An PathFileNameCode64 hash manager used for hashing and unhashing file names.</param>
        /// <returns>The FoxKit FormVariation.</returns>
        public static FormVariation MakeFoxKitFormVariationType(FoxLib.FormVariation.FormVariation formVariation, Func<uint, string> str32DictFunc, Func<ulong, string> str64DictFunc)
        {
            var newFormVariation = CreateInstance<FormVariation>();

            newFormVariation.HiddenMeshGroups = (from hiddenMeshGroup in formVariation.HiddenMeshGroups select MeshGroup.Convert(hiddenMeshGroup, str32DictFunc)).ToList();
            newFormVariation.ShownMeshGroups = (from shownMeshGroup in formVariation.ShownMeshGroups select MeshGroup.Convert(shownMeshGroup, str32DictFunc)).ToList();
            newFormVariation.TextureSwaps = (from textureSwap in formVariation.TextureSwaps select TextureSwap.Convert(textureSwap, str32DictFunc, str64DictFunc)).ToList();
            newFormVariation.BoneAttachments = (from boneAttachment in formVariation.BoneAttachments select BoneAttachment.Convert(boneAttachment, str64DictFunc)).ToList();
            newFormVariation.CNPAttachments = (from cNPAttachment in formVariation.CNPAttachments select CNPAttachment.Convert(cNPAttachment, str32DictFunc, str64DictFunc)).ToList();

            return newFormVariation;
        }

        /// <summary>
        /// Makes a FoxLib FormVariation from a FoxKit FormVariation.
        /// </summary>
        /// <param name="formVariation">The FoxKit FormVariation to convert.</param>
        /// <returns>The FoxLib FormVariation.</returns>
        public FoxLib.FormVariation.FormVariation Convert()
        {
            var hiddenMeshGroups = (from meshGroup in this.HiddenMeshGroups select (uint)meshGroup.MeshGroupName).ToArray();

            var shownMeshGroups = (from meshGroup in this.ShownMeshGroups select (uint)meshGroup.MeshGroupName).ToArray();

            var textureSwaps = (from textureSwap in this.TextureSwaps select textureSwap.Convert()).ToArray();

            var boneAttachments = (from boneAttachment in this.BoneAttachments select boneAttachment.Convert()).ToArray();

            var CNPAttachments = (from cNPAttachment in this.CNPAttachments select cNPAttachment.Convert()).ToArray();

            return new FoxLib.FormVariation.FormVariation(hiddenMeshGroups, shownMeshGroups, textureSwaps, CNPAttachments, boneAttachments);
        }
    }
}