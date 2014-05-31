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
using System.Xml.Linq;

namespace OpenGL
{
    /// <summary>
    /// Static helper class with some things to help control the OpenGL environment and system
    /// </summary>
    static public class Util
    {
        private static int maxShaderVertexTextures; // Max combinde shader - and vertex texture.
        private static int maxBuffers; //color buffers
        private static int currentTextureBuffers; // the number of current generated Texture buffers created
        private static int texMaxSize;
        /// <summary>
        /// OpenGL Lightning active?
        /// </summary>
        public static bool Lightning;
        /// <summary>
        /// OpenGL fog active?
        /// </summary>
        public static bool Fog;
        /// <summary>
        /// Run/ning in fullscreen?
        /// </summary>
        public static bool Fullscreen;
        /// <summary>
        /// Show PartyClock?
        /// </summary>
        public static bool ShowClock;

        private static string springOrFall;
        private static string strWorkingPath;
        private static bool MVP_changed;
        private static bool Viewport_changed;
        private static Matrix4 MVPMatrix;
        private static float[] viewport;

        //fix me...
        /// <summary>
        /// Field of view
        /// </summary>
        public static float FOV = OpenTK.MathHelper.DegreesToRadians(60.0f);
        /// <summary>
        /// Aspec ratio
        /// </summary>
        public static float Aspect = 1.6f;

        /// <summary>
        /// Premade random loaded
        /// </summary>
        public static Random Rnd;


        #region Constructor
        /// <summary>
        /// Constructor for the static Utils
        /// </summary>
        static Util()
        {
            GL.GetInteger(GetPName.MaxCombinedTextureImageUnits, out maxShaderVertexTextures);
            GL.GetInteger(GetPName.MaxDrawBuffers, out maxBuffers);
            GL.GetInteger(GetPName.MaxTextureSize, out texMaxSize);
            currentTextureBuffers = 0;
            Lightning = false;
            Fog = true;
            Fullscreen = false;
            ShowClock = false;
            MVP_changed = true;
            Viewport_changed = true;
            viewport = new float[4];
            Rnd = new Random();
            springOrFall = string.Empty;
            strWorkingPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
        }
        #endregion

        #region Method
        /// <summary>
        /// Load a texture to graphic memory
        /// </summary>
        /// <param name="filename">The filepath to the file to load</param>
        /// <param name="Width">How wide is the image</param>
        /// <param name="Height">How highe is the image</param>
        /// <param name="MinFilter">What filter to use</param>
        /// <param name="MagFilter">What filter to use</param>
        /// <param name="WrapS">Wrap it X?</param>
        /// <param name="WrapT">Wrap it Y?</param>
        /// <param name="Transparant">Do we have a transparant colour in the image?</param>
        /// <returns>TextureID from OpenGL</returns>
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

        /// <summary>
        /// Load a texture to graphic memory
        /// </summary>
        /// <param name="filename">The filepath to the file to load</param>
        /// <param name="MinFilter">What filter to use</param>
        /// <param name="MagFilter">What filter to use</param>
        /// <param name="WrapS">Wrap it X?</param>
        /// <param name="WrapT">Wrap it Y?</param>
        /// <param name="Transparant">Do we have a transparant colour in the image?</param>
        /// <returns>TextureID from OpenGL</returns>
        public static int LoadTexture(string filename, TextureMinFilter MinFilter = TextureMinFilter.Nearest, TextureMagFilter MagFilter = TextureMagFilter.Linear,
            TextureWrapMode WrapS = TextureWrapMode.Clamp, TextureWrapMode WrapT = TextureWrapMode.Clamp, Color Transparant = new Color())
        {
            Bitmap bitmap = null;
            if (File.Exists(filename))
            {
                //Console.WriteLine("-----> " + filename);
                bitmap = new Bitmap(filename, false);
                if (bitmap.Width >= Util.MaxTexturesSizeWidth || bitmap.Height >= Util.MaxTexturesSizeWidth)
                {
                    throw new Exception("GFX/Texture is to large it excides the allowed size by your hardware (" + Util.MaxTexturesSizeWidth + " x " + Util.MaxTexturesSizeWidth + "), " + filename);
                }
            }
            else
            {
                throw new Exception("Missing Bitmap-file!");
            }

            return LoadTexture(bitmap, MinFilter, MagFilter, WrapS, WrapT, Transparant);
        }//LoadTexture

