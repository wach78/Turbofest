﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace OpenGL
{
    class Star : IEffect
    {
        #region Suppress 414 warning variable set but not used
#pragma warning disable 414
        #endregion
        private bool disposed = false;
        private float x;
        private float y;
        private float z;
        private float speed;
        private float width;
        private bool MoveLeft;
        private bool MoveUp;

        public Star(float sX, float sY, float sZ, float sSpeed, bool left, bool up, float size=1.0f)
        {
            x = sX;
            y = sY;
            z = sZ;
            speed = sSpeed;
            width = size;
            MoveLeft = left;
            MoveUp = up;
        }

        #region Dispose
        ~Star()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            //base.Finalize();
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources if there are any.
            disposed = true;
        }
        #endregion

        public float X
        {
            get
            {
                return x;
            }
        }

        public float Y
        {
            get
            {
                return y;
            }
        }

        public float Z
        {
            get
            {
                return z;
            }
        }

        public float Speed
        {
            get
            {
                return speed;
            }
        }

        public float Size
        {
            get
            {
                return width;
            }
        }

        public void Draw(string Date)
        {
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.Disable(EnableCap.Texture2D);
            GL.PushAttrib(AttribMask.CurrentBit);
            
            GL.PointSize(width);
            GL.Begin(BeginMode.Points);
            GL.Color4(Color.White);
            GL.Vertex3(x, y, z); // bottom right

            GL.End();
            GL.PopAttrib();
            //GL.MatrixMode(MatrixMode.Modelview);
            Move();
        }

        public void Move()
        {

            //x = (MoveLeft ? x - speed : x + speed);
            //y = (MoveUp ? y - speed : y + speed);
            z -= speed;
            if (/*z > 0.49f ||*/ z < 0.0f)
            {
                z = 0.49f;
            }
        }
    
    } //end Start

    class Starfield : IEffect
    {
        #region Suppress 414 warning variable set but not used
#pragma warning disable 414
        #endregion
        private bool disposed = false;
        private Star[] Stars;
        private float minDepth;
        private float maxDepth;

        uint vboHandle;
        Vector3[] vertices;
        int vShader;
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

        public Starfield(int AmountOfStars = 100)
        {
            Random rnd = new Random(/*(int)(DateTime.Now.Ticks / 10000) + i*/);
            Stars = new Star[AmountOfStars];
            minDepth = 1.01f;
            maxDepth = 1.5f;
            for (int i = 0; i < AmountOfStars; i++)
            {
                Stars[i] = new Star(/*0.0f, 0.0f, 0.0f*/ (3 - (rnd.Next(int.MinValue, int.MaxValue) % 20)) * 0.1f, (3 - (rnd.Next(int.MinValue, int.MaxValue) % 15)) * 0.1f, 0.0f, rnd.Next(1, 50) / 100000.0f /*0.0025f*/, (rnd.Next(1, 100) <= 50 ? false : true), (rnd.Next(1, 100) <= 50 ? false : true), 1.0f);
            }
            vertices = new Vector3[AmountOfStars];
            GL.GenBuffers(1, out vboHandle);
            for (int i = 0; i < AmountOfStars; i++)
            {
                vertices[i] = new Vector3(Stars[i].X, Stars[i].Y, Stars[i].Z);
            }
            //GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);
            //GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.DynamicDraw);
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
        ~Starfield()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            //base.Finalize();
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
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
                /*GL.DetachShader(vsProgram, vShader);
                GL.DeleteShader(vShader);
                GL.DeleteProgram(vsProgram);*/
            }
            // free native resources if there are any.
            disposed = true;
        }
        #endregion

        public void Draw(string Date)
        {
            foreach (Star s in Stars)
            {
                s.Draw(Date);
            }

            /*GL.PushAttrib(AttribMask.CurrentBit);
            GL.Color4(Color.White);
            GL.Begin(BeginMode.Points);
            // loss about 1000 fps
            for (int i = 0; i < Stars.Length; i++)
            {
                GL.PointSize(Stars[i].Size);
                GL.Vertex3(Stars[i].X, Stars[i].Y, Stars[i].Z); // bottom right
            }
            // loss about 1200 fps
            foreach (Star s in Stars)
            {
                GL.PointSize(s.Size);
                GL.Vertex3(s.X, s.Y, s.Z); // bottom right
            }
            GL.End();
            GL.PopAttrib();*/
           
            // gains about 2000 fps :D
            /*GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StreamDraw); // mem drain...
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.DrawArrays(BeginMode.Points, 0, vertices.Length);

            GL.DisableVertexAttribArray(0);
            */
            /*Vector3 zmove; 
            Vector3 zmovereset = new Vector3(0.0f, 0.0f, 0.49f);
            for (int i = 0; i < vertices.Length; i++)
            {

                if (vertices[i].Z < 0.0f)
                {
                    Vector3.Add(ref vertices[i], ref zmovereset, out vertices[i]);
                    
                }
                else
                {
                    zmove = new Vector3(0.0f, 0.0f, Stars[i].Speed*-1);
                    Vector3.Add(ref vertices[i], ref zmove, out vertices[i]);
                    
                }
                
            }*/
            // gains about 2000 fps, static star need to find a way to find the changed data in the shader
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
            GL.UseProgram(0);
            */
        }

    }//end Startfield
}//end namespace