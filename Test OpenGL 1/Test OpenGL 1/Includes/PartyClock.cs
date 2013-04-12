using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    public class PartyClock : IDisposable
    {
        int[] m_tex;
        double runtime;
        double ticks, old_ticks;
        public double increments, clock;
        public DateTime dtStart;
        public DateTime dtEnd;
        Vector2[,] texTimeVert = new Vector2[8, 4];
        Vector2[,] texDateVert = new Vector2[10, 4];

        // CharPosition 
        private string charset = "0123456789: "; //"0123456789: -";
        private float[] charmap = new float[2];
        private float[] bitmapsize = new float[2];
        private OpenTK.Vector2[] retVal = new Vector2[4]; // used for texture coordinates
        private bool _disposed = false;


        public PartyClock(DateTime start, DateTime end, int runtime)
        {
            clock = 0;
            ticks = old_ticks = 0;
            dtStart = start;
            dtEnd = end;
            this.runtime = runtime;//4; //0.25 * runtime / 4  /**0.017*/; // 0.25 25 mins rt
            TimeSpan tsDiff = dtEnd.Subtract(dtStart);
            increments = (tsDiff.TotalSeconds) / (this.runtime / 1000) /*/ 1000*/; // every second in realtime we increase the clock with this increment
            m_tex = new int[2];

            //System.Drawing.Bitmap bitmapDate = new System.Drawing.Bitmap(/*System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)*/ System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/clockfont40x70.bmp" /*"../../gfx/clockfont40x70.bmp"*/); // this is now more controlled as it will looke for current exec path
            /*System.Drawing.Bitmap bitmapClock = new System.Drawing.Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/clockfont80x140.bmp"); // this is now more controlled as it will looke for current exec path

            GL.GenTextures(2, this.m_tex);

            // Date font
            GL.BindTexture(TextureTarget.Texture2D, this.m_tex[0]);
            System.Drawing.Imaging.BitmapData data = bitmapDate.LockBits(new System.Drawing.Rectangle(0, 0, bitmapDate.Width, bitmapDate.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmapDate.UnlockBits(data);
            data = null;
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Time font
            //GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, this.m_tex[1]);

            data = bitmapClock.LockBits(new System.Drawing.Rectangle(0, 0, bitmapClock.Width, bitmapClock.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmapClock.UnlockBits(data);
            data = null;
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            */
            m_tex[0] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/clockfont40x70.bmp");
            m_tex[1] = Util.LoadTexture(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/gfx/clockfont80x140.bmp");


        }

        ~PartyClock()
        {
            //GL.DeleteBuffers(2, this.m_tex); // casts exception here
        }

        public void Dispose()
        {
            Dispose(true);
            //base.Finalize();
            //GL.DeleteBuffers(2, this.m_tex); // this might bug out or?
            //this.m_tex = null; // protects for miss use if GC haven't been collecting...
            
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these  
            // operations, as well as in your methods that use the resource. 
            if (!_disposed)
            {
                if (disposing)
                {
                    if (m_tex != null)
                    {
                        Util.DeleteTexture(ref m_tex[0]);
                        Util.DeleteTexture(ref m_tex[1]);
                    }
                }

                // Indicate that the instance has been disposed.
                m_tex = null;
                _disposed = true;
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }

        public bool updateClock()
        {
            bool update = true;
            this.ticks = System.DateTime.Now.Ticks * 10 / TimeSpan.TicksPerSecond; // this is not good but it works again...

            if (this.old_ticks != 0)
            {
                //System.Diagnostics.Debug.WriteLine(this.clock + " , " + this.increments + " , " + this.ticks + " , " + this.old_ticks + " , " + (this.increments * (this.ticks - this.old_ticks)) / TimeSpan.TicksPerSecond);

                this.clock += (this.increments * (this.ticks - this.old_ticks)) / 10 /*/ TimeSpan.TicksPerSecond*/; // seconds use the tickspersecond to go faster more correct but to fast...
                if ((this.ticks - this.old_ticks) > 1)
                    update = true;
            }

            this.old_ticks = this.ticks;

            return update;
        }

        public DateTime CurrentClock()
        {
            return this.dtStart.AddSeconds(this.clock);
        }

        public bool EndOfRuntime()
        {
            if (this.dtEnd.Subtract(this.CurrentClock()).TotalSeconds > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        /// <summary>
        /// Where the texture starts
        /// </summary>
        /// <param name="Date">True if date charset, flase if clock charset</param>
        /// <returns>Returns position on the texture for the Char parameter</returns>
        public OpenTK.Vector2[] CharPosition(bool Date, char TimeChar)
        {
            /*string charset = "0123456789: "; //"0123456789: -";
            float[] charmap = new float[2];
            float[] bitmapsize = new float[2];
            */
            if (Date)
            {
                charmap[0] = 40.0f;    // width
                charmap[1] = 70.0f;    // height
                bitmapsize[0] = 320.0f; // width
                bitmapsize[1] = 240.0f; // height
            }
            else
            {
                charmap[0] = 80.0f;
                charmap[1] = 140.0f;
                bitmapsize[0] = 640.0f;
                bitmapsize[1] = 480.0f;
            }

            float y = 0;
            float x = 0;
            // 0 = bottom left, 1 = top left, 2 = top right, 3 = bottom right

            //OpenTK.Vector2[] retVal = new Vector2[4];
            retVal[0] = Vector2.Zero;
            retVal[1] = Vector2.Zero;
            retVal[2] = Vector2.Zero;
            retVal[3] = Vector2.Zero;
            if (charset.Contains(TimeChar))
            {
                x = charset.IndexOf(TimeChar) * charmap[0]; // this is not safe it the character is not in the right place ie. char is missing...
                while (x >= bitmapsize[0])
                {
                    y += charmap[1];
                    x -= bitmapsize[0];
                }
                /*
                // Projection made this inacurate
                // Bottom left
                retVal[0].X = x / bitmapsize[0];
                retVal[0].Y = (y + charmap[1]) / bitmapsize[1];
                // top left
                retVal[1].X = x / bitmapsize[0];
                retVal[1].Y = y / bitmapsize[1];
                // top right
                retVal[2].X = (x + charmap[0]) / bitmapsize[0];
                retVal[2].Y = y / bitmapsize[1];
                // bottom right
                retVal[3].X = (x + charmap[0]) / bitmapsize[0];
                retVal[3].Y = (y + charmap[1]) / bitmapsize[1];
                 */

                // Bottom right
                retVal[3].X = x / bitmapsize[0];
                retVal[3].Y = (y + charmap[1]) / bitmapsize[1];
                // top right
                retVal[2].X = x / bitmapsize[0];
                retVal[2].Y = y / bitmapsize[1];
                // top left
                retVal[1].X = (x + charmap[0]) / bitmapsize[0];
                retVal[1].Y = y / bitmapsize[1];
                // bottom left
                retVal[0].X = (x + charmap[0]) / bitmapsize[0];
                retVal[0].Y = (y + charmap[1]) / bitmapsize[1];

            }
            return retVal;
        }


        public void DrawTime()
        {
            //OpenTK.Vector2[] texVert = new Vector2[4];
            string tmp = this.CurrentClock().ToLongTimeString();
            for (int i = 0; i < tmp.Length; i++)
            {
                //System.Diagnostics.Debug.WriteLine(tmp);
                retVal = this.CharPosition(false, tmp[i]);
                texTimeVert[i, 0] = retVal[0];
                texTimeVert[i, 1] = retVal[1];
                texTimeVert[i, 2] = retVal[2];
                texTimeVert[i, 3] = retVal[3];
                //System.Diagnostics.Debug.WriteLine(texTimeVert[i,0] + ", " + texTimeVert[i,1] + ", " + texTimeVert[i,2] + ", " + texTimeVert[i,3]);
            }

            GL.Enable(EnableCap.Texture2D);
            //GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, this.m_tex[1]);
            GL.Begin(BeginMode.Quads);
            for (int i = 0; i < 8/*texTimeVert.GetLength(0)*/; i++)
            {
                Vector3 v1 = new Vector3(1.60f - (i * 0.4f), 1.00f, 0.5f);
                Vector3 v2 = new Vector3(1.60f - (i * 0.4f), 0.50f, 0.5f);
                Vector3 v3 = new Vector3(1.25f - (i * 0.4f), 0.50f, 0.5f);
                Vector3 v4 = new Vector3(1.25f - (i * 0.4f), 1.00f, 0.5f);
                
                GL.TexCoord2(texTimeVert[i, 2]); GL.Vertex3(v1); // top left
                GL.TexCoord2(texTimeVert[i, 3]); GL.Vertex3(v2); // bottom left
                GL.TexCoord2(texTimeVert[i, 0]); GL.Vertex3(v3); // bottom right
                GL.TexCoord2(texTimeVert[i, 1]); GL.Vertex3(v4); // top right
                
            }

            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }


        public void DrawDate()
        {
            /*float[] lightAmbient = new float[4] { 1.0f, 1.0f, 1.0f, 1.0f };
            GL.LightModel(LightModelParameter.LightModelAmbient, lightAmbient);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);*/

            //OpenTK.Vector2[] texVert = new Vector2[4];
            string tmp = this.CurrentClock().ToShortDateString();
            for (int i = 0; i < tmp.Length; i++)
            {
                //System.Diagnostics.Debug.WriteLine(tmp);
                retVal = this.CharPosition(true, tmp[i]);
                texDateVert[i, 0] = retVal[0];
                texDateVert[i, 1] = retVal[1];
                texDateVert[i, 2] = retVal[2];
                texDateVert[i, 3] = retVal[3];
                //System.Diagnostics.Debug.WriteLine(texTimeVert[i,0] + ", " + texTimeVert[i,1] + ", " + texTimeVert[i,2] + ", " + texTimeVert[i,3]);
            }


            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            //GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, this.m_tex[0]);
            GL.Begin(BeginMode.Quads);
            for (int i = 0; i < 10/*texDateVert.GetLength(0)*/; i++)
            {
                Vector3 v1 = new Vector3(1.00f - (i * 0.20f), 0.45f, 0.5f);
                Vector3 v2 = new Vector3(1.00f - (i * 0.20f), 0.25f, 0.5f);
                Vector3 v3 = new Vector3(0.85f - (i * 0.20f), 0.25f, 0.5f);
                Vector3 v4 = new Vector3(0.85f - (i * 0.20f), 0.45f, 0.5f);
                
                /*GL.Normal3(NormalizedCrossProd(v3, v4));*/
                GL.TexCoord2(texDateVert[i, 2]); GL.Vertex3(v1); // top left
                /*GL.Normal3(NormalizedCrossProd(v4, v1));*/
                GL.TexCoord2(texDateVert[i, 3]); GL.Vertex3(v2); // bottom left
                /*GL.Normal3(NormalizedCrossProd(v1, v2));*/ 
                GL.TexCoord2(texDateVert[i, 0]); GL.Vertex3(v3); // bottom right
                /*GL.Normal3(NormalizedCrossProd(v2, v3));*/ 
                GL.TexCoord2(texDateVert[i, 1]); GL.Vertex3(v4); // top right
                
            }

            GL.End();

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            /*GL.Disable(EnableCap.Light0);
            GL.Disable(EnableCap.Lighting);*/
        }

        public void Draw()
        {
            // Time
            DrawTime();
            // Date
            DrawDate();
        }

        // normalize a vector
        private Vector3 Normal3v(Vector3 v)
        {
            Vector3 ret = new Vector3();
            double norm = System.Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);

            if (norm.Equals(0.0))
            {
                norm = 1.0;
            }

            ret.X = (float)(v.X / norm);
            ret.Y = (float)(v.Y / norm);
            ret.Z = (float)(v.Z / norm);

            return ret;
        }

        private Vector3 NormalizedCrossProd(Vector3 v1, Vector3 v2)
        {
            Vector3 ret = new Vector3();

            // Cross Product of to vertices
            ret.X = v1.Y * v2.Z - v1.Z * v2.Y;
            ret.Y = v1.Z * v2.X - v1.X * v2.Z;
            ret.Z = v1.X * v2.Y - v1.Y * v2.X;

            ret = this.Normal3v(ret);
            return ret;
        }
    }
}
