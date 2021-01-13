using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using SimpleGame.API.Game;
using SimpleGame.API.Render;
using SimpleGame.Render;

namespace SimpleGame.Game
{
    public class Object : IObject
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public IMaterial Material { get; set; }

        public int ObjectID { get; set; }

        public void Render()
        {
            Material.Render();
        }
    }
}
