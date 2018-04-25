using FoxKit.Modules.DataSet.FoxCore;
using System;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Sdx
{
    [Serializable]
    [CreateAssetMenu(fileName = "TexturePackLoadConditioner", menuName = "TexturePackLoadConditioner", order = 1)]
    public class TexturePackLoadConditioner : Data
    {
        public string TexturePackPath;
    }
}