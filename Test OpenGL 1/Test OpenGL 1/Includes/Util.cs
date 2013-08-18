using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.IO;
using OpenTK;

namespace OpenGL
{
    static class Util
    {
        private static int maxShaderVertexTextures; // Max combinde shader - and vertex texture.
        private static int maxBuffers; //color buffers
        private static int currentTextureBuffers; // the number of current generated Texture buffers created
        public static bool Lightning;
        public static bool Fog;
        public static bool Fullscreen;
        public static bool ShowClock;

        private static bool MVP_changed;
        private static bool Viewport_changed;
        private static Matrix4 MVPMatrix;
        private static float[] viewport;

        //fix me...
        public static float FOV = OpenTK.MathHelper.DegreesToRadians(60.0f);
        public static float Aspect = 1.6f;

        //
        public static Random Rnd;


        #region Constructor
        static Util()
        {
            GL.GetInteger(GetPName.MaxCombinedTextureImageUnits, out maxShaderVertexTextures);
            GL.GetInteger(GetPName.MaxDrawBuffers, out maxBuffers);
            currentTextureBuffers = 0;
            Lightning = false;
            Fog = true;
            Fullscreen = false;
            ShowClock = false;
            MVP_changed = true;
            Viewport_changed = true;
            viewport = new float[4];
            Rnd = new Random();
        }
        #endregion

        #region Method
        public static int LoadTexture(string filename, out float Width, out float Height, TextureMinFilter MinFilter = TextureMinFilter.Linear, TextureMagFilter MagFilter = TextureMagFilter.Linear,
            TextureWrapMode WrapS = TextureWrapMode.Clamp, TextureWrapMode WrapT = TextureWrapMode.Clamp, Color Transparant = new Color())
        {
            Bitmap bitmap = null;
            if (File.Exists(filename))
            {
                bitmap = new Bitmap(filename, false);
                Width = bitmap.Width;
                Height = bitmap.Height;
            }
            else
            {
                throw new Exception("Missing Bitmap-file!");
            }


            return LoadTexture(bitmap, MinFilter, MagFilter, WrapS, WrapT, Transparant);
        }//LoadTexture

        public static int LoadTexture(string filename, TextureMinFilter MinFilter = TextureMinFilter.Linear, TextureMagFilter MagFilter = TextureMagFilter.Linear,
            TextureWrapMode WrapS = TextureWrapMode.Clamp, TextureWrapMode WrapT = TextureWrapMode.Clamp, Color Transparant = new Color())
        {
            Bitmap bitmap = null;
            if (File.Exists(filename))
            {
                //Console.WriteLine("-----> " + filename);
                bitmap = new Bitmap(filename, false);
            }
            else
            {
                throw new Exception("Missing Bitmap-file!");
            }
            

            return LoadTexture(bitmap, MinFilter, MagFilter, WrapS, WrapT, Transparant);
        }//LoadTexture

