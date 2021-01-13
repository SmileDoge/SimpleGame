using System;
using System.Collections.Generic;
using System.Text;
using SimpleGame.API.Game;
using SimpleGame.API.Render;

namespace SimpleGame.API
{
    public interface IApp
    {
        //public RenderEngine Engine { get; };

        public IWorld World { get; set; }

        public ICamera Camera { get; set; }
    }
}
