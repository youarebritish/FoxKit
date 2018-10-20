namespace FoxKit.Modules.Gr.GrTexture.Utils
{
    using System;
    using UnityEngine;

    public static class GrTextureUtils
    {
        public static TextureFormat GetTextureFormat(ushort ddsPixelFormat, ushort depth)
        {
            switch (ddsPixelFormat)
            {
                case 0:
                    // Another Unity quirk. Unity says that it wants either RGBA32 or BGRA32 as a format for volume textures if the BGRA32 format is passed in, so I have to switch the format and swizzle (make RGBA BGRA) the DDS bytes in GrTextureImporter.OnImportAsset().
                    return depth > 1 ? TextureFormat.RGBA32 : TextureFormat.BGRA32;
                case 1:
                    return TextureFormat.R8;
                case 2:
                    return TextureFormat.DXT1;
                case 4:
                    return TextureFormat.DXT5;
                default:
                    throw new ArgumentException($"Unknown PixelFormatType {ddsPixelFormat}");
            }
        }

        public static TextureFormat GetTextureFormat(uint dxgiFormat, uint depth)
        {
            switch (dxgiFormat)
            {
                case 87:
                    // Another Unity quirk. Unity says that it wants either RGBA32 or BGRA32 as a format for volume textures if the BGRA32 format is passed in, so I have to switch the format and swizzle (make RGBA BGRA) the DDS bytes in GrTextureImporter.OnImportAsset().
                    return depth > 1 ? TextureFormat.RGBA32 : TextureFormat.BGRA32;
                case 61:
                    return TextureFormat.R8;
                case 71:
                    return TextureFormat.DXT1;
                case 77:
                    return TextureFormat.DXT5;
                default:
                    throw new ArgumentException($"Unknown DXGI_FORMAT {dxgiFormat}");
            }
        }

        public static ushort GetTextureFormat(TextureFormat textureFormat)
        {
            switch (textureFormat)
            {
                case TextureFormat.RGBA32:
                case TextureFormat.BGRA32:
                    return 0;
                case TextureFormat.R8:
                    return 1;
                case TextureFormat.DXT1:
                    return 2;
                case TextureFormat.DXT5:
                    return 4;
                default:
                    throw new ArgumentException($"Invalid TextureFormat ({textureFormat})");
            }
        }

        public static uint GetDXGIFormat(ushort ddsPixelFormat)
        {
            switch (ddsPixelFormat)
            {
                case 0:
                    return 87; //DXGI_FORMAT_B8G8R8A8_UNORM This one is weird; apparently ARGB is an old name that now actually represents BGRA.
                case 1:
                    return 61; //DXGI_FORMAT_R8_UNORM
                case 2:
                    return 71; //DXGI_FORMAT_BC1_UNORM (DXT1)
                case 4:
                    return 77; //DXGI_FORMAT_BC3_UNORM (DXT5)
                default:
                    throw new ArgumentException($"Unknown PixelFormatType {ddsPixelFormat}");
            }
        }

        public static uint GetDXGIFormat(TextureFormat textureFormat)
        {
            switch (textureFormat)
            {
                case TextureFormat.RGBA32:
                case TextureFormat.BGRA32:
                    return 87; //DXGI_FORMAT_B8G8R8A8_UNORM This one is weird; apparently ARGB is an old name that now actually represents BGRA.
                case TextureFormat.R8:
                    return 61; //DXGI_FORMAT_R8_UNORM
                case TextureFormat.DXT1:
                    return 71; //DXGI_FORMAT_BC1_UNORM (DXT1)
                case TextureFormat.DXT5:
                    return 77; //DXGI_FORMAT_BC3_UNORM (DXT5)
                default:
                    throw new ArgumentException($"Unknown TextureFormat {textureFormat}");
            }
        }

        public static bool GetLinear(FoxLib.GrTexture.TextureType textureType)
        {
            if (textureType == FoxLib.GrTexture.TextureType.Linear || textureType == FoxLib.GrTexture.TextureType.Normal)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Texture3D CreateTexture3D(uint width, uint height, uint depth, TextureFormat textureFormat, byte[] pixels)
        {
            var texture = new Texture3D((int)width, (int)height, (int)depth, /*textureFormat*/TextureFormat.RGBA32, false);

            byte[] newPixelData = DirectXTexHelper.Flip3DImage(width, width, width, GrTextureUtils.GetDXGIFormat(textureFormat), pixels);

            var colours = new Color32[newPixelData.Length / 4];
            for (var i = 0; i < newPixelData.Length; i += 4)
            {
                // Another Unity quirk. Unity says that it wants either RGBA32 or BGRA32 as a format for volume textures if the BGRA32 format is passed in, so I have to correct for it here and in GrTextureUtils.GetTextureFormat().
                //var colour = new Color32(newPixelData[i + 2], newPixelData[i + 1], newPixelData[i + 0], newPixelData[i + 3]);
                var colour = new Color32(newPixelData[i + 2], newPixelData[i + 1], newPixelData[i + 0], newPixelData[i + 3]);
                colours[i / 4] = colour;
            }

            texture.SetPixels32(colours);
            texture.Apply();

            return texture;
        }
    }
}