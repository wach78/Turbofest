using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Drawing.Imaging;

using OpenTK;
//using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    public class GLWindow : OpenTK.GameWindow
    {
        const string TITLE = "Project X";
        const int WIDTH = 1024;
        const int HEIGHT = 768;
        bool blnFullscreen;
        bool blnPointDraw;
        bool blnWireFrameDraw;
        bool blnFog;
        bool blnLight;
        uint[] texture;
        /*double xScroll = 0.0;
        double yScroll = 0.0;*/
        PartyClock pc;
        Chess chess;
        Text2D text;

        Particle particle;
        SuneAnimation sune;
        Dif dif;
        Christmas xmas;
        Semla s;
        Fbk f;
        TurboLogo tl;

        // Test for sound
        Sound snd;
        System.Threading.Thread soundThread;


        System.Xml.Linq.XDocument m_events; // this needs to be fixed and stuff...

        // OpenGL version after 3.0 needs there own matrix libs so we need to create them if we run over 3.0!!! if ser major and minor to 0 we can get around it?!
        public GLWindow(System.Xml.Linq.XDocument Events, string runtime) : base(WIDTH, HEIGHT, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(32), 24, 8, 0)/*OpenTK.Graphics.GraphicsMode.Default*/, TITLE, OpenTK.GameWindowFlags.Default, OpenTK.DisplayDevice.Default, 0, 0, OpenTK.Graphics.GraphicsContextFlags.Debug | OpenTK.Graphics.GraphicsContextFlags.Default) {
            //Mouse.Move += new EventHandler<MouseMoveEventArgs>(OnMouseMove);
            
            if (runtime.Length < 28)
            {
                throw new Exception("Error in runtime, it is to short");
            }

            Keyboard.KeyDown += OnKeyboardKeyDown;

            this.blnFullscreen = false;
            this.blnPointDraw = false;
            this.blnWireFrameDraw = false;
            this.blnFog = true;
            this.blnLight = false;
            this.texture = new uint[3];
            this.m_events = Events;
            DateTime dtStart = DateTime.Parse(runtime.Substring(0, 10)/*"2007-09-23"*/);
            DateTime dtEnd = DateTime.Parse(runtime.Substring(10, 10)/*"2008-03-20"*/);
            TimeSpan tsDiff = dtEnd.Subtract(dtStart);
            this.pc = new PartyClock(dtStart, dtEnd, int.Parse(runtime.Substring(20))/*4*/);
            this.chess = new Chess();
            this.text = new Text2D(100,100);
            this.particle = new Particle();
            Console.WriteLine(dtStart + "," + dtStart.Ticks + "," + tsDiff.Ticks + "," + tsDiff.TotalSeconds);
            Console.WriteLine(GL.GetString(StringName.Version)+ "," + GL.GetString(StringName.Renderer) + "," + GL.GetString(StringName.Extensions));
            int value;
            GL.GetInteger(GetPName.MajorVersion, out value);
            Console.WriteLine(value+ " , " + GL.GetString(StringName.ShadingLanguageVersion));

            Console.WriteLine(Util.MaxTextures +":"+Util.CurrentUsedTextures + ", " + Util.MaxBuffers);
            //sune = new SuneAnimation();
            //dif = new Dif();
           // xmas = new Christmas();
          //  s = new Semla();
           // f = new Fbk();
            tl = new TurboLogo();

            snd = new Sound();
            snd.SetAudioBuffer("FBK");
            soundThread = new System.Threading.Thread(new System.Threading.ThreadStart(snd.Play));
            //soundThread.Start();
            snd.Play("FBK");
        }

        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // Move this GL.Enable in to the drawing method as this is only to be done in there
            //GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest); // to enable depth but needs reconfiguring
            GL.DepthRange(1.0, -1.0); // needed for DepthTest to show graphics
            
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            
            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.AlphaTest);
            
            //GL.Fog(FogParameter.FogIndex, 3.0f);
            //GL.FogCoord();

            
            // Environment light
            //GL.Enable(EnableCap.Normalize); // if we don't use calculated normals we can use this...
            float[] lightAmbient = new float[4] { 0.5f, 0.5f, 0.5f, 1.0f };
            GL.LightModel(LightModelParameter.LightModelAmbient, lightAmbient);
            /*GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);*/
            

            GL.Enable(EnableCap.StencilTest);
            //GL.Enable(EnableCap.VertexArray);
            // end of GL.Enable

            this.VSync = OpenTK.VSyncMode.Adaptive; // this is to make it possible to run faster then refreshrate on screen...
            /*
            this.TargetRenderFrequency = 200.0; // forcing to update at 50 hz
            this.TargetUpdateFrequency = 200.0; // forcing to update at 50 hz
            */
            //System.Drawing.Bitmap bitmapDate = new System.Drawing.Bitmap(System.IO.Path.GetFullPath("../../gfx/clockfont40x70.bmp"));
            //System.Drawing.Bitmap bitmapClock = new System.Drawing.Bitmap(System.IO.Path.GetFullPath("../../gfx/clockfont80x140.bmp"));
            //System.Drawing.Bitmap bitmapChess = new System.Drawing.Bitmap(System.IO.Path.GetFullPath("../../gfx/stripes.bmp"));
            //System.Drawing.Bitmap bitmapChess = new System.Drawing.Bitmap(System.IO.Path.GetFullPath("../../gfx/chess.gif"));

            //Console.WriteLine(System.IO.Path.GetFullPath("../../gfx/clockfont.bmp"));
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f); // Set the clear color to a black

            //GL.ActiveTexture(TextureUnit.Texture0);
            GL.GenTextures(3, texture);
            #region textures - Old
            /*GL.GenTextures(1, out texture1);
            GL.GenTextures(1, out texture2);
            GL.GenTextures(1, out texture3);*/
            /*
            // Date font
            //GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture[0]);
            System.Drawing.Imaging.BitmapData data = bitmapDate.LockBits(new System.Drawing.Rectangle(0, 0, bitmapDate.Width, bitmapDate.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmapDate.UnlockBits(data);
            data = null;
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            *//*
            // clock font
            //GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, texture[1]);

            data = bitmapClock.LockBits(new System.Drawing.Rectangle(0, 0, bitmapClock.Width, bitmapClock.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmapClock.UnlockBits(data);
            data = null;
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            */
            
            // stripes
            //GL.ActiveTexture(TextureUnit.Texture2);
            /*GL.BindTexture(TextureTarget.Texture2D, texture[2]);
            System.Drawing.Imaging.BitmapData data = bitmapChess.LockBits(new System.Drawing.Rectangle(0, 0, bitmapChess.Width, bitmapChess.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmapChess.UnlockBits(data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            */
            //GL.ActiveTexture(TextureUnit.Texture0);
            //GL.BindTexture(TextureTarget.Texture2D, 0);
            #endregion



        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            //this.texture = null;
            //sound.Stop(); // not done when it should...
            GL.DeleteBuffers(3, this.texture);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.LoadIdentity();
            // Set the viewport to match the new width and height of the window
            //GL.LoadIdentity();
            //GL.Ortho(0, ClientRectangle.Width, 0, ClientRectangle.Height, -1.0f, 64.0f);
            //GL.Ortho(-1 * (ClientRectangle.Width / 2), (ClientRectangle.Width / 2), -1 * (ClientRectangle.Height / 2), (ClientRectangle.Height / 2), -1.0f, 64.0f);
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            
            //GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            Matrix4 matProj = Matrix4.CreatePerspectiveFieldOfView(OpenTK.MathHelper.DegreesToRadians(60.0f), (float)(DisplayDevice.Default.Width / (double)DisplayDevice.Default.Height), 0.1f, 5.0f);
            GL.LoadMatrix(ref matProj); // need to invers this to make the camera on the right side of the object?
            
            /*Matrix4 ortho_projection = Matrix4.CreateOrthographicOffCenter(0.0f, Width, Height, 0, -1.0f, 1.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref ortho_projection);*/
            
            GL.MatrixMode(MatrixMode.Modelview);
           // GL.PopMatrix();
            GL.LoadIdentity();
            Matrix4 matLook = Matrix4.LookAt(new Vector3(0.0f, 0.0f, -1.3f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            //Matrix4 matLook = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.LoadMatrix(ref matLook);
            //GL.Rotate(180, 0.0, 0.0, 1.0);
            //GL.Scale(2.0f, 2.0f, 2.0f); // scale to fit window better if z is -2.3 on eye placement
            
        }

        protected override void OnUpdateFrame(OpenTK.FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            
            //add frame render stuff...
        }


        /*OpenTK.Vector2[,] texTimeVert = new Vector2[8, 4];
        OpenTK.Vector2[,] texDateVert = new Vector2[10, 4];*/

        // The rendering for the scene happens here.
        protected override void OnRenderFrame(OpenTK.FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.StencilBufferBit | ClearBufferMask.DepthBufferBit); // Clear the OpenGL color buffer
            if (!pc.EndOfRuntime())
            {
                //Console.WriteLine(this.RenderFrequency);
                pc.updateClock();
                #region Comment
                /*if (pc.updateClock())
                {


                    //DateTime xx = pc.dtStart.AddSeconds(x);
                    OpenTK.Vector2[] texVert = new Vector2[4];
                    Console.WriteLine(DateTime.Now + " , " + pc.CurrentClock() + ", " + this.RenderFrequency);
                    string tmp = pc.CurrentClock().ToLongTimeString();
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        //Console.WriteLine(tmp);
                        
                        texVert = pc.CharPosition(false, tmp[i]);
                        texTimeVert[i, 0] = texVert[0];
                        texTimeVert[i, 1] = texVert[1];
                        texTimeVert[i, 2] = texVert[2];
                        texTimeVert[i, 3] = texVert[3];
                        //Console.WriteLine(texTimeVert[i,0] + ", " + texTimeVert[i,1] + ", " + texTimeVert[i,2] + ", " + texTimeVert[i,3]);
                    }

                    tmp = pc.CurrentClock().ToShortDateString();
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        //Console.WriteLine(tmp);
                        //OpenTK.Vector2[] texVert = new Vector2[4];
                        texVert = pc.CharPosition(true, tmp[i]);
                        texDateVert[i, 0] = texVert[0];
                        texDateVert[i, 1] = texVert[1];
                        texDateVert[i, 2] = texVert[2];
                        texDateVert[i, 3] = texVert[3];
                        //Console.WriteLine(tmp[i] + ", " +texDateVert[i, 0] + ", " + texDateVert[i, 1] + ", " + texDateVert[i, 2] + ", " + texDateVert[i, 3]);
                    }
                }
                */
                //addd your stuff here?!
                /*OpenTK.Box2 box1 = new Box2(10, 10, 10, 10);
                OpenTK.Vector3[] box2 = new Vector3[4];
                box2[0] = new Vector3(10, 10, 1);
                box2[1] = new Vector3(20, 10, 1);
                box2[2] = new Vector3(20, 20, 1);
                box2[3] = new Vector3(10, 20, 1);*/
                #endregion
                GL.MatrixMode(MatrixMode.Modelview);
                #region Comment
                //GL.LoadIdentity();
                //Matrix4 mv = Matrix4.LookAt(Vector3.Zero, Vector3.UnitY, Vector3.UnitZ);
                //GL.LoadMatrix(ref mv);
                //GL.Enable(EnableCap.Texture2D);
                

                /*GL.BindTexture(TextureTarget.Texture2D, texture[0]);
                GL.TexCoord2(0.0f, 0.295f); GL.Vertex3(-0.6f, -0.4f, 1.0f); // bottom left
                GL.TexCoord2(0.125f, 0.295f); GL.Vertex3(0.6f, -0.4f, 1.0f); // bottom right
                GL.TexCoord2(0.125f, 0.0f); GL.Vertex3(0.6f, 0.4f, 1.0f); // top right
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-0.6f, 0.4f, 1.0f); // top left*/

                /*GL.BindTexture(TextureTarget.Texture2D, texture[0]);
                GL.TexCoord2(0.0f, 0.295f); GL.Vertex3(-0.6f, -0.4f, 1.0f); // bottom left
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-0.6f, 0.4f, 1.0f); // top left
                GL.TexCoord2(0.125f, 0.0f); GL.Vertex3(0.6f, 0.4f, 1.0f); // top right
                GL.TexCoord2(0.125f, 0.295f); GL.Vertex3(0.6f, -0.4f, 1.0f); // bottom right*/
                #endregion
                // Time
                pc.DrawTime();
                #region Draw Time - Old
                /*GL.Enable(EnableCap.Texture2D);
                //GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, texture[0]);
                GL.Begin(BeginMode.Quads);
                // H
                //GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, texture[0]);
                GL.TexCoord2(texTimeVert[0, 0]); GL.Vertex3(-0.85f, 0.65f, 1.0f); // bottom left
                GL.TexCoord2(texTimeVert[0, 1]); GL.Vertex3(-0.85f, 0.95f, 1.0f); // top left
                GL.TexCoord2(texTimeVert[0, 2]); GL.Vertex3(-0.60f, 0.95f, 1.0f); // top right
                GL.TexCoord2(texTimeVert[0, 3]); GL.Vertex3(-0.60f, 0.65f, 1.0f); // bottom right
                // H
                //GL.BindTexture(TextureTarget.Texture2D, texture[0]);
                GL.TexCoord2(texTimeVert[1, 0]); GL.Vertex3(-0.60f, 0.65f, 1.0f); // bottom left
                GL.TexCoord2(texTimeVert[1, 1]); GL.Vertex3(-0.60f, 0.95f, 1.0f); // top left
                GL.TexCoord2(texTimeVert[1, 2]); GL.Vertex3(-0.35f, 0.95f, 1.0f); // top right
                GL.TexCoord2(texTimeVert[1, 3]); GL.Vertex3(-0.35f, 0.65f, 1.0f); // bottom right
                // :
                //GL.BindTexture(TextureTarget.Texture2D, texture[0]);
                GL.TexCoord2(texTimeVert[2, 0]); GL.Vertex3(-0.35f, 0.65f, 1.0f); // bottom left
                GL.TexCoord2(texTimeVert[2, 1]); GL.Vertex3(-0.35f, 0.95f, 1.0f); // top left
                GL.TexCoord2(texTimeVert[2, 2]); GL.Vertex3(-0.25f, 0.95f, 1.0f); // top right
                GL.TexCoord2(texTimeVert[2, 3]); GL.Vertex3(-0.25f, 0.65f, 1.0f); // bottom right
                // M
                //GL.BindTexture(TextureTarget.Texture2D, texture[0]);
                GL.TexCoord2(texTimeVert[3, 0]); GL.Vertex3(-0.25f, 0.65f, 1.0f); // bottom left
                GL.TexCoord2(texTimeVert[3, 1]); GL.Vertex3(-0.25f, 0.95f, 1.0f); // top left
                GL.TexCoord2(texTimeVert[3, 2]); GL.Vertex3(0.00f, 0.95f, 1.0f); // top right
                GL.TexCoord2(texTimeVert[3, 3]); GL.Vertex3(0.00f, 0.65f, 1.0f); // bottom right
                // M
                //GL.BindTexture(TextureTarget.Texture2D, texture[0]);
                GL.TexCoord2(texTimeVert[4, 0]); GL.Vertex3(0.00f, 0.65f, 1.0f); // bottom left
                GL.TexCoord2(texTimeVert[4, 1]); GL.Vertex3(0.00f, 0.95f, 1.0f); // top left
                GL.TexCoord2(texTimeVert[4, 2]); GL.Vertex3(0.25f, 0.95f, 1.0f); // top right
                GL.TexCoord2(texTimeVert[4, 3]); GL.Vertex3(0.25f, 0.65f, 1.0f); // bottom right
                // :
                //GL.BindTexture(TextureTarget.Texture2D, texture[0]);
                GL.TexCoord2(texTimeVert[5, 0]); GL.Vertex3(0.25f, 0.65f, 1.0f); // bottom left
                GL.TexCoord2(texTimeVert[5, 1]); GL.Vertex3(0.25f, 0.95f, 1.0f); // top left
                GL.TexCoord2(texTimeVert[5, 2]); GL.Vertex3(0.35f, 0.95f, 1.0f); // top right
                GL.TexCoord2(texTimeVert[5, 3]); GL.Vertex3(0.35f, 0.65f, 1.0f); // bottom right
                // S
                //GL.BindTexture(TextureTarget.Texture2D, texture[0]);
                GL.TexCoord2(texTimeVert[6, 0]); GL.Vertex3(0.35f, 0.65f, 1.0f); // bottom left
                GL.TexCoord2(texTimeVert[6, 1]); GL.Vertex3(0.35f, 0.95f, 1.0f); // top left
                GL.TexCoord2(texTimeVert[6, 2]); GL.Vertex3(0.60f, 0.95f, 1.0f); // top right
                GL.TexCoord2(texTimeVert[6, 3]); GL.Vertex3(0.60f, 0.65f, 1.0f); // bottom right
                // S
                //GL.BindTexture(TextureTarget.Texture2D, texture[0]);
                GL.TexCoord2(texTimeVert[7, 0]); GL.Vertex3(0.60f, 0.65f, 1.0f); // bottom left
                GL.TexCoord2(texTimeVert[7, 1]); GL.Vertex3(0.60f, 0.95f, 1.0f); // top left
                GL.TexCoord2(texTimeVert[7, 2]); GL.Vertex3(0.85f, 0.95f, 1.0f); // top right
                GL.TexCoord2(texTimeVert[7, 3]); GL.Vertex3(0.85f, 0.65f, 1.0f); // bottom right

                GL.End();
                GL.Disable(EnableCap.Texture2D);*/
                
                //GL.BindTexture(TextureTarget.Texture2D, 0);
                //Same result as above if 1.0f is used instead!
                /*GL.BindTexture(TextureTarget.Texture2D, texture[0]);
                GL.TexCoord2(0, 1); GL.Vertex3(-0.6f, -0.4f, 1.0f);
                GL.TexCoord2(1, 1); GL.Vertex3(0.6f, -0.4f, 1.0f);
                GL.TexCoord2(1, 0); GL.Vertex3(0.6f, 0.4f, 1.0f);
                GL.TexCoord2(0, 0); GL.Vertex3(-0.6f, 0.4f, 1.0f);*/
                #endregion
                
                // Date
                pc.DrawDate();
                #region Draw Date - Old
                // Y
                //GL.Enable(EnableCap.Texture2D);
                //GL.ActiveTexture(TextureUnit.Texture1);
                /*GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, texture[1]);
                GL.Begin(BeginMode.Quads);
                GL.TexCoord2(texDateVert[0, 0]); GL.Vertex3(-0.25f, 0.45f, 1.0f); // bottom left
                GL.TexCoord2(texDateVert[0, 1]); GL.Vertex3(-0.25f, 0.60f, 1.0f); // top left
                GL.TexCoord2(texDateVert[0, 2]); GL.Vertex3(-0.2f, 0.60f, 1.0f); // top right
                GL.TexCoord2(texDateVert[0, 3]); GL.Vertex3(-0.2f, 0.45f, 1.0f); // bottom right
                // Y
                //GL.BindTexture(TextureTarget.Texture2D, texture[1]);
                GL.TexCoord2(texDateVert[1, 0]); GL.Vertex3(-0.2f, 0.45f, 1.0f); // bottom left
                GL.TexCoord2(texDateVert[1, 1]); GL.Vertex3(-0.2f, 0.60f, 1.0f); // top left
                GL.TexCoord2(texDateVert[1, 2]); GL.Vertex3(-0.15f, 0.60f, 1.0f); // top right
                GL.TexCoord2(texDateVert[1, 3]); GL.Vertex3(-0.15f, 0.45f, 1.0f); // bottom right
                // Y
                //GL.BindTexture(TextureTarget.Texture2D, texture[1]);
                GL.TexCoord2(texDateVert[2, 0]); GL.Vertex3(-0.15f, 0.45f, 1.0f); // bottom left
                GL.TexCoord2(texDateVert[2, 1]); GL.Vertex3(-0.15f, 0.60f, 1.0f); // top left
                GL.TexCoord2(texDateVert[2, 2]); GL.Vertex3(-0.10f, 0.60f, 1.0f); // top right
                GL.TexCoord2(texDateVert[2, 3]); GL.Vertex3(-0.10f, 0.45f, 1.0f); // bottom right
                // Y
                //GL.BindTexture(TextureTarget.Texture2D, texture[1]);
                GL.TexCoord2(texDateVert[3, 0]); GL.Vertex3(-0.10f, 0.45f, 1.0f); // bottom left
                GL.TexCoord2(texDateVert[3, 1]); GL.Vertex3(-0.10f, 0.60f, 1.0f); // top left
                GL.TexCoord2(texDateVert[3, 2]); GL.Vertex3(-0.05f, 0.60f, 1.0f); // top right
                GL.TexCoord2(texDateVert[3, 3]); GL.Vertex3(-0.05f, 0.45f, 1.0f); // bottom right
                // -
                //GL.BindTexture(TextureTarget.Texture2D, texture[1]);
                GL.TexCoord2(texDateVert[4, 0]); GL.Vertex3(-0.05f, 0.45f, 1.0f); // bottom left
                GL.TexCoord2(texDateVert[4, 1]); GL.Vertex3(-0.05f, 0.60f, 1.0f); // top left
                GL.TexCoord2(texDateVert[4, 2]); GL.Vertex3(0.00f, 0.60f, 1.0f); // top right
                GL.TexCoord2(texDateVert[4, 3]); GL.Vertex3(0.00f, 0.45f, 1.0f); // bottom right
                // M
                //GL.BindTexture(TextureTarget.Texture2D, texture[1]);
                GL.TexCoord2(texDateVert[5, 0]); GL.Vertex3(0.00f, 0.45f, 1.0f); // bottom left
                GL.TexCoord2(texDateVert[5, 1]); GL.Vertex3(0.00f, 0.60f, 1.0f); // top left
                GL.TexCoord2(texDateVert[5, 2]); GL.Vertex3(0.05f, 0.60f, 1.0f); // top right
                GL.TexCoord2(texDateVert[5, 3]); GL.Vertex3(0.05f, 0.45f, 1.0f); // bottom right
                // M
                //GL.BindTexture(TextureTarget.Texture2D, texture[1]);
                GL.TexCoord2(texDateVert[6, 0]); GL.Vertex3(0.05f, 0.45f, 1.0f); // bottom left
                GL.TexCoord2(texDateVert[6, 1]); GL.Vertex3(0.05f, 0.60f, 1.0f); // top left
                GL.TexCoord2(texDateVert[6, 2]); GL.Vertex3(0.10f, 0.60f, 1.0f); // top right
                GL.TexCoord2(texDateVert[6, 3]); GL.Vertex3(0.10f, 0.45f, 1.0f); // bottom right
                // -
                //GL.BindTexture(TextureTarget.Texture2D, texture[1]);
                GL.TexCoord2(texDateVert[7, 0]); GL.Vertex3(0.10f, 0.45f, 1.0f); // bottom left
                GL.TexCoord2(texDateVert[7, 1]); GL.Vertex3(0.10f, 0.60f, 1.0f); // top left
                GL.TexCoord2(texDateVert[7, 2]); GL.Vertex3(0.15f, 0.60f, 1.0f); // top right
                GL.TexCoord2(texDateVert[7, 3]); GL.Vertex3(0.15f, 0.45f, 1.0f); // bottom right
                // D
                //GL.BindTexture(TextureTarget.Texture2D, texture[1]);
                GL.TexCoord2(texDateVert[8, 0]); GL.Vertex3(0.15f, 0.45f, 1.0f); // bottom left
                GL.TexCoord2(texDateVert[8, 1]); GL.Vertex3(0.15f, 0.60f, 1.0f); // top left
                GL.TexCoord2(texDateVert[8, 2]); GL.Vertex3(0.20f, 0.60f, 1.0f); // top right
                GL.TexCoord2(texDateVert[8, 3]); GL.Vertex3(0.20f, 0.45f, 1.0f); // bottom right
                // D
                //GL.BindTexture(TextureTarget.Texture2D, texture[1]);
                GL.TexCoord2(texDateVert[9, 0]); GL.Vertex3(0.20f, 0.45f, 1.0f); // bottom left
                GL.TexCoord2(texDateVert[9, 1]); GL.Vertex3(0.20f, 0.60f, 1.0f); // top left
                GL.TexCoord2(texDateVert[9, 2]); GL.Vertex3(0.25f, 0.60f, 1.0f); // top right
                GL.TexCoord2(texDateVert[9, 3]); GL.Vertex3(0.25f, 0.45f, 1.0f); // bottom right
                GL.End();
                GL.Disable(EnableCap.Texture2D);
                */
                #endregion
                //GL.BindTexture(TextureTarget.Texture2D, 0);
                /*GL.PushMatrix();
                GL.MatrixMode(MatrixMode.Projection);
                Matrix4 matProj = Matrix4.CreatePerspectiveFieldOfView( OpenTK.MathHelper.DegreesToRadians(70.0f) , (float)this.Width / this.Height, 1.0f, 10.0f);
                GL.LoadMatrix(ref matProj);

                GL.PopMatrix();
                GL.MatrixMode(MatrixMode.Modelview);*/
                //OpenTK.Graphics.Glu.Perspective(75.0, this.Width / this.Height, 1.0, -1.0);
               
            }

            
            if (this.blnFog)
            {
                GL.Enable(EnableCap.Fog);
                GL.Fog(FogParameter.FogMode, (float)FogMode.Linear);
                GL.Fog(FogParameter.FogDensity, 0.45f);
                GL.Hint(HintTarget.FogHint, HintMode.Nicest);
                GL.Fog(FogParameter.FogStart, 2.0f);
                GL.Fog(FogParameter.FogEnd, 2.3f);
            }
            if (this.blnLight)
            {
                GL.Enable(EnableCap.Lighting);
                GL.Enable(EnableCap.Light0);
            }

            // Chess quad
            chess.Draw();
            #region Chess - Old
            /*GL.Enable(EnableCap.Texture2D);
                //GL.ActiveTexture(TextureUnit.Texture2);
                GL.BindTexture(TextureTarget.Texture2D, texture[2]);
                GL.Begin(BeginMode.Quads);*/
            /*GL.TexCoord2(0.0, 0.0); GL.Vertex3(-1.00f, -1.00f, 1.0f); // bottom left
            GL.TexCoord2(0.0, 1.0); GL.Vertex3(-1.00f, 0.30f, -1.2f); // top left
            GL.TexCoord2(1.0, 1.0); GL.Vertex3(1.00f, 0.30f, -1.2f); // top right
            GL.TexCoord2(1.0, 0.0); GL.Vertex3(1.00f, -1.00f, 1.0f); // bottom right
            */
            /*
            GL.TexCoord2(texTimeVert[i, 0]); GL.Vertex3(0.60f , 0.65f, 0.5f); // bottom right
            GL.TexCoord2(texTimeVert[i, 1]); GL.Vertex3(0.60f , 0.95f, 0.5f); // top right
            GL.TexCoord2(texTimeVert[i, 2]); GL.Vertex3(0.85f , 0.95f, 0.5f); // top left
            GL.TexCoord2(texTimeVert[i, 3]); GL.Vertex3(0.85f , 0.65f, 0.5f); // bottom left
            */
            /*yScroll += 0.008;
            if (yScroll >= 1.0) yScroll = 0.0;
            Console.WriteLine(yScroll);
            GL.TexCoord2(0.0 + xScroll, 0.0 + yScroll); GL.Vertex3(-2.30f, -1.50f, -1.0f); // bottom right
            GL.TexCoord2(0.0 + xScroll, 5.0 + yScroll); GL.Vertex3(-2.30f, 0.20f, 1.0f); // top right
            GL.TexCoord2(5.0 + xScroll, 5.0 + yScroll); GL.Vertex3(2.30f, 0.20f, 1.0f); // top left
            GL.TexCoord2(5.0 + xScroll, 0.0 + yScroll); GL.Vertex3(2.30f, -1.50f, -1.0f); // bottom left
            // this needs the TexCoord4 to make it depth perspective correction
            GL.End();
            GL.Disable(EnableCap.Texture2D);*/
            /*
            GL.Begin(BeginMode.Points);

            GL.Color3(System.Drawing.Color.Yellow); GL.Vertex3(1.00f, 0.20f, 1.0f); // top left
            GL.Color3(System.Drawing.Color.Orange); GL.Vertex3(1.00f, -1.00f, 1.0f); // bottom left
            GL.Color3(System.Drawing.Color.Green); GL.Vertex3(-1.00f, 0.20f, 1.0f); // top right
            GL.Color3(System.Drawing.Color.Red); GL.Vertex3(-1.00f, -1.00f, 1.0f); // bottom right
            GL.Color3(System.Drawing.Color.White);
            GL.End();
            */
            #endregion

            if (this.blnFog) GL.Disable(EnableCap.Fog);

            // Draw a text
            //text.Draw();
            if (this.blnLight)
            {
                GL.Disable(EnableCap.Lighting);
                GL.Disable(EnableCap.Light0);
            }

            //particle.drawParticles();

           // sune.Draw();
            //dif.Draw();
         //   xmas.Draw();
            // s.Draw();
           // f.Draw();
            tl.Draw();
            SwapBuffers(); // Swapping the background and foreground buffers to display our scene
            //Console.WriteLine(this.RenderFrequency);
        }

        
        
        private void OnKeyboardKeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs key)
        {
            
           // RaiseKeyboardEvent(OpenTKKeyboardEventType.ButtonPressed, key.Key);
            if (key.Key == OpenTK.Input.Key.Escape)
            {
               // xmas.Dispose();
                this.Exit();
                //this.Close(); // dosen't release the window...
            }
            else if (/*key.Key == OpenTK.Input.Key.Enter && key.Key.HasFlag(OpenTK.Input.Key.AltLeft)*/ key.Key == OpenTK.Input.Key.F)
            {
                if (this.blnFullscreen)
                {
                    
                    OpenTK.DisplayDevice.Default.RestoreResolution();
                    this.WindowState = OpenTK.WindowState.Normal;
                    //GL.Viewport(this.ClientRectangle);
                    Console.WriteLine("Going to window");
                }
                else
                {
                    OpenTK.DisplayDevice dev = OpenTK.DisplayDevice.Default;
                    OpenTK.DisplayDevice.Default.ChangeResolution(dev.Width, dev.Height, dev.BitsPerPixel, dev.RefreshRate);
                    //OpenTK.DisplayDevice.Default.ChangeResolution(OpenTK.DisplayDevice.Default.AvailableResolutions.Last());
                    this.WindowState = OpenTK.WindowState.Fullscreen;
                    //GL.Viewport(this.ClientRectangle);
                    Console.WriteLine("Going to fullscreen");
                }
                this.blnFullscreen = !this.blnFullscreen;
            }
            else if (key.Key == OpenTK.Input.Key.P)
            {
                this.blnPointDraw = !this.blnPointDraw; // not active
            }
            else if (key.Key == OpenTK.Input.Key.W)
            {
                this.blnWireFrameDraw = !this.blnWireFrameDraw; // not active
            }
            else if (key.Key == OpenTK.Input.Key.O)
            {
                this.blnFog = !this.blnFog;
            }
            else if (key.Key == OpenTK.Input.Key.L)
            {
                this.blnLight = !this.blnLight;
            }
        }

        protected override void OnKeyPress(OpenTK.KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            /*if ( (Byte)e.KeyChar == 27 )
            {
                this.Exit();
                //this.Close(); // dosen't release the window...
            }*/
            /*else if (OpenTK.Input.Key.AltLeft == )
            {
                if (this.blnFullscreen)
                {
                    this.blnFullscreen = !this.blnFullscreen;
                    OpenTK.DisplayDevice.Default.RestoreResolution();
                    this.WindowState = OpenTK.WindowState.Normal;
                }
                else
                {
                    this.blnFullscreen = !this.blnFullscreen;
                    OpenTK.DisplayDevice.Default.ChangeResolution(OpenTK.DisplayDevice.Default.AvailableResolutions.Last());
                    this.WindowState = OpenTK.WindowState.Fullscreen;
                }
            }
            Console.WriteLine((byte)e.KeyChar);*/
        }
    }

}
