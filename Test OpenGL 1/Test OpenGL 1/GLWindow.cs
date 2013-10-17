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
using System.Windows.Forms;

namespace OpenGL
{
    public class GLWindow : OpenTK.GameWindow
    {
        private const string TITLE = "Turbo";
        //const int WIDTH = 800; // titta på
        //const int HEIGHT = 640;

        //private bool blnPointDraw;
        //private bool blnWireFrameDraw;

        private Event.Event events;
        private CrashHandler CrashH;
        private int[] Resolution;

        // OpenGL version after 3.0 needs there own matrix libs so we need to create them if we run over 3.0!!! if ser major and minor to 0 we can get around it?!
        public GLWindow(System.Xml.Linq.XDocument Events, string runtime, int[] res, ref CrashHandler Crash) : base(res[0], res[1], new OpenTK.Graphics.GraphicsMode(/*new OpenTK.Graphics.ColorFormat(32), 24, 8, 0*/)/*OpenTK.Graphics.GraphicsMode.Default*/, TITLE, OpenTK.GameWindowFlags.Default, OpenTK.DisplayDevice.Default, 0, 0, OpenTK.Graphics.GraphicsContextFlags.Debug | OpenTK.Graphics.GraphicsContextFlags.Default) 
        {
            this.WindowBorder = OpenTK.WindowBorder.Fixed;
#if !DEBUG
            //OpenTK.DisplayDevice.Default.ChangeResolution(this.Width, this.Height, OpenTK.DisplayDevice.AvailableDisplays[0].BitsPerPixel, OpenTK.DisplayDevice.AvailableDisplays[0].RefreshRate);
            WindowState = OpenTK.WindowState.Fullscreen;
            WindowBorder = OpenTK.WindowBorder.Hidden;
            Util.Fullscreen = true;
            System.Windows.Forms.Cursor.Hide();

#else
            _WriteVersion();

#endif
            // fix me...
            /*if (runtime.Length < 24) // YYYY-MM-DDYYYY-MM-DDxxxxxx
            {
                throw new Exception("Error in runtime, it is to short");
            }*/

            System.Diagnostics.Debug.WriteLine("Currently used textures: " + Util.CurrentUsedTextures);
            Keyboard.KeyDown += OnKeyboardKeyDown;
            Closing += OnClosing;

            //blnPointDraw = false;
            //blnWireFrameDraw = false;
            DateTime dtStart = DateTime.Parse(runtime.Substring(0, 10));
            DateTime dtEnd = DateTime.Parse(runtime.Substring(10, 10));
            CrashH = Crash;
            Resolution = res;

            //TimeSpan tsDiff = dtEnd.Subtract(dtStart);
            
            events = new Event.Event(dtStart, dtEnd, int.Parse(runtime.Substring(20)), Events, ref CrashH);
            
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
            System.Diagnostics.Debug.WriteLine("Max texture size: " + Util.MaxTexturesSizeWidth); // make this a Util-tool as this we need to use to see so that our textures can fit the opengl target of the machine...
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
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.Enable(EnableCap.CullFace);
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
            this.TargetRenderFrequency = 60.0; // forcing to update at 60 hz
            this.TargetUpdateFrequency = 60.0; // forcing to update at 60 hz

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f); // Set the clear color to a black
        }


        protected void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            //snd.StopThread();
#if DEBUG
            events.StopSound();
            CrashH.Exit = false;
            this.Exit();
            GL.BindTexture(TextureTarget.Texture2D, 0);
            System.Diagnostics.Debug.WriteLine("Closing");
#else
            if (Util.Fullscreen)
            {
                Util.Fullscreen = false;
                System.Windows.Forms.Cursor.Show();
                //OpenTK.DisplayDevice.Default.RestoreResolution();
                foreach (var item in OpenTK.DisplayDevice.AvailableDisplays)
                {
                    item.RestoreResolution();
                }
                //OpenTK.DisplayDevice.AvailableDisplays[0].RestoreResolution();
                WindowState = OpenTK.WindowState.Normal;
                WindowBorder = OpenTK.WindowBorder.Fixed;
                System.Diagnostics.Debug.WriteLine("Going to window");
            }

