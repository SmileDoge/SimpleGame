using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using SimpleGame.API.Game;
using SimpleGame.API.Render;
using SimpleGame.Render;

namespace SimpleGame.Game
{
    public abstract class Object : IObject
    {
        public float Scale { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Color { get; set; }
        public IMaterial Material { get; set; }

        public int ObjectID { get; set; }

        public Object()
        {
            Scale = 1f;
            Position = new Vector3(0f, 0f, 0f);
            Rotation = Quaternion.FromEulerAngles(new Vector3(0f, 0f, 0f));
            Color = new Vector3(1f, 1f, 1f);
        }

        public virtual void Render()
        {
            Material.Shader.SetMatrix4("model",
                Matrix4.CreateScale(Scale) *
                Matrix4.CreateFromQuaternion(Rotation) *
                Matrix4.CreateTranslation(Position)
                );

            Material.Shader.SetMatrix4("projection", MainGame.Instance.Engine.Camera.GetProjectionMatrix());

            Material.Shader.SetMatrix4("view", MainGame.Instance.Engine.Camera.GetViewMatrix());

            Material.Shader.SetVector3("objectColor", Color);

            Material.Render();
        }
    }
}
