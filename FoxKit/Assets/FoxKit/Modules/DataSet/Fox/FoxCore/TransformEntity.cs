namespace FoxKit.Modules.DataSet.Fox.FoxCore
{
    using Quaternion = UnityEngine.Quaternion;
    using Vector3 = UnityEngine.Vector3;

    public partial class TransformEntity
    {
        /// <summary>
        /// The translation.
        /// </summary>
        public Vector3 Translation
        {
            get
            {
                return this.translation;
            }
            set
            {
                this.transform_translation = value;
                this.translation = value;
            }
        }

        public Quaternion RotQuat
        {
            get
            {
                return this.rotQuat;
            }
            set
            {
                this.transform_rotation_quat = value;
                this.rotQuat = value;
            }
        }

        public Vector3 Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.transform_scale = value;
                this.scale = value;
            }
        }

        public TransformEntity()
            : base()
        {
            this.scale = new Vector3(1, 1, 1);
            this.transform_scale = this.scale;
        }

        protected override void OnPropertiesLoaded()
        {
            base.OnPropertiesLoaded();

            this.translation = this.transform_translation;
            this.rotQuat = this.transform_rotation_quat;
            this.transform_scale.x *= -1;
            this.scale = this.transform_scale;
        }

        public override void OnPreparingToExport()
        {
            base.OnPreparingToExport();

            this.transform_translation = this.translation;
            this.transform_rotation_quat = this.rotQuat;
            this.transform_scale = this.scale;

            this.Scale = new Vector3(-this.Scale.x, this.Scale.y, this.Scale.z);
        }

        public override void OnFinishedExporting()
        {
            base.OnFinishedExporting();

            this.Scale = new Vector3(-this.Scale.x, this.Scale.y, this.Scale.z);
        }
    }
}