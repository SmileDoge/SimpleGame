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

        public RenderEngine()
        {
            
        }

        public void InitializeGL()
        {
            GL.ClearColor(0.5f, 0.6f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            _shader = new Shader("./Resources/shader.vert", "./Resources/shader.frag");
        }

        public void Begin()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();
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
