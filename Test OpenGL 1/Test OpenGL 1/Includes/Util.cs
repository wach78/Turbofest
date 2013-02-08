﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    static class Util
    {
        private static int maxTextures;
        private static int maxBuffers;
        private static int currentTextureBuffers;

        #region Constructor
        static Util()
        {
            GL.GetInteger(GetPName.MaxCombinedTextureImageUnits, out maxTextures);
            GL.GetInteger(GetPName.MaxDrawBuffers, out maxBuffers);
            currentTextureBuffers = 0;
        }
        #endregion

        #region Method
        public static int LoadTexture(string filename, TextureMinFilter MinFilter = TextureMinFilter.Linear, TextureMagFilter MagFilter = TextureMagFilter.Linear,
            TextureWrapMode WrapS = TextureWrapMode.Clamp, TextureWrapMode WrapT = TextureWrapMode.Clamp, Color Transparant = new Color())
        {
            Bitmap bitmap = new Bitmap(filename);

            return LoadTexture(bitmap, MinFilter, MagFilter, WrapS, WrapT, Transparant);
        }//LoadTexture

        public static int LoadTexture(Bitmap bitmap, TextureMinFilter MinFilter = TextureMinFilter.Linear, TextureMagFilter MagFilter = TextureMagFilter.Linear,
            TextureWrapMode WrapS = TextureWrapMode.Clamp, TextureWrapMode WrapT = TextureWrapMode.Clamp, Color Transparant = new Color())
        {
            int tex;
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            bitmap.MakeTransparent(Transparant);

            //GL.GenTextures(1, out tex);
            //currentTextureBuffers++;
            GenTextureID(out tex);
            
            GL.BindTexture(TextureTarget.Texture2D, tex);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)MinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)MagFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)WrapS);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)WrapT);

            return tex;
        }//LoadTexture

        public static int GenTextureID()
        {
            if (CurrentUsedTextures >= 160)
            {
                throw new Exception("To many texture buffers used!");
            }
            currentTextureBuffers++;
            return GL.GenTexture();
        }

        public static void GenTextureID(out int tid)
        {
            if (CurrentUsedTextures >= 160)
            {
                throw new Exception("To many texture buffers used!");
            }
            currentTextureBuffers++;
            GL.GenTextures(1, out tid);
        }

        /// <summary>
        /// Frees up the texture from memory and sets it to -1 as there is no texture that is acitve
        /// </summary>
        /// <param name="Texture">If cleard this is -1</param>
        public static void DeleteTexture(ref int Texture)
        {
            GL.DeleteTextures(1, ref Texture);
            currentTextureBuffers--;
            Texture = -1;
            if (currentTextureBuffers < 0)
            {
                throw new Exception("Can't be less then zero current textures.");
            }
        }//DeleteTexture
        #endregion

        #region Property
        public static int CurrentUsedTextures
        {
            get
            {
                return currentTextureBuffers;
            }
        }

        public static int MaxTextures
        {
            get
            {
                return maxTextures;
            }
        }

        public static int MaxBuffers
        {
            get
            {
                return maxBuffers;
            }
        }
        #endregion
    }//class
}//namespace