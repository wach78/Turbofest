using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Drawing.Imaging;

using OpenTK;
//using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Threading;

namespace OpenGL
{
    public class GLWindow : OpenTK.GameWindow
    {
        const string TITLE = "Project X";
        const int WIDTH = 1024;
        const int HEIGHT = 768;

        bool blnPointDraw;
        bool blnWireFrameDraw;


        /*
        string lastDate, nowDate; 
        PartyClock pc;
        Chess chess;
        Starfield sf;
        Text2D text;

        //Particle particle;
        SuneAnimation sune;
        Dif dif;
        Fbk fbk;
        Christmas xmas;
        Semla s;
        TurboLogo tl;
        Datasmurf smurf;
        Halloween hw;
        Valentine v;
        Outro o;
        Intro i;
        Birthday b;
        RMS richard;
        WinLinux wl;
        Lucia lucia;
        Advent advent;
        NewYear nw;
        Scroller scroll;


        // Test for sound
        Sound snd;
        */
        Event.Event events;
        System.Xml.Linq.XDocument m_events; // this needs to be fixed and stuff...

        // OpenGL version after 3.0 needs there own matrix libs so we need to create them if we run over 3.0!!! if ser major and minor to 0 we can get around it?!
        public GLWindow(System.Xml.Linq.XDocument Events, string runtime) : base(WIDTH, HEIGHT, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(32), 24, 8, 0)/*OpenTK.Graphics.GraphicsMode.Default*/, TITLE, OpenTK.GameWindowFlags.Default, OpenTK.DisplayDevice.Default, 0, 0, OpenTK.Graphics.GraphicsContextFlags.Debug | OpenTK.Graphics.GraphicsContextFlags.Default) 
        {
            /*if (runtime.Length < 28)
            {
                throw new Exception("Error in runtime, it is to short");
            }*/
            System.Diagnostics.Debug.WriteLine("Currently used textures: " + Util.CurrentUsedTextures);
            Keyboard.KeyDown += OnKeyboardKeyDown;
            Closing += OnClosing;

            blnPointDraw = false;
            blnWireFrameDraw = false;
            m_events = Events;
            DateTime dtStart = DateTime.Parse(runtime.Substring(0, 10));
            DateTime dtEnd = DateTime.Parse(runtime.Substring(10, 10));
            //TimeSpan tsDiff = dtEnd.Subtract(dtStart);

            events = new Event.Event(dtStart, dtEnd, int.Parse(runtime.Substring(20)), Events);

            /*
            //Sound
            snd = new Sound(true); // this starts the sound thread
            // Clock
            pc = new PartyClock(dtStart, dtEnd, int.Parse(runtime.Substring(20)));
            
            // Effects
            chess = new Chess();
            sf = new Starfield(300);
            text = new Text2D();
            sune = new SuneAnimation(ref snd, ref text);
            
            //particle = new Particle();
            fbk = new Fbk(ref snd);
            dif = new Dif(ref chess);
            xmas = new Christmas(ref snd);
            s = new Semla();
            f = new Fbk(ref snd);
            tl = new TurboLogo(ref snd,ref chess);
            smurf = new Datasmurf(ref snd,ref text);
            hw = new Halloween(ref chess, 25);
            v = new Valentine(ref snd);
            o = new Outro(ref snd);
            i = new Intro(ref snd, ref text);
            b = new Birthday(ref snd, ref text, ref chess);
            richard = new RMS(ref snd, ref text);
            wl = new WinLinux(ref chess);
            lucia = new Lucia(ref chess, ref snd);
            advent = new Advent(ref snd);
            nw = new NewYear();
            scroll = new Scroller(ref chess, ref sf, ref text);
*/


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
            System.Diagnostics.Debug.WriteLine(GL.GetString(StringName.Version) + ", " + GL.GetString(StringName.ShadingLanguageVersion) + ", " + GL.GetString(StringName.Renderer) + ", " + GL.GetString(StringName.Extensions));
            System.Diagnostics.Debug.WriteLine("Max combinde textures: " + Util.MaxCombindeTextures + ", Currently using genTextures: " + Util.CurrentUsedTextures + ", Max Draw Buffers: " + Util.MaxBuffers);
            System.Diagnostics.Debug.WriteLine("Max texture units in a vertex shader: " + texVxShader);
            System.Diagnostics.Debug.WriteLine("Max texture units in a fragment shader: " + texFragSh);
            System.Diagnostics.Debug.WriteLine("Max texture units in a fixed pipe: " + texFixedPipe);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Move this GL.Enable in to the drawing method as this is only to be done in there
            //GL.Enable(EnableCap.Texture2D);
            //GL.Enable(EnableCap.DepthTest); // to enable depth but needs reconfiguring
            //GL.DepthRange(-1.0, 1.0); // needed for DepthTest to show graphics
            GL.DepthRange(0.0, 1.0);
            GL.Enable(EnableCap.DepthTest);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.AlphaTest);

