using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SimpleGame.Render;
using SimpleGame.FreeTypeHelper;
using System.IO;

namespace SimpleGame
{
    public class App : GameWindow
    {
        private RenderEngine _engine;
        private ObjModel _model;

        private Texture _texture;

        private float _oldWheel;

        private FontRenderer _fontRenderer;
        private FreeType _freeType;
        private PluginLoader _pluginLoader;

        private double _maxFps;

        public RenderEngine Engine { get { return _engine; } }
        public FontRenderer FontRenderer { get { return _fontRenderer; } }
        public double MaxFPS { 
            get { return _maxFps; }
            set
            {
                value = Math.Clamp(value, 1, 500);
                RenderFrequency = value;
                UpdateFrequency = value;
                _maxFps = value;
            } 
        }

        public App(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws)
        {
            _engine = new RenderEngine(nws.Size.X, nws.Size.Y);
            _engine.Camera.Fov = 90f;
            _engine.Camera.Sensitivity = 0.7f;

            _maxFps = RenderFrequency;
            MaxFPS = RenderFrequency;

            var _model2 = ModelLoader.LoadFromFile("./Resources/untitled1.obj");
            _model = new ObjModel(_model2.Vertex.ToArray(), _model2.UV.ToArray());

            _freeType = new FreeType("C:/Windows/Fonts/segoeuisl.ttf");
            _fontRenderer = new FontRenderer(Size.X, Size.Y);

            _pluginLoader = new PluginLoader();
            _pluginLoader.LoadPlugin("SimplePlugin");

            var stringFont = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890\"!`?'.,;:()[]{}<>|/@\\^$-%+=#_&~* ";

            try
            {
                stringFont = File.ReadAllText("./Resources/stringFont.txt").Replace("\n",string.Empty);
            }
            catch
            {
                Console.WriteLine("stringFont.txt not found. Using default");
            }


            VSync = VSyncMode.Off;
          
            foreach(var c in stringFont)
            {
                new Character(c.ToString());
            }
        }

        protected override void OnLoad()
        {
            _engine.InitializeGL();
            _model.InitializeGL();
            _fontRenderer.InitalizeGL();
            foreach (var ch in Character.Characters)
            {
                ch.Value.InitializeGL();
            }
            _freeType.Done();
            _texture = Texture.LoadFromFile("Resources/text.jpg");

            CursorGrabbed = true;



            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            Quaternion qt = new Quaternion(0f, 0f, 0f);

            var matrix = Matrix4.Identity;
            matrix = matrix * Matrix4.CreateFromQuaternion(qt);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            _engine.Begin();
            _texture.Use(TextureUnit.Texture0);
            _engine.SetMatrix("model", matrix);
            
            _model.Render();
            _engine.End();

            var _fps = Math.Round(1d / args.Time);

            _fontRenderer.PrintText(_fps.ToString(), 0f, 0f, 1f, new Vector3(0.8f, 0.2f, 0.1f));
            _fontRenderer.PrintText(_engine.Camera.Fov.ToString(), 0f, 0.1f, 1f, new Vector3(0.8f, 0.2f, 0.1f));


            SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            _engine.Resize(e.Width, e.Height);
            _fontRenderer.UpdateProjection(e.Width, e.Height);

            

            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (!IsFocused)
            {
                return;
            }
            _engine.Camera.Controll(KeyboardState, MouseState, (float)args.Time);

            base.OnUpdateFrame(args);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            _engine.Camera.Fov += (_oldWheel - e.OffsetY);

            _oldWheel = e.OffsetY;

            base.OnMouseWheel(e);
        }

    }
}
