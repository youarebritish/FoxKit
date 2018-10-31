namespace FoxKit.Modules.DataSet.TppGameCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Modules.DataSet.GameCore;
    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Parameters for a <see cref="GameCore.GameObject"/> of type TppHostage2.
    /// </summary>
    public class TppHostage2Parameter : GameObjectParameter
    {
        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object partsFile;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object motionGraphFile;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object mtarFile;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object extensionMtarFile;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField]
        private OrderedDictionary_string_Object vfxFiles;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField]
        private string partsFilePath;

        /// <summary>
        /// Path to <see cref="motionGraphFile"/>.
        /// </summary>
        [SerializeField]
        private string motionGraphFilePath;

        /// <summary>
        /// Path to <see cref="mtarFilePath"/>.
        /// </summary>
        [SerializeField]
        private string mtarFilePath;

        /// <summary>
        /// Path to <see cref="extensionMtarFile"/>.
        /// </summary>
        [SerializeField]
        private string extensionMtarFilePath;

        /// <summary>
        /// Paths to <see cref="vfxFiles"/>.
        /// </summary>
        [SerializeField]
        private OrderedDictionary_string_string vfxFilePaths;

        /// <inheritdoc />
        public override short ClassId => 176;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            tryGetAsset(this.partsFilePath, out this.partsFile);
            tryGetAsset(this.motionGraphFilePath, out this.motionGraphFile);
            tryGetAsset(this.mtarFilePath, out this.mtarFile);
            tryGetAsset(this.extensionMtarFilePath, out this.extensionMtarFile);

            this.vfxFiles = new OrderedDictionary_string_Object();
            foreach (var entry in this.vfxFilePaths)
            {
                UnityEngine.Object asset;
                tryGetAsset(entry.Value, out asset);
                this.vfxFiles.Add(entry.Key, asset);
            }
        }
    }
}