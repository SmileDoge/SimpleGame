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
        private Model _model;

        private Texture _texture;

        private Matrix4 _view = Matrix4.Identity;
        private Matrix4 _projection = Matrix4.Identity;

        private double _time;

        private FontRenderer _fontRenderer;
        private FreeType _freeType;
        private PluginLoader _pluginLoader;

        public App(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws)
        {
            _engine = new RenderEngine();
            Vertex[] vertices = new[]
            {
                new Vertex(new Vector3(0.5f, 0.5f, 0.0f), new Vector2(1.0f, 1.0f)),
                new Vertex(new Vector3(0.5f, -0.5f, 0.0f), new Vector2(1.0f, 0.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f, 0.0f), new Vector2(0.0f, 0.0f)),
                new Vertex(new Vector3(-0.5f, 0.5f, 0.0f), new Vector2(0.0f, 1.0f)),
            };
            var indices = new uint[]
            {
                0, 1, 3,
                1, 2, 3
            };
            _model = new Model("./Resources/untitled1.obj");

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


            /*
            float[] _vertices =
            {
                -0.5f, -0.5f, 0.0f, // Bottom-left vertex
                 0.5f, -0.5f, 0.0f, // Bottom-right vertex
                 0.0f,  0.5f, 0.0f  // Top vertex
            };
            _model = new Model(_vertices);
            */
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
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            _view = _view * Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            //_view = _view * Matrix4.CreateFromQuaternion(new Quaternion(0f, 45f, 0f));
            
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(70f), Size.X / (float)Size.Y, 0.1f, 100.0f);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {

            _time += 3.0 * args.Time;

            Quaternion qt = new Quaternion(0f, Convert.ToSingle(_time), 0f);

            var matrix = Matrix4.Identity;
            matrix = matrix * Matrix4.CreateFromQuaternion(qt);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            _engine.Begin();
            _texture.Use(TextureUnit.Texture0);
            _engine.SetMatrix("model", matrix);
            _engine.SetMatrix("view", _view);
            _engine.SetMatrix("projection", _projection);
            
            _model.Render();
            _engine.End();

            var _fps = Math.Round(1d / args.Time);

            _fontRenderer.PrintText(_fps.ToString(), 0f, 0f, 1f, new Vector3(0.8f, 0.2f, 0.1f));

            SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);

            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(70f), e.Width / (float)e.Height, 0.1f, 100.0f);
            _fontRenderer.UpdateProjection(e.Width, e.Height);


            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            base.OnUpdateFrame(args);
        }
    }
}
