using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using SimpleGame.Render;

namespace SimpleGame.Game
{
    public class Light : Object
    {
        private Model _model;

        public Light() : base()
        {
            Material = SimpleGame.Render.Material.GetMaterial("lightMat");

            Scale = 0.2f;
            Position = MainGame.Instance.World.LightPos;

            _model = Model.GetModel("test");
        }

        public override void Render()
        {
            base.Render();

            _model.Render();
        }
    }
}
