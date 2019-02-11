using FoxKit.Modules.DataSet.Fox.FoxCore;
using UnityEditor;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Fox.Sdx
{
    public partial class SoundSource : TransformData
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(AudioSource)).image as Texture2D;
    }
}
