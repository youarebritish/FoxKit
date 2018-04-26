namespace FoxKit.Modules.FormVariation
{
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// A set of form variation options used by the FoxEngine. 
    /// </summary>
    [CreateAssetMenu(fileName = "New Form Variation", menuName = "FoxKit/Form Variation")]
    public class FormVariation : ScriptableObject
    {
        public HiddenMeshGroup[] HiddenMeshGroups;
        public ShownMeshGroup[] ShownMeshGroups;
        public TextureSwap[] TextureSwaps;
        public BoneAttachment[] BoneAttachments;
        public CNPAttachment[] CNPAttachments;

        /// <summary>
        /// Makes a FoxKit FormVariation from a FoxLib FormVariation.
        /// </summary>
        /// <param name="formVariation">The created FoxKit FormVariation.</param>
        /// /// <returns>The FoxLib FormVariation.</returns>
        public static FormVariation makeFormVariation(FoxLib.FormVariation.FormVariation formVariation)
        {
            var hiddenMeshGroups = (from hiddenMeshGroup in formVariation.HiddenMeshGroups select new HiddenMeshGroup(hiddenMeshGroup)).ToArray();

            var shownMeshGroups = (from shownMeshGroup in formVariation.ShownMeshGroups select new ShownMeshGroup(shownMeshGroup)).ToArray();

            var textureSwaps = (from textureSwap in formVariation.TextureSwaps select new TextureSwap(textureSwap)).ToArray();

            var boneAttachments = (from boneAttachment in formVariation.BoneAttachments select new BoneAttachment(boneAttachment)).ToArray();

            var CNPAttachments = (from CNPAttachment in formVariation.CNPAttachments select new CNPAttachment(CNPAttachment)).ToArray();

            var newFormVariation = CreateInstance<FormVariation>();

            newFormVariation.HiddenMeshGroups = hiddenMeshGroups;
            newFormVariation.ShownMeshGroups = shownMeshGroups;
            newFormVariation.TextureSwaps = textureSwaps;
            newFormVariation.BoneAttachments = boneAttachments;
            newFormVariation.CNPAttachments = CNPAttachments;

            return newFormVariation;
        }
    }
}