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
                return Vector3.back;/*
                return this.transform_translation;*/
            }
            set
            {
                //this.transform_translation = value;
            }
        }

        public Quaternion RotQuat
        {
            get
            {
                return Quaternion.identity;//this.transform_rotation_quat;
            }
            set
            {
                //this.transform_rotation_quat = value;
            }
        }

        public Vector3 Scale
        {
            get
            {
                return Vector3.back;//this.transform_scale;
            }
            set
            {
                //this.transform_scale = value;
            }
        }
    }
}