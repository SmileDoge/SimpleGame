using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SimpleGame.Render;

namespace SimpleGame.Game
{
    public class Cube : Object
    {
        private Model _model;

        public Cube()
        {
            Material = SimpleGame.Render.Material.GetMaterial("Mesh");
            //_mesh = Mesh.GetMesh("test");

            Position = new Vector3(0f, 0f, 0f);
            Rotation = Quaternion.FromEulerAngles(new Vector3(0f, 0f, 0f));
            Color = new Vector3(1f, 0.5f, 0.31f);

            //Texture texture = Texture.LoadFromFile("./Resources/text.jpg");
            //Shader shader = new Shader("./Resources/shader.vert", "./Resources/shader.frag");

            //Material = new Material();
            //Material.Texture = texture;
            //Material.Shader = shader;

            //var result = ModelLoader.LoadFromFile("./Resources/untitled1.obj");
            //_mesh = new Mesh(result.Vertex.ToArray(), result.UV.ToArray());

            _model = Model.GetModel("test");
        }

        public override void Render()
        {

            base.Render();

            Material.Shader.SetVector3("lightColor", MainGame.Instance.World.LightColor);
            Material.Shader.SetVector3("lightPos", MainGame.Instance.World.LightPos);
            Material.Shader.SetVector3("viewPos", MainGame.Instance.Engine.Camera.Position);

            _model.Render();
        }
    }
}
