using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace OpenGL
{
    /// <summary>
    /// Star class
    /// </summary>
    class Star : IEffect
    {
        private bool disposed = false;
        private float x;
        private float y;
        private float z;
        private float startx;
        private float starty;
        private float startz;
        private float speed;
        private float width;
        private bool MoveLeft;
        private bool MoveUp;
        private Color StarColour;

        /// <summary>
        /// Constructor for Star
        /// </summary>
        /// <param name="sX">Start X-position</param>
        /// <param name="sY">Start Y-position</param>
        /// <param name="sZ">Start Z-position</param>
        /// <param name="sSpeed">Speed in Z-axis</param>
        /// <param name="left">?</param>
        /// <param name="up">?</param>
        /// <param name="size">Size of star</param>
        public Star(float sX, float sY, float sZ, float sSpeed, bool left, bool up, float size=1.0f)
        {
            startx = x = sX;
            starty = y = sY;
            startz = z = sZ;
            speed = sSpeed;
            width = size;
            MoveLeft = left;
            MoveUp = up;
            StarColour = Color.White;
        }

        #region Dispose
        /// <summary>
        /// Destructor
        /// </summary>
        ~Star()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            //base.Finalize();
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing">Is it disposing?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // free managed resources
                }
                // free native resources if there are any.
                disposed = true;
               // System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }
        #endregion

        /// <summary>
        /// Property returning a float of the X-position
        /// </summary>
        public float X
        {
            get
            {
                return x;
            }
        }

        /// <summary>
        /// Property returning a float of the Y-position
        /// </summary>
        public float Y
        {
            get
            {
                return y;
            }
        }

        /// <summary>
        /// Property returning a float of the Z-position
        /// </summary>
        public float Z
        {
            get
            {
                return z;
            }
        }

        /// <summary>
        /// Property returning a flot with the speed movment of star
        /// </summary>
        public float Speed
        {
            get
            {
                return speed;
            }
        }

        /// <summary>
        /// Property returning a float with the size of star
        /// </summary>
        public float Size
        {
            get
            {
                return width;
            }
        }

        /// <summary>
        /// Convert X, Y, Z-positions to a Vector3
        /// </summary>
        /// <returns>Vector3 with positional information</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Set the colour of the Star
        /// </summary>
        /// <param name="StarC">What color is the star</param>
        public void SetColour(Color StarC)
        {
            StarColour = StarC;
        }

        /// <summary>
        /// Draw Star effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.Disable(EnableCap.Texture2D);
            GL.PushAttrib(AttribMask.CurrentBit);
            
            GL.PointSize(width);
            GL.Begin(BeginMode.Points);
            GL.Color4(StarColour);
            GL.Vertex3(x, y, z); // bottom right

            GL.End();
            GL.PopAttrib();
            //GL.MatrixMode(MatrixMode.Modelview);
            Move();
        }

        /// <summary>
        /// Move Star
        /// </summary>
        public void Move()
        {

            //x = (MoveLeft ? x - speed : x + speed);
            //x += speed;
            //y = (MoveUp ? y - speed : y + speed);
            z -= speed*4;

            if (/*z > 0.49f ||*/ z < 1.5f)
            {
                z = 4.9f;
                x = Util.Rnd.Next(-25, 30) / 10.0f; 
                y = Util.Rnd.Next(-20, 20) / 10.0f;
            }
            /*float[] viewport = Util.GetViewport();
            Matrix4 mvp = Util.GetMVP();
            Vector3 pointV = Vector3.Transform(new Vector3(x,y,z), mvp);
            pointV.Normalize(); //can normalize it to get the same?!
            //float X = (pointV.X / pointV.Z);
            //float Y = (pointV.Y / pointV.Z);
            float X = pointV.X;
            float Y = pointV.Y;
            X = ((pointV.X) + 1) * viewport[2] / 2;
            Y = ((pointV.Y) + 1) * viewport[3] / 2;
            //System.Diagnostics.Debug.WriteLine(x + ", " + y + ". " + X + ", " + Y);
            Vector2 pointV2D = new Vector2(X, Y);

            if (pointV2D.X >= viewport[2])
            {
                x = startx;
                z = startz;
            }
            else if (pointV2D.X <= viewport[0])
            {
                x = startx;
                z = startz;
            }
            if (pointV2D.Y >= viewport[3])
            {
                y = starty;
                z = startz;
            }
            else if (pointV2D.Y <= viewport[1])
            {
                y = starty;
                z = startz;
            }
            */
            //System.Diagnostics.Debug.WriteLine(pointV2D.X + ", " + pointV2D.Y);
        }
    
    } //end Start

    /// <summary>
    /// Starfield effect
    /// </summary>
    class Starfield : IEffect
    {
        private bool disposed = false;
        private Star[] Stars;
        private string oldDate;
        uint vboHandle, voaHandle;
        Vector3[] vertices;
        /*private float minDepth;
        private float maxDepth;*/
        /*int vShader;
        int vsProgram;
        int locMVP;
        int locPosition;
        Matrix4 MVP;
        private string VertexShader = @"
            #version 130
            // incoming vector
            //in vec3 vVertex;
            // modelview & projection matrix as one
            uniform mat4 mvpMatrix;

            void main(void)
            {
                // Don't forget to transform the geometry!
                //vVertex.z += 0.00025f;
                //gl_Vertex.z += 0.00025f;
                gl_Position = mvpMatrix * gl_Vertex;
            }
            ";
        */

        /// <summary>
        /// Constructor for Starfield effect
        /// </summary>
        /// <param name="AmountOfStars">Integer numbers of stars to be drawn</param>
        public Starfield(int AmountOfStars = 100)
        {
            Stars = new Star[AmountOfStars];
            //minDepth = 0.00f;
            //maxDepth = 5.0f;
            for (int i = 0; i < AmountOfStars; i++)
            {
                //Stars[i] = new Star((3 - (rnd.Next(int.MinValue, int.MaxValue) % 20)) * 0.1f, (3 - (rnd.Next(int.MinValue, int.MaxValue) % 15)) * 0.1f, Util.Rnd.Next(16, 49) / 10.0f, 0.1f + 0.9f * rnd.Next(0, 32 * 1024) / (32 * 1024 +1) /*0.00025f*/, (rnd.Next(1, 100) <= 50 ? false : true), (rnd.Next(1, 100) <= 50 ? false : true), 1.0f);
                Stars[i] = new Star(Util.Rnd.Next(-25, 30) / 10.0f, Util.Rnd.Next(-20, 20) / 10.0f, Util.Rnd.Next(16, 49) / 10.0f, 
                    Util.Rnd.Next(50, 100) / 100000.0f /*0.00025f*/, (Util.Rnd.Next(1, 100) % 2 == 0 ? false : true), (Util.Rnd.Next(1, 100) % 2 == 0 ? false : true), 2.0f);
                System.Threading.Thread.Sleep(1);
            }

            vertices = new Vector3[AmountOfStars];
            GL.GenBuffers(1, out vboHandle);
            GL.GenVertexArrays(1, out voaHandle);

            for (int i = 0; i < AmountOfStars; i++)
            {
                //
                vertices[i] = Stars[i].ToVector3();
            }
            
            /*GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.DynamicDraw);
            */
            GL.BindVertexArray(voaHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, voaHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StaticDraw);
            
            //GL.GetVertexAttribPointer(0, VertexAttribPointerParameter.ArrayPointer, 
            /*MVP = Util.GetMVP();
            vsProgram = GL.CreateProgram();
            vShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vShader, VertexShader);
            locMVP = GL.GetUniformLocation(vShader, "mvpMatrix");
            locPosition = GL.GetAttribLocation(vShader, "vVertex");
            GL.CompileShader(vShader);
            GL.AttachShader(vsProgram, vShader);
            GL.LinkProgram(vsProgram);*/
        }

        #region Dispose
        /// <summary>
        /// Destructor
        /// </summary>
        ~Starfield()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            //base.Finalize();
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing">Is it disposing?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    for (int i = 0; i < Stars.Length; i++)
                    {
                        Stars[i].Dispose();
                    }
                    Stars = null;
                    vertices = null;
                    GL.DeleteBuffers(1, ref vboHandle);
                    GL.DeleteVertexArrays(1, ref voaHandle);
                    /*GL.DetachShader(vsProgram, vShader);
                    GL.DeleteShader(vShader);
                    GL.DeleteProgram(vsProgram);*/
                }
                // free native resources if there are any.
                disposed = true;
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
            }
        }
        #endregion

        /// <summary>
        /// Set the colour of the Star
        /// </summary>
        /// <param name="StartColour">What color is the star</param>
        public void SetColour(Color StartColour)
        {
            foreach (Star s in Stars)
            {
                s.SetColour(StartColour);
            }
        }

        /// <summary>
        /// Draw Starfield effect on screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            if (oldDate != Date)
            {
                switch (Util.Rnd.Next(0,1000)/100) // 0 to 10
                { 
                    case 0:
                        SetColour(Color.Purple);
                        break;
                    case 1:
                        SetColour(Color.Yellow);
                        break;
                    case 2:
                        SetColour(Color.Red);
                        break;
                    case 3:
                        SetColour(Color.Green);
                        break;
                    case 4:
                        SetColour(Color.LightPink);
                        break;
                    case 5:
                        SetColour(Color.LightYellow);
                        break;
                    case 6:
                        SetColour(Color.Magenta);
                        break;
                    case 7:
                        SetColour(Color.MediumPurple);
                        break;
                    case 8:
                        SetColour(Color.MediumVioletRed);
                        break;
                    case 9:
                        SetColour(Color.PaleVioletRed);
                        break;
                    default:
                        SetColour(Color.White);
                        break;
                }
                oldDate = Date;
            }
            // 700-900 fps
            foreach (Star s in Stars)
            {
                s.Draw(Date);
            }

            /*GL.PushAttrib(AttribMask.CurrentBit);
            GL.Color4(Color.White);
            GL.Begin(BeginMode.Points);
            // about 400-700 fps
            for (int i = 0; i < Stars.Length; i++)
            {
                GL.PointSize(Stars[i].Size);
                GL.Vertex3(Stars[i].X, Stars[i].Y, Stars[i].Z); // bottom right
            }
            // about 400-600 fps
            foreach (Star s in Stars)
            {
                GL.PointSize(s.Size);
                GL.Vertex3(s.X, s.Y, s.Z); 
            }
            GL.End();
            GL.PopAttrib();
           */

            // about 1600-1700fps
            /*GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StreamDraw); // mem drain...
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(BeginMode.Points, 0, vertices.Length);
            GL.DisableVertexAttribArray(0);*/
            
            // 1500-1700 fps
            /*GL.BindVertexArray(voaHandle);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StreamDraw); // mem drain...
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.DrawArrays(BeginMode.Points, 0, vertices.Length);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);*/
            
            /*Vector3 zmove; 
            Vector3 zmovereset = new Vector3(0.0f, 0.0f, 4.5f);
            for (int i = 0; i < vertices.Length; i++)
            {

                if (vertices[i].Z < 0.0f)
                {
                    Vector3.Add(ref vertices[i], ref zmovereset, out vertices[i]);
                    
                }
                else
                {
                    zmove = new Vector3(0.0f, 0.0f, Stars[i].Speed);
                    Vector3.Add(ref vertices[i], ref zmove, out vertices[i]);
                    
                }
                
            }*/
            // 1500-1700 fps, static star need to find a way to find the changed data in the shader
            /*GL.UseProgram(vsProgram);
            GL.UniformMatrix4(locMVP, false, ref MVP);
            GL.EnableVertexAttribArray(0);
            //GL.EnableVertexAttribArray(locPosition);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StreamDraw); // mem drain...
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //GL.VertexAttribPointer(locPosition, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(BeginMode.Points, 0, vertices.Length);
            //GL.EnableVertexAttribArray(locPosition);
            GL.DisableVertexAttribArray(0);
            GL.UseProgram(0);*/
        }
    }//end Startfield
}//end namespace