            GL.Fog(FogParameter.FogMode, (float)FogMode.Linear);
            GL.Fog(FogParameter.FogDensity, 0.95f);
            GL.Hint(HintTarget.FogHint, HintMode.Nicest);
            GL.Fog(FogParameter.FogStart, 4.7f);
            GL.Fog(FogParameter.FogEnd, 5.0f);

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

            //snd.StopThread();

            GL.BindTexture(TextureTarget.Texture2D, 0);
            System.Diagnostics.Debug.WriteLine("Closing");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            System.Diagnostics.Debug.WriteLine("Currently used textures: " + Util.CurrentUsedTextures);
            //Not clean... set to null as well if we want that
            /*
            snd.Dispose();
            pc.Dispose(); // 2 texturer
            sune.Dispose(); // 1-2 texturer
            chess.Dispose(); // 7 texturer
            sf.Dispose(); // 0 texturer
            text.Dispose(); // 9 texturer
            fbk.Dispose(); // 1 textur
            dif.Dispose(); // 1 textur
            xmas.Dispose(); // 3 texturer
            s.Dispose(); // 1 textur
            tl.Dispose(); // 1 textur
            smurf.Dispose(); // 1 textur
            hw.Dispose(); // 1 textur
            v.Dispose(); // 2 texturer
            o.Dispose(); // 1 textur
            i.Dispose(); // 1 textur
            b.Dispose(); // 1 textur
            richard.Dispose(); // 1 textur
            wl.Dispose(); // 1 textur
            lucia.Dispose(); // 1 textur
            advent.Dispose(); // 1 textur
            nw.Dispose();

            */
            if (events != null) events.Dispose();
            System.Diagnostics.Debug.WriteLine("Currently used textures: " + Util.CurrentUsedTextures);
            System.Diagnostics.Debug.WriteLine(this.GetType().ToString() + " closed.");

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
            /*float fov = OpenTK.MathHelper.DegreesToRadians(60.0f); // MathHelper.PiOver2;
            float aspect = 1.6f; // (float)(DisplayDevice.Default.Width / (double)DisplayDevice.Default.Height)
            Matrix4 matProj = Matrix4.CreatePerspectiveFieldOfView(fov, aspect, 0.1f, 5.0f);*/
            Matrix4 matProj = Matrix4.CreatePerspectiveFieldOfView(Util.FOV, Util.Aspect, 0.1f, 5.0f);
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
            Util.MVPChanged(true);
            Util.ViewportChanged(true);
        }

        protected override void OnUpdateFrame(OpenTK.FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            
            //add frame render stuff...
            //tl.FrameUpdate(nowDate);
        }


        // The rendering for the scene happens here.
        protected override void OnRenderFrame(OpenTK.FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            this.Title = "Project X - FPS: " + this.RenderFrequency.ToString("F2") + " Vsync: " + (VSync == OpenTK.VSyncMode.Off ? "Off" : "On");

            /*GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            matLook = Matrix4.LookAt(new Vector3(0.0f, 0.0f, -1.3f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(ref matLook);*/

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.StencilBufferBit | ClearBufferMask.DepthBufferBit); // Clear the OpenGL color buffer
            //GL.MatrixMode(MatrixMode.Projection);

            /*
            if (!pc.EndOfRuntime())
            {
                pc.updateClock();
                //GL.MatrixMode(MatrixMode.Modelview);
                // Time

                pc.DrawTime();
                // Date
                pc.DrawDate();
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
            */
            //sf.Draw(nowDate);

            //sune.Draw(nowDate);

           // sune.Draw(nowDate);

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
             //dif.Draw(nowDate);
            //xmas.Draw(nowDate);
            //f.Draw(nowDate);
            //smurf.Draw(nowDate);
            //v.Draw(nowDate);
            //o.Draw(nowDate);
            //i.Draw(nowDate);
            //chess.Draw(nowDate);
            //hw.Draw(nowDate);
            //tl.Draw(nowDate);

            //b.Draw(nowDate);
            //snd.Play("Sune");
            //fbk.Draw(nowDate);
            //tl.Draw(nowDate);
            //hw.Draw(nowDate);

            //wl.Draw(nowDate);
            //richard.Draw(nowDate);
            //lucia.Draw(nowDate);
            //advent.Draw(nowDate, Advent.WhatAdvent.Fourth);

            events.Draw();

            SwapBuffers(); // Swapping the background and foreground buffers to display our scene
            //System.Diagnostics.Debug.WriteLine("FPS: " + (1.0/e.Time));
            //System.Diagnostics.Debug.WriteLine(RenderFrequency);
        }

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
                if (Util.Fullscreen)
                {
                    
                    OpenTK.DisplayDevice.Default.RestoreResolution();
                    WindowState = OpenTK.WindowState.Normal;
                    WindowBorder = OpenTK.WindowBorder.Resizable;
                    //GL.Viewport(this.ClientRectangle);
                    System.Diagnostics.Debug.WriteLine("Going to window");
                    
                }
                else
                {
                    OpenTK.DisplayDevice dev = OpenTK.DisplayDevice.Default;
                    OpenTK.DisplayDevice.Default.ChangeResolution(dev.Width, dev.Height, dev.BitsPerPixel, dev.RefreshRate);
                    //OpenTK.DisplayDevice.Default.ChangeResolution(OpenTK.DisplayDevice.Default.AvailableResolutions.Last());
                    WindowState = OpenTK.WindowState.Fullscreen;
                    WindowBorder = OpenTK.WindowBorder.Hidden;
                    //GL.Viewport(this.ClientRectangle);
                    System.Diagnostics.Debug.WriteLine("Going to fullscreen");
                }
                Util.Fullscreen = !Util.Fullscreen;
            }
