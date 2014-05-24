using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
//using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace OpenGL
{
    /// <summary>
    /// CrayFish event.
    /// </summary>
    class CrayFish : IEffect
    {
        private bool disposed = false;
        private int[] texture;
        private Vector3[] Shellfish;
        private SizeF Size;
        private float Speed;
        private float X;
        private float Y;
        private float Z;
        private long ticks = 0;
        private long oldTicks = 0;
        private MoveState CurrentMove;
        private ImageState ImageSequence;
        private int CurrentImage;
        private int CurrentImageSequence;
        private string oldDate;
        /// <summary>
        /// Where is it moving
        /// </summary>
        private enum MoveState { Intro=0, MoveRight, MoveLeft, Stop };
        /// <summary>
        /// What are we showing the image as?
        /// </summary>
        private enum ImageState { Normal = 0, NormalStop, Steroid, SteroidStop };

        /// <summary>
        /// Constructor for Crayfish
        /// </summary>
        public CrayFish()
        {
            texture = new int[8];
            texture[0] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/krafta0.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[1] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/krafta1.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[2] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/krafta2.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[3] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/krafta3.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[4] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/krafta4.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[5] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/krafta5.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[6] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/krafta6.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));
            texture[7] = Util.LoadTexture(Util.CurrentExecutionPath + "/gfx/krafta7.bmp", TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp, TextureWrapMode.Clamp, Color.FromArgb(255, 0, 255));


            Shellfish = new Vector3[4];
            Size = new SizeF(0.6f, 0.8f);
            Init();

            Shellfish[0] = new Vector3(X, Y, Z); // red
            Shellfish[1] = new Vector3(X, Y + Size.Height, Z); // blue
            Shellfish[2] = new Vector3(X + Size.Width, Y + Size.Height, Z); // green
            Shellfish[3] = new Vector3(X + Size.Width, Y, Z); // yellow

            /*Matrix4 projectionM = new Matrix4();
            GL.GetFloat(GetPName.ProjectionMatrix, out projectionM);
            Matrix4 modelviewM = new Matrix4();
            GL.GetFloat(GetPName.ModelviewMatrix, out modelviewM);
            float[] viewport = new float[4];
            GL.GetFloat(GetPName.Viewport, viewport);
            System.Drawing.Size viewM = new Size((int)viewport[2], (int)viewport[3]);
            Vector4 vec = Util.UnProject(projectionM, modelviewM, viewM, new Vector3(0, 300, 0.45f));
            Console.WriteLine(vec.Xyz.ToString());
            vec = Util.Project(new Vector4(-2.0f, 1.0f, 0.45f, 1.0f), projectionM, modelviewM, viewM);
            Console.WriteLine(vec.Xyz.ToString());*/
            /*Byte Pixel = 0;
            GL.ReadPixels(950, 600, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Byte, ref Pixel);
            Console.WriteLine(Pixel.ToString());*/
        }

        /// <summary>
        /// Initialization method
        /// </summary>
        public void Init()
        {
            Speed = Util.Rnd.Next(4000, 6200) / 1000000.0f;
            CurrentMove = MoveState.Intro;
            ImageSequence = ImageState.Normal;
            CurrentImage = 0;
            oldTicks = 0;
            CurrentImageSequence = 0;

            //X = 0.0f - Size.Width / 2.0f;
            //Y = -0.5f - Size.Height / 2.0f;
            X = (Util.Rnd.Next(-120, 120) / 100.0f) - (Size.Width / 2.0f); //0.0f - (Size.Width / 2.0f);
            Y = -1.2f - Size.Height;
            Z = 0.45f;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~CrayFish()
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
                    Util.DeleteTexture(ref texture[0]);
                    Util.DeleteTexture(ref texture[1]);
                    Util.DeleteTexture(ref texture[2]);
                    Util.DeleteTexture(ref texture[3]);
                    Util.DeleteTexture(ref texture[4]);
                    Util.DeleteTexture(ref texture[5]);
                    Util.DeleteTexture(ref texture[6]);
                    Util.DeleteTexture(ref texture[7]);
                }
                // free native resources if there are any.
                
                System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " disposed.");
                disposed = true;
            }
        }

        /// <summary>
        /// Very unsecure way of doing moving on a screen that is of unknown size in ogl window....
        /// </summary>
        public void Move()
        {
            ticks = System.DateTime.Now.Ticks / TimeSpan.TicksPerSecond;

            if (this.oldTicks != 0)
            {
                #region Timed move - disabled
                /*
                //Console.WriteLine(DateTime.Now.ToString("hh:mm:ss.ffffff") + ": " + this.ticks + "-" + this.oldTicks + "=" + (this.ticks - this.oldTicks));

                if ((this.ticks - this.oldTicks) % 3 == 2)
                {
                    CurrentImage++;
                    if (CurrentImage > 7)
                    {
                        CurrentImage = 0;
                    }
                }

                if ( 
                    (CurrentMove == 0 && (this.ticks - this.oldTicks) > 6) ||
                    (CurrentMove == 1 && (this.ticks - this.oldTicks) > 6) ||
                    (CurrentMove == 2 && (this.ticks - this.oldTicks) > 13) ||
                    (CurrentMove == 3 && (this.ticks - this.oldTicks) > 13)
                   )
                {
                    if (CurrentMove == 3)
                    {
                        CurrentMove = 1;
                    }
                    CurrentMove++;
                    oldTicks = ticks;
                    //Console.WriteLine(DateTime.Now.ToString("hh:mm:ss.ffffff") + ": change move");
                }

                
                // Make move, this is so not good to have constant sizes on edges...
                switch (CurrentMove)
                {
                    case 0:
                        if (Y < 1.0f)
                        {
                            Y += Speed;    
                        }
                        break;
                    case 3:
                    case 1:
                        if (X > -2.0f)
                        {
                            X -= Speed;
                        }
                        break;
                    case 2:
                        if (X < 2.0f)
                        {
                            X += Speed;
                        }
                        break;
                    default:
                        break;
                }
                */
                #endregion
                
                //Image selection
                if (CurrentImageSequence >= 10)
                {
                    CurrentImageSequence = 0;
                    switch (Util.Rnd.Next(0, 1000) / 100)
                    {
                        case 4: // Normal stop
                        case 3:
                        case 5:
                            ImageSequence = ImageState.NormalStop;
                            CurrentMove = MoveState.Stop;
                            break;
                        case 6: // Steroid
                        case 7:
                            ImageSequence = ImageState.Steroid;
                            CurrentMove = (MoveState)(Util.Rnd.Next(0, 999) / 500);
                            break;
                        case 8:// Steroid stop
                        case 9:
                            ImageSequence = ImageState.SteroidStop;
                            CurrentMove = MoveState.Stop;
                            break;
                        default: // Normal
                            ImageSequence = ImageState.Normal;
                            CurrentMove = (MoveState)(Util.Rnd.Next(0, 999) / 500);
                            break;
                    }
                }

                //Console.WriteLine((((this.ticks - this.oldTicks) / TimeSpan.TicksPerSecond) % 2) + ", " + ((this.ticks - this.oldTicks) / TimeSpan.TicksPerSecond));
                if (this.ticks != this.oldTicks && (
                    ( (((this.ticks - this.oldTicks) / TimeSpan.TicksPerSecond) % 2) == 0)
                   ))
                {
                    /*
                    CurrentImage++;
                    
                    if (CurrentImage > 7)
                    {
                        CurrentImage = 0;
                    }*/

                    switch (ImageSequence)
                    {
                        case ImageState.Normal: //0,1,2,3,4,5
                        case ImageState.NormalStop:
                            CurrentImage++;
                            if (CurrentImage > 5)
                            {
                                CurrentImage = 0;
                            }
                            break;
                        case ImageState.Steroid: //0,1,2,3,6,7
                        case ImageState.SteroidStop:
                            CurrentImage++;
                            if (CurrentImage > 3 && CurrentImage < 6)
                            {
                                CurrentImage = 6;
                            }
                            else if (CurrentImage > 7)
                            {
                                CurrentImage = 0;
                            }
                            break;
                        default:
                            break;
                    }

                    oldTicks = ticks;
                    CurrentImageSequence++;
                }
                

                // Movements, fixed size :(
                switch (CurrentMove)
                {
                    case MoveState.Intro:
                        if (Y + Size.Height < 0.0f)
                        {
                            Y += 0.003f;
                        }
                        else
                        {
                            if ((Util.Rnd.Next(0, 1000) / 500.0) > 1.0)
                            {
                                CurrentMove = MoveState.MoveRight;
                            }
                            else
                            {
                                CurrentMove = MoveState.MoveLeft;
                            }
                            
                        }
                        break;
                    case MoveState.MoveRight:
                        if (X - Size.Width > -2.0f)
                        {
                            X -= Speed;
                        }
                        else
                        {
                            CurrentMove = MoveState.MoveLeft;
                        }
                        break;
                    case MoveState.MoveLeft:
                        if (X + Size.Width < 1.5f)
                        {
                            X += Speed;
                        }
                        else
                        {
                            CurrentMove = MoveState.MoveRight;
                        }
                        break;
                    default: //MoveState.Stop
                        break;
                }
            }
            else if (oldTicks == 0)
            {
                oldTicks = ticks;
            }
        }

        /// <summary>
        /// Draw to screen
        /// </summary>
        /// <param name="Date">Current date</param>
        public void Draw(string Date)
        {
            // Spiders that draws after eachother and end ontop durring run will be hidden...
            if (oldDate != Date)
            {
                Init();
                oldDate = Date;
            }

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, texture[CurrentImage]);

            GL.Begin(BeginMode.Quads);
            // Fix this to be dynamic, I don't have the feeling for it currently...

                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(X, Y, Z); // bottom right 
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(X, Y + Size.Height, Z); // Top right
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(X + Size.Width, Y + Size.Height, Z);// top left
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(X + Size.Width, Y, Z); // bottom left

            GL.End();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            Move();
        }

    }
}

