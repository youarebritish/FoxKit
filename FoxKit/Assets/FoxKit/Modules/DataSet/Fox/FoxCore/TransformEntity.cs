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
                return this.transform_translation;
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
                return this.transform_rotation_quat;
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
                return this.transform_scale;
            }
            set
            {
                this.transform_scale = value;
                this.scale = value;
            }
        }
    }
}