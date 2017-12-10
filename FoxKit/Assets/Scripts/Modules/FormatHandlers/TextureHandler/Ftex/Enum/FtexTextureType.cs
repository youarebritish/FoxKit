namespace FtexTool.Ftex.Enum
{
    public enum FtexTextureType
    {
        /// <summary>
        ///     SRM and MTM files.
        ///     In SRM files each channel acts as a different material parameter.
        ///     R = Ambient Occlusion
        ///     G = Specular Albedo
        ///     B = Roughness
        /// </summary>
        MaterialMap = 0x01000001,

        /// <summary>
        ///     BSM files
        ///     Albedo diffuse texture
        /// </summary>
        DiffuseMap = 0x01000003,

        /// <summary>
        ///     CBM files
        ///     CubeMap
        /// </summary>
        CubeMap = 0x01000007,

        /// <summary>
        ///     NRM files
        ///     NormalMap
        /// </summary>
        NormalMap = 0x01000009
    }
}
