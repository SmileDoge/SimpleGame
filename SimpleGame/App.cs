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
using SimpleGame.Game;
using OpenTK.Audio.OpenAL;

namespace SimpleGame
{
    public class MainGame : GameWindow
    {
        public static MainGame Instance ;

        private World _world;

        private RenderEngine _engine;

        private FontRenderer _fontRenderer;
        private FreeType _freeType;
        private PluginLoader _pluginLoader;

        private float _oldWheel;
        private double _maxFps;

        public int LightID { get; private set; }
        public Light Light { get; private set; }

        public RenderEngine Engine { get { return _engine; } }
        public FontRenderer FontRenderer { get { return _fontRenderer; } }
        public World World { get { return _world; } }
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

        public MainGame(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws)
        {
            _engine = new RenderEngine(nws.Size.X, nws.Size.Y);
            _engine.Camera.Fov = 90f;
            _engine.Camera.Sensitivity = 0.7f;

            _maxFps = RenderFrequency;
            MaxFPS = RenderFrequency;

            _freeType = new FreeType("C:/Windows/Fonts/segoeuisl.ttf");
            _fontRenderer = new FontRenderer(Size.X, Size.Y);

            _pluginLoader = new PluginLoader();
            _pluginLoader.LoadPlugin("SimplePlugin");

            _world = new World();

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
            _fontRenderer.InitalizeGL();
            foreach (var ch in Character.Characters)
            {
                ch.Value.InitializeGL();
            }
            _freeType.Done();

            var text = Texture.LoadFromFile("./Resources/text.jpg");
            Texture.RegisterTexture("texture", text);

            var shader = new Shader("./Resources/shader.vert", "./Resources/shader.frag");
            Shader.RegisterShader("shader", shader);

            var mat = new Material();
            mat.Shader = shader;
            mat.Texture = text;
            Material.RegisterMaterial("Mesh", mat);

            var lightshader = new Shader("./Resources/shader.vert", "./Resources/lightShader.frag");
            Shader.RegisterShader("lightShader", lightshader);

            var lightMat = new Material
            {
                Shader = lightshader,
                Texture = text
            };
            Material.RegisterMaterial("lightMat", lightMat);

            var model = new Model("./Resources/untitled1.obj");
            Model.RegisterModel("test", model);

            Light = new Light();

            var cube = new Cube();
            _world.AddObject(cube);
            LightID = _world.AddObject(Light);

            CursorGrabbed = true;

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _world.Render();
            
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

        public void SetLightPos(Vector3 pos)
        {
            World.LightPos = pos;
            Light.Position = pos;
        }

        public void MoveLight(Vector3 dir)
        {
            SetLightPos(Light.Position + dir);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (!IsFocused) return;

            CursorGrabbed = false;
            CursorVisible = true;

            if (KeyboardState.IsKeyDown(Keys.Up)) MoveLight(new Vector3(0f, 0.6f * (float)args.Time, 0f));
            if (KeyboardState.IsKeyDown(Keys.Down)) MoveLight(new Vector3(0f, -0.6f * (float)args.Time, 0f));

            if (KeyboardState.IsKeyDown(Keys.Right)) MoveLight(new Vector3(0.6f * (float)args.Time, 0f, 0f));
            if (KeyboardState.IsKeyDown(Keys.Left)) MoveLight(new Vector3(-0.6f * (float)args.Time, 0f, 0f));

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
