using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleGame.API.Render
{
    public interface ITexture
    {
        public void Use(int textureUnit);
    }
}
