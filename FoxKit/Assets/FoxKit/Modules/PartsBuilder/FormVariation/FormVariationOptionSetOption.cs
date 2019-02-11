namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using FoxKit.Core;
    using OdinSerializer;

    [System.Serializable]
    public class FormVariationOptionSetOption
    {
        [NonSerialized, OdinSerialize]
        public List<MeshGroup> HiddenMeshGroups = new List<MeshGroup>();

        [NonSerialized, OdinSerialize]
        public List<MeshGroup> ShownMeshGroups = new List<MeshGroup>();

        [NonSerialized, OdinSerialize]
        public List<TextureSwap> TextureSwaps = new List<TextureSwap>();

        [NonSerialized, OdinSerialize]
        public List<BoneAttachment> BoneAttachments = new List<BoneAttachment>();

        [NonSerialized, OdinSerialize]
        public List<CNPAttachment> CNPAttachments = new List<CNPAttachment>();

        /// <summary>
        /// Makes a FoxKit FormVariation from a FoxLib FormVariation.
        /// </summary>
        /// <param name="formVariation">The FoxLib FormVariation to convert.</param>
        /// <param name="nameHashManager">An StrCode32 hash manager used for hashing and unhashing names.</param>
        /// <param name="fileHashManager">An PathFileNameCode64 hash manager used for hashing and unhashing file names.</param>
        /// <returns>The FoxKit FormVariation.</returns>
        public static FormVariationOptionSetOption MakeFoxKitFormVariationOptionSetOption(FoxLib.FormVariation.VariableFormVariationEntryOption entryOption, Func<uint, string> str32DictFunc, Func<ulong, string> str64DictFunc)
        {
            var option = new FormVariationOptionSetOption
            {
                ShownMeshGroups = (from shownMeshGroup in entryOption.Meshes select MeshGroup.Convert(shownMeshGroup, str32DictFunc)).ToList(),
                TextureSwaps = (from textureSwap in entryOption.MaterialInstances select TextureSwap.Convert(textureSwap, str32DictFunc, str64DictFunc)).ToList(),
                BoneAttachments = (from boneAttachment in entryOption.BoneAttachments select BoneAttachment.Convert(boneAttachment, str64DictFunc)).ToList(),
                CNPAttachments = (from cNPAttachment in entryOption.CNPAttachments select CNPAttachment.Convert(cNPAttachment, str32DictFunc, str64DictFunc)).ToList()
            };

            return option;
        }

        /// <summary>
        /// Makes a FoxKit FormVariationOptionSetOption from a FoxLib FormVariation. This is used for the manual "type".
        /// </summary>
        /// <param name="formVariation">The FoxLib FormVariation to convert.</param>
        /// <param name="str32DictFunc">An StrCode32 dictionary funtion used unhashing names.</param>
        /// <param name="str64DictFunc">An PathFileNameCode64 dictionary funtion used unhashing file names.</param>
        /// <returns>The FoxKit FormVariationOptionSetOption.</returns>
        public static FormVariationOptionSetOption MakeFoxKitFormVariationOptionSetOption(FoxLib.FormVariation.FormVariation formVariation, Func<uint, string> str32DictFunc, Func<ulong, string> str64DictFunc)
        {
            return new FormVariationOptionSetOption
            {
                HiddenMeshGroups = (from shownMeshGroup in formVariation.HiddenMeshGroups select MeshGroup.Convert(shownMeshGroup, str32DictFunc)).ToList(),
                ShownMeshGroups = (from shownMeshGroup in formVariation.ShownMeshGroups select MeshGroup.Convert(shownMeshGroup, str32DictFunc)).ToList(),
                TextureSwaps = (from textureSwap in formVariation.TextureSwaps select TextureSwap.Convert(textureSwap, str32DictFunc, str64DictFunc)).ToList(),
                BoneAttachments = (from boneAttachment in formVariation.BoneAttachments select BoneAttachment.Convert(boneAttachment, str64DictFunc)).ToList(),
                CNPAttachments = (from cNPAttachment in formVariation.CNPAttachments select CNPAttachment.Convert(cNPAttachment, str32DictFunc, str64DictFunc)).ToList()
            };
        }

        /// <summary>
        /// Makes a FoxLib VariableFormVariationEntryOption (non-manual) from a FoxKit FormVariationOptionSetOption.
        /// </summary>
        /// <returns>The FoxLib VariableFormVariationEntryOption.</returns>
        public FoxLib.FormVariation.VariableFormVariationEntryOption Convert()
        {
            var shownMeshGroups = (from meshGroup in this.ShownMeshGroups select (uint)meshGroup.MeshGroupName).ToArray();

            var textureSwaps = (from textureSwap in this.TextureSwaps select textureSwap.Convert()).ToArray();

            var boneAttachments = (from boneAttachment in this.BoneAttachments select boneAttachment.Convert()).ToArray();

            var CNPAttachments = (from cNPAttachment in this.CNPAttachments select cNPAttachment.Convert()).ToArray();

            return new FoxLib.FormVariation.VariableFormVariationEntryOption(shownMeshGroups, textureSwaps, CNPAttachments, boneAttachments);
        }

        /// <summary>
        /// Makes a FoxLib FormVariation (manual) from a FoxKit FormVariationOptionSetOption.
        /// </summary>
        /// <returns>The FoxLib VariableFormVariationEntryOption.</returns>
        public FoxLib.FormVariation.FormVariation Convert(FoxLib.FormVariation.VariableFormVariationEntry[] variableEntries)
        {
            var hiddenMeshGroups = (from meshGroup in this.HiddenMeshGroups select (uint)meshGroup.MeshGroupName).ToArray();

            var shownMeshGroups = (from meshGroup in this.ShownMeshGroups select (uint)meshGroup.MeshGroupName).ToArray();

            var textureSwaps = (from textureSwap in this.TextureSwaps select textureSwap.Convert()).ToArray();

            var boneAttachments = (from boneAttachment in this.BoneAttachments select boneAttachment.Convert()).ToArray();

            var CNPAttachments = (from cNPAttachment in this.CNPAttachments select cNPAttachment.Convert()).ToArray();

            return new FoxLib.FormVariation.FormVariation(hiddenMeshGroups, shownMeshGroups, textureSwaps, CNPAttachments, boneAttachments, variableEntries);
        }
    }
}