        /// <summary>
        /// Load a texture to graphic memory
        /// </summary>
        /// <param name="bitmap">Bitmap data</param>
        /// <param name="MinFilter">What filter to use</param>
        /// <param name="MagFilter">What filter to use</param>
        /// <param name="WrapS">Wrap it X?</param>
        /// <param name="WrapT">Wrap it Y?</param>
        /// <param name="Transparant">Do we have a transparant colour in the image?</param>
        /// <returns>TextureID from OpenGL</returns>
        public static int LoadTexture(Bitmap bitmap, TextureMinFilter MinFilter = TextureMinFilter.Nearest, TextureMagFilter MagFilter = TextureMagFilter.Linear,
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

            if (bitmap.Width >= Util.MaxTexturesSizeWidth || bitmap.Height >= Util.MaxTexturesSizeWidth)
            {
                throw new Exception("GFX/Texture is to large it excides the allowed size by your hardware (" + Util.MaxTexturesSizeWidth + " x " + Util.MaxTexturesSizeWidth + ")");
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

        /// <summary>
        /// Generate a TextureID with OpenGL and keep count on how many we have made
        /// </summary>
        /// <returns>TextureID from OpenGL</returns>
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

        /// <summary>
        /// Generate a TextureID with OpenGL and keep count on how many we have made
        /// </summary>
        /// <param name="tid">TextureID from OpenGL</param>
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
            if (GL.IsTexture(Texture))
            {
                //System.Diagnostics.Debug.WriteLine("Delete: " + Texture + ", " + currentTextureBuffers);
                if (Texture >= 0)
                {
                    GL.DeleteTextures(1, ref Texture);
                    currentTextureBuffers--;
                    Texture = -1;
                    if (currentTextureBuffers < 0)
                    {
                        throw new Exception("Can't be less then zero current textures.");
                    }
                }
            }
            /*else
            {
                throw new Exception("Not a texture, so can't delete it!");
            }*/
        }//DeleteTexture

        /// <summary>
        /// Might need to rethink this as this make my head spin on the same point as this isen't helping that mutch ;D
        /// </summary>
        /// <param name="changed">Have a change of view port happend?</param>
        /// <returns>Is the change done?</returns>
        public static bool ViewportChanged(bool changed)
        {
            return (Viewport_changed = changed);
        }

        /// <summary>
        /// Get the current viewport 
        /// </summary>
        /// <returns>Current area of the viewport</returns>
        public static float[] GetViewport()
        {
            if (Viewport_changed)
            {
                GL.GetFloat(GetPName.Viewport, viewport);
                Viewport_changed = false;
            }
            return viewport;
        }

        /// <summary>
        /// Have the Viewport matrix changed?
        /// </summary>
        /// <param name="changed">Is it changed?</param>
        /// <returns>Have the changed been done?</returns>
        public static bool MVPChanged(bool changed)
        {
            return (MVP_changed = changed);
        }

        /// <summary>
        /// Get the Viewport matrix
        /// </summary>
        /// <returns>Matrix4 of the viewport</returns>
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

        /// <summary>
        /// UnProject the projection ie. flatten it on the screen
        /// </summary>
        /// <param name="projection"></param>
        /// <param name="view"></param>
        /// <param name="viewport"></param>
        /// <param name="mouse"></param>
        /// <returns>bad...</returns>
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

        /// <summary>
        /// Project it on the screen
        /// </summary>
        /// <param name="objPos"></param>
        /// <param name="projection"></param>
        /// <param name="view"></param>
        /// <param name="viewport"></param>
        /// <returns>bad...</returns>
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
        /// <summary>
        /// How many textures have been created and are active still
        /// </summary>
        public static string CurrentExecutionPath
        {
            get
            {
                return strWorkingPath;
            }
        }

        /// <summary>
        /// How many textures have been created and are active still
        /// </summary>
        public static int CurrentUsedTextures
        {
            get
            {
                return currentTextureBuffers;
            }
        }

        /// <summary>
        /// How many combined textures can there be on this system?
        /// </summary>
        public static int MaxCombindeTextures
        {
            get
            {
                return maxShaderVertexTextures;
            }
        }

        /// <summary>
        /// Get the maximum width of a texture that fits on the texture memory.
        /// There is not way to get the Height of it except if using proxy texture...
        /// </summary>
        public static int MaxTexturesSizeWidth
        {
            get
            {
                return texMaxSize;
            }
        }

        /// <summary>
        /// How many buffers can there be on this system?
        /// </summary>
        public static int MaxBuffers
        {
            get
            {
                return maxBuffers;
            }
        }

        /// <summary>
        /// Is it spring or fall?
        /// </summary>
        public static string SpringOrFall
        {
            get { return springOrFall; }
            set { springOrFall = value; }
        }
        #endregion

        /// <summary>
        /// List of months
        /// </summary>
        /// <returns>A string array with the short names of the months in Swedish</returns>
        public static string[] monthlist()
        {
            string[] monthstlist;

            if ("Spring".Equals(springOrFall))
            {

                monthstlist = new string[] { "sep", "okt", "nov", "dec", "jan", "feb", "mar" };
            }
            else
            {
                monthstlist = new string[] { "mar", "apr", "maj", "jun", "jul", "aug", "sep" };
            }
            return monthstlist;
        }

    }//class

