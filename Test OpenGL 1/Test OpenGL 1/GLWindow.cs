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

        Matrix4 matLook;
        /*double xScroll = 0.0;
        double yScroll = 0.0;*/
        PartyClock pc;
        Chess chess;
        Starfield sf;
        Text2D text;

        Particle particle;
        SuneAnimation sune;
        Dif dif;
        Fbk fbk;
        Christmas xmas;
        Semla s;
        Fbk f;
        TurboLogo tl;
        Datasmurf smurf;

        // Test for sound
        Sound snd;

        System.Xml.Linq.XDocument m_events; // this needs to be fixed and stuff...

        // OpenGL version after 3.0 needs there own matrix libs so we need to create them if we run over 3.0!!! if ser major and minor to 0 we can get around it?!
        public GLWindow(System.Xml.Linq.XDocument Events, string runtime) : base(WIDTH, HEIGHT, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(32), 24, 8, 0)/*OpenTK.Graphics.GraphicsMode.Default*/, TITLE, OpenTK.GameWindowFlags.Default, OpenTK.DisplayDevice.Default, 0, 0, OpenTK.Graphics.GraphicsContextFlags.Debug | OpenTK.Graphics.GraphicsContextFlags.Default) 
        {
            if (runtime.Length < 28)
            {
                throw new Exception("Error in runtime, it is to short");
            }
            Console.WriteLine("Currently used textures: " + Util.CurrentUsedTextures);
            Keyboard.KeyDown += OnKeyboardKeyDown;
            Closing += OnClosing;

            blnFullscreen = false;
            blnPointDraw = false;
            blnWireFrameDraw = false;
            blnFog = true;
            blnLight = false;
            m_events = Events;
            DateTime dtStart = DateTime.Parse(runtime.Substring(0, 10));
            DateTime dtEnd = DateTime.Parse(runtime.Substring(10, 10));
            TimeSpan tsDiff = dtEnd.Subtract(dtStart);
            
            //Sound
            snd = new Sound(true); // this starts the sound thread
            // Clock
            pc = new PartyClock(dtStart, dtEnd, int.Parse(runtime.Substring(20)));
            
            // Effects
            chess = new Chess();
            sf = new Starfield();
            text = new Text2D();
            particle = new Particle();
            fbk = new Fbk(ref snd);
            sune = new SuneAnimation(ref snd, ref text);
            dif = new Dif(ref chess);
            xmas = new Christmas(ref snd);
            s = new Semla();
            f = new Fbk(ref snd);
            tl = new TurboLogo(ref snd);
            smurf = new Datasmurf(ref snd);

            //Events
            //_WriteVersion();
            //text.TextureCoordinates('A', Text2D.FontName.Coolfont);
        }

        public void _WriteVersion()
        {
            int texVxShader = 0;
            int texFragSh = 0;
            int texFixedPipe = 0;
            GL.GetInteger(GetPName.MaxVertexTextureImageUnits, out texVxShader);
            GL.GetInteger(GetPName.MaxTextureImageUnits, out texFragSh);
            GL.GetInteger(GetPName.MaxTextureUnits, out texFixedPipe);
            Console.WriteLine(GL.GetString(StringName.Version) + ", " + GL.GetString(StringName.ShadingLanguageVersion) + ", " + GL.GetString(StringName.Renderer) + ", " + GL.GetString(StringName.Extensions));
            Console.WriteLine("Max combinde textures: " + Util.MaxCombindeTextures + ", Currently using genTextures: " + Util.CurrentUsedTextures + ", Max Draw Buffers: " + Util.MaxBuffers);
            Console.WriteLine("Max texture units in a vertex shader: " + texVxShader);
            Console.WriteLine("Max texture units in a fragment shader: " + texFragSh);
            Console.WriteLine("Max texture units in a fixed pipe: " + texFixedPipe);
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
            VSync = OpenTK.VSyncMode.On; // this is to make it possible to run faster then refreshrate on screen...
            /*
            this.TargetRenderFrequency = 200.0; // forcing to update at 50 hz
            this.TargetUpdateFrequency = 200.0; // forcing to update at 50 hz
            */

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f); // Set the clear color to a black
        }


        protected void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            snd.StopThread();

            //Not clean... set to null as well if we want that
            snd.Dispose();
            pc.Dispose();
            chess.Dispose();
            sf.Dispose();
            text.Dispose();
            fbk.Dispose();
            sune.Dispose();
            tl.Dispose();
            f.Dispose();
            s.Dispose();
            xmas.Dispose();
            dif.Dispose();

            GL.BindTexture(TextureTarget.Texture2D, 0);
            Console.WriteLine("Closing");
        }

        /*protected override void Dispose(bool manual)
        {
            base.Dispose(manual);
            if (manual)
            {
                
                Console.WriteLine("After dispose textures: " + Util.CurrentUsedTextures);
            }
            Console.WriteLine("Disposing");
        }*/

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Console.WriteLine("Currently used textures: " + Util.CurrentUsedTextures);
            Console.WriteLine("Closed");
            this.Dispose(true);
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
            float fov = OpenTK.MathHelper.DegreesToRadians(60.0f); // MathHelper.PiOver2;
            float aspect = 1.6f; // (float)(DisplayDevice.Default.Width / (double)DisplayDevice.Default.Height)
            Matrix4 matProj = Matrix4.CreatePerspectiveFieldOfView(fov, aspect, 0.1f, 5.0f);
            GL.LoadMatrix(ref matProj); // need to invers this to make the camera on the right side of the object?
            
            /*Matrix4 ortho_projection = Matrix4.CreateOrthographicOffCenter(0.0f, Width, Height, 0, -1.0f, 1.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref ortho_projection);*/
            /*
            GL.MatrixMode(MatrixMode.Modelview);
           // GL.PopMatrix();
            GL.LoadIdentity();
            Matrix4 matLook = Matrix4.LookAt(new Vector3(0.0f, 0.0f, -1.3f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            //Matrix4 matLook = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.LoadMatrix(ref matLook);
            //GL.Rotate(180, 0.0, 0.0, 1.0);
            //GL.Scale(2.0f, 2.0f, 2.0f); // scale to fit window better if z is -2.3 on eye placement
            */
        }

        protected override void OnUpdateFrame(OpenTK.FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            
            //add frame render stuff...
            //tl.FrameUpdate(nowDate);
        }


        /*OpenTK.Vector2[,] texTimeVert = new Vector2[8, 4];
        OpenTK.Vector2[,] texDateVert = new Vector2[10, 4];*/

        // The rendering for the scene happens here.
        protected override void OnRenderFrame(OpenTK.FrameEventArgs e)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            matLook = Matrix4.LookAt(new Vector3(0.0f, 0.0f, -1.3f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(ref matLook);

            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.StencilBufferBit | ClearBufferMask.DepthBufferBit); // Clear the OpenGL color buffer
            GL.MatrixMode(MatrixMode.Projection);
            if (!pc.EndOfRuntime())
            {
                pc.updateClock();
                //GL.MatrixMode(MatrixMode.Modelview);
                // Time
                pc.DrawTime();
                // Date
                pc.DrawDate();
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
            //chess.Draw();

            if (this.blnFog) GL.Disable(EnableCap.Fog);

            // Draw a text
            //text.Draw();
            if (this.blnLight)
            {
                GL.Disable(EnableCap.Lighting);
                GL.Disable(EnableCap.Light0);
            }

            // Draw effects and events here
            nowDate = pc.CurrentClock().ToShortDateString();
            if (nowDate != lastDate)
            {
                if (lastDate != string.Empty)
                {
                    snd.StopSound();
                }
                lastDate = nowDate;
                sune.NewQoute();
            }
            //sf.Draw(nowDate);
         //   sune.Draw(nowDate);
            /*if (nowDate == "2012-03-03")
                sune.Draw(nowDate);
            else if (nowDate == "2012-03-02")
                fbk.Draw(nowDate);
            else if (nowDate == "2012-03-04")
                sune.Draw(nowDate);
            else
            {
                //tl.toPlay(nowDate);
                tl.Draw(nowDate);
            }*/
           // text.Draw("Hej på dig!", Text2D.FontName.Coolfont, new Vector3(1.0f, 0.0f, 1.5f), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(0.0f, 0.0f));
            //text.Draw("andra raden som skall själv delas?", Text2D.FontName.CandyPink, new Vector3(1.0f, -0.4f, 1.5f), new OpenTK.Vector2(0.10f, 0.10f), new OpenTK.Vector2(2.8f, 0.10f));
            //text.Draw("Ännu mer här !åäö? och så har vi något lång rad som inte skall få radbrytnignar om man inte\ngör en själv!", Text2D.FontName.TypeFont, new Vector3(1.6f, -0.6f, 1.5f), new OpenTK.Vector2(0.1f, 0.1f), new OpenTK.Vector2(0.0f, 0.0f));
            //s.Draw(nowDate);
           // dif.Draw(nowDate);
           // xmas.Draw(nowDate);
          //  f.Draw(nowDate);
            smurf.Draw(nowDate);
            
            SwapBuffers(); // Swapping the background and foreground buffers to display our scene
            //Console.WriteLine("FPS: " + (1.0/e.Time));
            //Console.WriteLine(RenderFrequency);
        }

        string lastDate, nowDate;
        
        
        private void OnKeyboardKeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs key)
        {
            //OpenTK.Input.MouseDevice asd = new OpenTK.Input.MouseDevice();
            
           // RaiseKeyboardEvent(OpenTKKeyboardEventType.ButtonPressed, key.Key);
            if (key.Key == OpenTK.Input.Key.Escape)
            {
                //snd.Stop();
                //soundThread.Abort();
                this.Exit();
                //this.Close(); // dosen't release the window...
            }
            else if (/*key.Key == OpenTK.Input.Key.Enter && key.Key.HasFlag(OpenTK.Input.Key.AltLeft)*/ key.Key == OpenTK.Input.Key.F)
            {
                if (this.blnFullscreen)
                {
                    
                    OpenTK.DisplayDevice.Default.RestoreResolution();
                    WindowState = OpenTK.WindowState.Normal;
                    WindowBorder = OpenTK.WindowBorder.Resizable;
                    //GL.Viewport(this.ClientRectangle);
                    Console.WriteLine("Going to window");
                    
                }
                else
                {
                    OpenTK.DisplayDevice dev = OpenTK.DisplayDevice.Default;
                    OpenTK.DisplayDevice.Default.ChangeResolution(dev.Width, dev.Height, dev.BitsPerPixel, dev.RefreshRate);
                    //OpenTK.DisplayDevice.Default.ChangeResolution(OpenTK.DisplayDevice.Default.AvailableResolutions.Last());
                    WindowState = OpenTK.WindowState.Fullscreen;
                    WindowBorder = OpenTK.WindowBorder.Hidden;
                    //GL.Viewport(this.ClientRectangle);
                    Console.WriteLine("Going to fullscreen");
                }
                this.blnFullscreen = !this.blnFullscreen;
            }
            else if (key.Key == OpenTK.Input.Key.P)
            {
                this.blnPointDraw = !this.blnPointDraw; // not active
            }
            else if (key.Key == OpenTK.Input.Key.V)
            {
                if (VSync == VSyncMode.Off)
                {
                    VSync = OpenTK.VSyncMode.On;
                }
                else
                {
                    VSync = OpenTK.VSyncMode.Off;
                }
                
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
    }

}