#if DEBUG
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
                Util.Fog = !Util.Fog;
            }
            else if (key.Key == OpenTK.Input.Key.L)
            {
                Util.Lightning = !Util.Lightning;
            }
            else if (key.Key == OpenTK.Input.Key.C)
            {
                Util.ShowClock = !Util.ShowClock;
            }
#endif
            else if (key.Key == OpenTK.Input.Key.F12)
            {
                if (OpenTK.Graphics.GraphicsContext.CurrentContext == null)
                {
                    throw new OpenTK.Graphics.GraphicsContextMissingException();
                }
                Bitmap bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
                System.Drawing.Imaging.BitmapData data = bmp.LockBits(this.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                GL.ReadPixels(0, 0, this.ClientSize.Width, this.ClientSize.Height, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
                bmp.UnlockBits(data);
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                // break this out so it does that check on load???
                string[] files = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "img*.bmp", System.IO.SearchOption.TopDirectoryOnly);
                int maxFileName = 0;
                int currentFileName = 0;
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(/*@"(img([0-9]+).bmp)"*/ "([0-9]+)");
                System.Text.RegularExpressions.Match mh = null;
                foreach (string item in files)
                {
                    mh = reg.Match(item);
                    if (mh.Captures.Count > 0)
                    {
                        currentFileName = int.Parse(mh.Value); // not secure way if we get a error here we are fubared...
                        if (currentFileName > maxFileName)
                        {
                            maxFileName = currentFileName;
                        }
                    }
                }
                reg = null;
                mh = null;
                maxFileName++;
                //Save the screenshot to file.
                bmp.Save(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "//img" + maxFileName + ".bmp");
                bmp.Dispose(); // clear the data so that we don't have leaks...
            }
        }
    }

}