    /// <summary>
    /// Helper class for XML file handling
    /// </summary>
    public static class UtilXML
    {
        /// <summary>
        /// How we present event data to the program
        /// </summary>
        public class EventData
        {
            #region Variables
            private string strName;
            //private bool blnVeto;
            // private int intPrio;
            private List<string> namelist;
            private List<int> runslist;
            private List<bool> runAllowedlist;
            private List<int> priolist;
            private List<bool> vetolist;
            #endregion

            #region Constructor
            /// <summary>
            /// Constructor of the event
            /// </summary>
            /// <param name="Name">Name of event</param>
            public EventData(string Name)
            {
                strName = Name;
                // blnVeto = Veto;
                //intPrio = Prio;
                namelist = new List<string>();
                runslist = new List<int>();
                runAllowedlist = new List<bool>();
                priolist = new List<int>();
                vetolist = new List<bool>();
            }

            /// <summary>
            /// Constructor of empty event
            /// </summary>
            public EventData()
            {
                namelist = new List<string>();
                runslist = new List<int>();
                runAllowedlist = new List<bool>();
                priolist = new List<int>();
                vetolist = new List<bool>();
            }
            #endregion

            #region Properties
            /// <summary>
            /// Name of Event
            /// </summary>
            public string Name
            {
                get { return strName; }
            }

            /// <summary>
            /// Name of month
            /// </summary>
            public List<string> Namelist
            {
                get { return namelist; }
            }

            /// <summary>
            /// How many runs Event do
            /// </summary>
            public List<int> Runslist
            {
                get { return runslist; }
            }

            /// <summary>
            /// Allowed to run?
            /// </summary>
            public List<bool> RunAllowedlist
            {
                get { return runAllowedlist; }
            }

            /// <summary>
            /// Do this Event have high/low prio in the month?
            /// </summary>
            public List<int> Priolist
            {
                get { return priolist; }
            }

            /// <summary>
            /// This Event forced to run in this month?
            /// </summary>
            public List<bool> VetoList
            {
                get { return vetolist; }
            }
            #endregion

            #region Methods
            /// <summary>
            /// Set in what month it can be used
            /// </summary>
            /// <param name="name">Name of month</param>
            /// <param name="runs">How many runs it can do</param>
            /// <param name="allowed">Can this be used</param>
            /// <param name="veto">Force this to run</param>
            /// <param name="prio">What is the priority</param>
            public void setData(string name, int runs, bool allowed, bool veto, int prio)
            {
                namelist.Add(name);
                runslist.Add(runs);
                runAllowedlist.Add(allowed);
                priolist.Add(prio);
                vetolist.Add(veto);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(object obj)
            {
                EventData Compare = (EventData)obj;
                int result = this.Name.CompareTo(Compare.Name);
                /*if (result == 0)
                    result = this.Name.CompareTo(Compare.Name);*/
                return result;
            }
            #endregion
        }

        static UtilXML()
        {

        }

        /// <summary>
        /// Load effect xml and get stuff oriented as they should
        /// </summary>
        /// <returns></returns>
        public static List<EventData> Loadeffectdata()
        {
            List<EventData> objlist = new List<EventData>();

            XDocument xDoc = XDocument.Load(Util.CurrentExecutionPath + "/XMLFiles/Effects/randomeffects" + Util.SpringOrFall + ".xml");

            var effects = xDoc.Descendants("effect");

            foreach (var effect in effects)
            {
                EventData ev = new EventData(effect.Element("Name").Value);

                var months = effect.Descendants("Month");

                foreach (var m in months)
                {
                    ev.setData(m.Element("Name").Value, Int16.Parse(m.Element("Runs").Value), bool.Parse(m.Element("RunAllowed").Value), bool.Parse(m.Element("Veto").Value), Int16.Parse(m.Element("Prio").Value));
                }

                objlist.Add(ev);
            }

            return objlist;
        }
    }
}//namespace
