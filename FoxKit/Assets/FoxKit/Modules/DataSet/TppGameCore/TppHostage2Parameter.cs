namespace FoxKit.Modules.DataSet.TppGameCore
{
    using FoxKit.Modules.DataSet.GameCore;
    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

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
        private ObjectStringMap vfxFiles;

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
        private StringStringMap vfxFilePaths;

        /// <inheritdoc />
        protected override short ClassId => 176;

        /// <inheritdoc />
        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            tryGetAsset(this.partsFilePath, out this.partsFile);
            tryGetAsset(this.motionGraphFilePath, out this.motionGraphFile);
            tryGetAsset(this.mtarFilePath, out this.mtarFile);
            tryGetAsset(this.extensionMtarFilePath, out this.extensionMtarFile);

            this.vfxFiles = new ObjectStringMap();
            foreach (var entry in this.vfxFilePaths)
            {
                UnityEngine.Object asset;
                tryGetAsset(entry.Value, out asset);
                this.vfxFiles.Add(entry.Key, asset);
            }
        }

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "partsFile":
                    this.partsFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "motionGraphFile":
                    this.motionGraphFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "mtarFile":
                    this.mtarFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "extensionMtarFile":
                    this.extensionMtarFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "vfxFiles":
                    var dictionary = DataSetUtils.GetStringMap<FoxFilePtr>(propertyData);

                    this.vfxFilePaths = new StringStringMap();
                    foreach (var entry in dictionary)
                    {
                        var path = this.extensionMtarFilePath = DataSetUtils.ExtractFilePath(entry.Value);
                        this.vfxFilePaths.Add(entry.Key.ToString(), path);
                    }

                    break;
            }
        }
    }
}