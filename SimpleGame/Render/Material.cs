using System;
using System.Collections.Generic;
using System.Text;
using SimpleGame.API.Render;

namespace SimpleGame.Render
{
    public class Material : IMaterial
    {
        public IShader Shader { get; set; }
        public ITexture Texture { get; set; }

        public void Render()
        {
            Shader.Use();
            Texture.Use((int)OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
        }
    }
}
