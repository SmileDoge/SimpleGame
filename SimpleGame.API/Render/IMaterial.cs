using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleGame.API.Render
{
    public interface IMaterial
    {
        public IShader Shader { get; set; }
        public ITexture Texture { get; set; }

        public void Render();
    }
}