        public static int LoadTexture(Bitmap bitmap, TextureMinFilter MinFilter = TextureMinFilter.Linear, TextureMagFilter MagFilter = TextureMagFilter.Linear,
            TextureWrapMode WrapS = TextureWrapMode.Clamp, TextureWrapMode WrapT = TextureWrapMode.Clamp, Color Transparant = new Color())
        {
            if (bitmap == null)
            {
                throw new Exception("Bitmap is null, texture is not loading!");
            }
            int tex;
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            if (!Transparant.IsEmpty)
            {
                bitmap.MakeTransparent(Transparant);
            }
            //GL.GenTextures(1, out tex);
            //currentTextureBuffers++;
            GenTextureID(out tex);
            GL.BindTexture(TextureTarget.Texture2D, tex);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb/*bitmap.PixelFormat*/);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);
            bitmap.Dispose();
            bitmap = null;

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)MinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)MagFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)WrapS);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)WrapT);
             return tex;
        }//LoadTexture

        public static int GenTextureID()
        {
            /*if (CurrentUsedTextures >= 160)
            {
                throw new Exception("To many texture buffers used!");
            }*/
            currentTextureBuffers++;
            int tex = GL.GenTexture();
            //System.Diagnostics.Debug.WriteLine("Create: " + tex + ", " + currentTextureBuffers);
            return tex;
        }

        public static void GenTextureID(out int tid)
        {
            /*if (CurrentUsedTextures >= 160)
            {
                throw new Exception("To many texture buffers used!");
            }*/
            currentTextureBuffers++;
            GL.GenTextures(1, out tid);
            //System.Diagnostics.Debug.WriteLine("Create: " + tid + ", " + currentTextureBuffers);
        }

        /// <summary>
        /// Frees up the texture from memory and sets it to -1 as there is no texture that is acitve
        /// </summary>
        /// <param name="Texture">If cleard this is -1</param>
        public static void DeleteTexture(ref int Texture)
        {
            /*if (GL.IsTexture(Texture))
            {*/
            //System.Diagnostics.Debug.WriteLine("Delete: " + Texture + ", " + currentTextureBuffers);
                GL.DeleteTextures(1, ref Texture);
                currentTextureBuffers--;
                Texture = -1;
                if (currentTextureBuffers < 0)
                {
                    throw new Exception("Can't be less then zero current textures.");
                }
            /*}
            else
            {
                throw new Exception("Not a texture, so can't delete it!");
            }*/
        }//DeleteTexture

        /// <summary>
        /// Might need to rethink this as this make my head spin on the same point as this isen't helping that mutch ;D
        /// </summary>
        /// <param name="changed"></param>
        /// <returns></returns>
        public static bool ViewportChanged(bool changed)
        {
            return (Viewport_changed = changed);
        }

        public static float[] GetViewport()
        {
            if (Viewport_changed)
            {
                GL.GetFloat(GetPName.Viewport, viewport);
                Viewport_changed = false;
            }
            return viewport;
        }

        public static bool MVPChanged(bool changed)
        {
            return (MVP_changed = changed);
        }

        public static Matrix4 GetMVP()
        {
            /*//float[] viewport = new float[4];
            //float[] projectiofM = new float[16];
            //float[] modelviewfM = new float[16];
            Matrix4 projectionM = new Matrix4();
            Matrix4 modelviewM = new Matrix4();
            Matrix4 projMultimodel;

            //GL.GetFloat(GetPName.Viewport, viewport);
            GL.GetFloat(GetPName.ProjectionMatrix, out projectionM);
            GL.GetFloat(GetPName.ModelviewMatrix, out modelviewM);
            //projMultimodel = Matrix4.Mult(projectionM, modelviewM);
            projMultimodel = Matrix4.Mult(modelviewM, projectionM);
            return projMultimodel;*/
            if (MVP_changed)
            {
                Matrix4 projectionM = new Matrix4();
                Matrix4 modelviewM = new Matrix4();
                GL.GetFloat(GetPName.ProjectionMatrix, out projectionM);
                GL.GetFloat(GetPName.ModelviewMatrix, out modelviewM);
                MVPMatrix = Matrix4.Mult(modelviewM, projectionM);
                MVP_changed = false;
            }
            return MVPMatrix;
        }

        /// <summary>
        /// This is the frustum.
        /// </summary>
        /// <param name="normalize">Normalize the vectors</param>
        /// <returns>0-3 is left,right,top,bottom, 4-5 is the near/far plane</returns>
        public static Vector4[] GetFrustum(bool normalize)
        {
            // Get bounds of view.
            float[] viewport = new float[4];
            float[] projectiofM = new float[16];
            float[] modelviewfM = new float[16];
            Matrix4 projectionM = new Matrix4();
            Matrix4 modelviewM = new Matrix4();
            Matrix4 projMultimodel;// = new Matrix4();
            Matrix4 ScreenFrustum = new Matrix4(); // all 4 used
            Vector4[] NearFarFrustum = new Vector4[2]; // only 0-1 used, 2-3 is zero
            Vector4[] RetArr = new Vector4[6]; // only 0-1 used, 2-3 is zero
            GL.GetFloat(GetPName.Viewport, viewport);
            GL.GetFloat(GetPName.ProjectionMatrix, out projectionM);
            GL.GetFloat(GetPName.ModelviewMatrix, out modelviewM);
            projMultimodel = Matrix4.Mult(projectionM, modelviewM);

            // Got the wrong order used columns when it should have been rows.....
            /*Vector4 rPlane = new Vector4(projMultimodel.Column0.W - projMultimodel.Column0.X,
                projMultimodel.Column1.W - projMultimodel.Column1.X,
                projMultimodel.Column2.W - projMultimodel.Column2.X,
                projMultimodel.Column3.W - projMultimodel.Column3.X);*/
            Vector4 rPlane = new Vector4(projMultimodel.Column3.X - projMultimodel.Column0.X,
                projMultimodel.Column3.Y - projMultimodel.Column1.X,
                projMultimodel.Column3.Z - projMultimodel.Column2.X,
                projMultimodel.Column3.W - projMultimodel.Column3.X);
            if (normalize)
            {
                rPlane.Normalize();
            }

            Vector4 rPlaneManual = new Vector4(projMultimodel.M14 - projMultimodel.M11,
                projMultimodel.M24 - projMultimodel.M21,
                projMultimodel.M34 - projMultimodel.M31,
                projMultimodel.M44 - projMultimodel.M41);
            if (normalize)
            {
                rPlaneManual.Normalize();
            }

            /*Vector4 rPlaneManual2;
            unsafe
            {
                float* clip1 = (float*)(&projMultimodel);
                rPlaneManual2 = new Vector4(clip1[3] - clip1[0], clip1[7] - clip1[4], clip1[11] - clip1[8], clip1[15] - clip1[12]);
                rPlaneManual2.Normalize();
            }
            */

            /*Vector4 lPlane = new Vector4(projMultimodel.Column0.W + projMultimodel.Column0.X,
                projMultimodel.Column1.W + projMultimodel.Column1.X,
                projMultimodel.Column2.W + projMultimodel.Column2.X,
                projMultimodel.Column3.W + projMultimodel.Column3.X);*/
            /*
            Vector4 lPlane = new Vector4(projMultimodel.Column3.X + projMultimodel.Column0.X,
                projMultimodel.Column3.Y + projMultimodel.Column1.X,
                projMultimodel.Column3.Z + projMultimodel.Column2.X,
                projMultimodel.Column3.W + projMultimodel.Column3.X);*/
            Vector4 row = projMultimodel.Row0;
            Vector4 lPlane = new Vector4(projMultimodel.Column3.X + row.X,
               projMultimodel.Column3.Y + row.Y,
               projMultimodel.Column3.Z + row.Z,
               projMultimodel.Column3.W + row.W);
            if (normalize)
            {
                lPlane.Normalize();
            }
            /*Vector4 bPlane = new Vector4(projMultimodel.Column0.W - projMultimodel.Column0.Y,
                projMultimodel.Column1.W - projMultimodel.Column1.Y,
                projMultimodel.Column2.W - projMultimodel.Column2.Y,
                projMultimodel.Column3.W - projMultimodel.Column3.Y);*/
            Vector4 bPlane = new Vector4(projMultimodel.Column3.X - projMultimodel.Column0.Y,
                projMultimodel.Column3.Y - projMultimodel.Column1.Y,
                projMultimodel.Column3.Z - projMultimodel.Column2.Y,
                projMultimodel.Column3.W - projMultimodel.Column3.Y);
            if (normalize)
            {
                bPlane.Normalize();
            }
            /*Vector4 tPlane = new Vector4(projMultimodel.Column0.W + projMultimodel.Column0.Y,
                projMultimodel.Column1.W + projMultimodel.Column1.Y,
                projMultimodel.Column2.W + projMultimodel.Column2.Y,
                projMultimodel.Column3.W + projMultimodel.Column3.Y);*/
            Vector4 tPlane = new Vector4(projMultimodel.Column3.X + projMultimodel.Column0.Y,
                projMultimodel.Column3.Y + projMultimodel.Column1.Y,
                projMultimodel.Column3.Z + projMultimodel.Column2.Y,
                projMultimodel.Column3.W + projMultimodel.Column3.Y);
            if (normalize)
            {
                tPlane.Normalize();
            }
            ScreenFrustum = new Matrix4(rPlane, lPlane, bPlane, tPlane);

            /*NearFarFrustum[0] = new Vector4(projMultimodel.Column0.W + projMultimodel.Column0.Z,
                projMultimodel.Column1.W + projMultimodel.Column1.Z,
                projMultimodel.Column2.W + projMultimodel.Column2.Z,
                projMultimodel.Column3.W + projMultimodel.Column3.Z);*/
            NearFarFrustum[0] = new Vector4(projMultimodel.Column3.X + projMultimodel.Column0.Z,
                projMultimodel.Column3.Y + projMultimodel.Column1.Z,
                projMultimodel.Column3.Z + projMultimodel.Column2.Z,
                projMultimodel.Column3.W + projMultimodel.Column3.Z);
            if (normalize)
            {
                NearFarFrustum[0].Normalize();
            }
            /*NearFarFrustum[1] = new Vector4(projMultimodel.Column0.W - projMultimodel.Column0.Z,
                projMultimodel.Column1.W - projMultimodel.Column1.Z,
                projMultimodel.Column2.W - projMultimodel.Column2.Z,
                projMultimodel.Column3.W - projMultimodel.Column3.Z);*/
            NearFarFrustum[1] = new Vector4(projMultimodel.Column3.X - projMultimodel.Column0.Z,
                projMultimodel.Column3.Y - projMultimodel.Column1.Z,
                projMultimodel.Column3.Z - projMultimodel.Column2.Z,
                projMultimodel.Column3.W - projMultimodel.Column3.Z);
            if (normalize)
            {
                NearFarFrustum[1].Normalize();
            }

            RetArr[0] = ScreenFrustum.Row0;
            RetArr[1] = ScreenFrustum.Row1;
            RetArr[2] = ScreenFrustum.Row2;
            RetArr[3] = ScreenFrustum.Row3;
            RetArr[4] = NearFarFrustum[0];
            RetArr[5] = NearFarFrustum[1];

            return RetArr;
        }

        // new test for get max with...


        public static Vector4 UnProject(Matrix4 projection, Matrix4 view, Size viewport, Vector3 mouse)
        {
            Vector4 vec;

            vec.X = 2.0f * mouse.X / (float)viewport.Width - 1;
            vec.Y = -(2.0f * mouse.Y / (float)viewport.Height - 1);
            vec.Z = 1.0f;
            vec.W = 1.0f;

            Matrix4 viewInv = Matrix4.Invert(view);
            Matrix4 projInv = Matrix4.Invert(projection);

            Vector4.Transform(ref vec, ref projInv, out vec);
            Vector4.Transform(ref vec, ref viewInv, out vec);

            if (vec.W > float.Epsilon || vec.W < float.Epsilon)
            {
                vec.X /= vec.W;
                vec.Y /= vec.W;
                vec.Z /= vec.W;
            }

            return vec;
        }

        public static Vector4 Project(OpenTK.Vector4 objPos, Matrix4 projection, Matrix4 view, Size viewport)
        {
            Vector4 vec = objPos;

            vec = Vector4.Transform(vec, Matrix4.Mult(projection, view));

            vec.X = (vec.X + 1) * (viewport.Width / 2);
            vec.Y = (vec.Y + 1) * (viewport.Height / 2);

            return vec;
        }

        /// <summary>
        /// Method to make printouts more controlled not only by debug enabeling but also with compiler option,
        /// DEBUGPRINT or RELEASEPRINT
        /// </summary>
        /// <param name="Text">Text to be printed</param>
        public static void DebugPrint(string Text)
        {
#if DEBUGPRINT
            System.Diagnostics.Debug.WriteLine(Text);
#elif RELEASEPRINT
            System.Console.WriteLine(Text);
#endif
        }
        #endregion

        #region Property
        public static int CurrentUsedTextures
        {
            get
            {
                return currentTextureBuffers;
            }
        }

        public static int MaxCombindeTextures
        {
            get
            {
                return maxShaderVertexTextures;
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
