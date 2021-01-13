using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SimpleGame.Render
{
    public class RenderEngine
    {
        private Shader _shader;

        private Camera _camera;
        public Camera Camera { get { return _camera; } }

        public RenderEngine(float width, float height)
        {
            _camera = new Camera(new Vector3(0f, 0f, 0f), width/height);
        }

        public void Resize(float width, float height)
        {
            GL.Viewport(0, 0, (int)width, (int)height);
            var aspect = width / height;
            _camera.AspectRatio = aspect;
        }

        public void InitializeGL()
        {
            GL.ClearColor(0.5f, 0.6f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            _shader = new Shader("./Resources/shader.vert", "./Resources/shader.frag");
        }

        public void Begin()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
        }

        public void SetMatrix(string uniform, Matrix4 matrix)
        {
            _shader.SetMatrix4(uniform, matrix);
        }

        public void End()
        {
            
        }
    }
}
