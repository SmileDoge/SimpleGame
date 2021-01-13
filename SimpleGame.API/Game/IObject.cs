using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using SimpleGame.API.Render;

namespace SimpleGame.API.Game
{
    public interface IObject : IRender
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public IMaterial Material { get; set; }

        public int ObjectID { get; set; }
    }
}