            if (System.Windows.Forms.MessageBox.Show("You are about to close this window, are you sure?", "Close window", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                events.StopSound();
                CrashH.Exit = false;    
                this.Exit();
                GL.BindTexture(TextureTarget.Texture2D, 0);
                System.Diagnostics.Debug.WriteLine("Closing");
            }
            else
            {
                e.Cancel = true;
                if (!Util.Fullscreen)
                {
                    
                    Util.Fullscreen = true;
                    System.Windows.Forms.Cursor.Hide();
                    OpenTK.DisplayDevice.Default.ChangeResolution(Resolution[0], Resolution[1], OpenTK.DisplayDevice.Default.BitsPerPixel, Resolution[2]);
                    //OpenTK.DisplayDevice.AvailableDisplays[1].ChangeResolution(Resolution[0], Resolution[1], OpenTK.DisplayDevice.Default.BitsPerPixel, Resolution[2]);
                    WindowState = OpenTK.WindowState.Fullscreen;
                    WindowBorder = OpenTK.WindowBorder.Hidden;
                    System.Diagnostics.Debug.WriteLine("Going to fullscreen");
                }
            }
#endif

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            System.Diagnostics.Debug.WriteLine("Currently used textures: " + Util.CurrentUsedTextures);
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
#if DEBUG
                events.StopSound();
                CrashH.Exit = false;
                this.Exit();
                
#else
                /*Util.Fullscreen = false;
                System.Windows.Forms.Cursor.Show();
                OpenTK.DisplayDevice.Default.RestoreResolution();
                WindowState = OpenTK.WindowState.Normal;
                WindowBorder = OpenTK.WindowBorder.Resizable;
                //GL.Viewport(this.ClientRectangle);
                System.Diagnostics.Debug.WriteLine("Going to window");

                if (MessageBox.Show("You are about to close this window, are you sure?", "Close window", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    events.StopSound();
                    CrashH.Exit = false;*/
                    this.Exit();
                /*}
                else
                {
                    Util.Fullscreen = true;
                    System.Windows.Forms.Cursor.Hide();
                    OpenTK.DisplayDevice.Default.ChangeResolution(Resolution[0], Resolution[1], OpenTK.DisplayDevice.Default.BitsPerPixel, Resolution[2]);
                    WindowState = OpenTK.WindowState.Fullscreen;
                    WindowBorder = OpenTK.WindowBorder.Hidden;
                    System.Diagnostics.Debug.WriteLine("Going to fullscreen");
                }*/
                Application.DoEvents();
#endif

                //this.Close(); // dosen't release the window...
            }
            else if (/*key.Key == OpenTK.Input.Key.Enter && key.Key.HasFlag(OpenTK.Input.Key.AltLeft)*/ key.Key == OpenTK.Input.Key.F)
            {
                if (Util.Fullscreen)
                {
                    System.Windows.Forms.Cursor.Show();
                    OpenTK.DisplayDevice.Default.RestoreResolution();
                    WindowState = OpenTK.WindowState.Normal;
                    WindowBorder = OpenTK.WindowBorder.Fixed;
                    //GL.Viewport(this.ClientRectangle);
                    System.Diagnostics.Debug.WriteLine("Going to window");
                }
                else
                {
                    System.Windows.Forms.Cursor.Hide();
                    //OpenTK.DisplayDevice.Default.ChangeResolution(dev.Width, dev.Height, dev.BitsPerPixel, dev.RefreshRate);
                    OpenTK.DisplayDevice.Default.ChangeResolution(Resolution[0], Resolution[1], Screen.PrimaryScreen.BitsPerPixel/*OpenTK.DisplayDevice.Default.BitsPerPixel*/, Resolution[2]);
                    //OpenTK.DisplayDevice.Default.ChangeResolution(OpenTK.DisplayDevice.Default.AvailableResolutions.Last());
                    WindowState = OpenTK.WindowState.Fullscreen;
                    WindowBorder = OpenTK.WindowBorder.Hidden;
                    System.Diagnostics.Debug.WriteLine("Going to fullscreen");
                }
                Util.Fullscreen = !Util.Fullscreen;
            }
#if DEBUG
            /*else if (key.Key == OpenTK.Input.Key.P)
            {
                this.blnPointDraw = !this.blnPointDraw; // not active
            }*/
            else if (key.Key == OpenTK.Input.Key.V)
            {
                if (VSync == VSyncMode.Off)
                {
                    VSync = OpenTK.VSyncMode.On;
                    this.TargetRenderFrequency = 60.0; // forcing to update at 60 hz, change this to the selected refresh rate.....
                    this.TargetUpdateFrequency = 60.0;
                }
                else
                {
                    VSync = OpenTK.VSyncMode.Off;
                    this.TargetRenderFrequency = 0.0;
                    this.TargetUpdateFrequency = 0.0;
                }
                
            }
            /*else if (key.Key == OpenTK.Input.Key.W)
            {
                this.blnWireFrameDraw = !this.blnWireFrameDraw; // not active
            }*/
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
