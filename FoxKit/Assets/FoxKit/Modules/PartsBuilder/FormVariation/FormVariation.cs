namespace FoxKit.Modules.PartsBuilder.FormVariation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using FoxKit.Core;
    using OdinSerializer;

    /// <summary>
    /// A set of form variation options used by the FoxEngine. 
    /// </summary>
    [CreateAssetMenu(fileName = "New Form Variation", menuName = "FoxKit/Form Variation", order = 2)]
    public class FormVariation : SerializedScriptableObject
    {
        [NonSerialized, OdinSerialize]
        public List<FormVariationOptionSet> Types = new List<FormVariationOptionSet>();

        [HideInInspector]
        public bool IsReadOnly;

        /// <summary>
        /// Makes a FoxKit FormVariation from a FoxLib FormVariation.
        /// </summary>
        /// <param name="formVariation">The FoxLib FormVariation to convert.</param>
        /// <param name="nameHashManager">An StrCode32 hash manager used for hashing and unhashing names.</param>
        /// <param name="fileHashManager">An PathFileNameCode64 hash manager used for hashing and unhashing file names.</param>
        /// <returns>The FoxKit FormVariation.</returns>
        public static FormVariation MakeFoxKitFormVariation(FoxLib.FormVariation.FormVariation formVariation, Func<uint, string> str32DictFunc, Func<ulong, string> str64DictFunc)
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