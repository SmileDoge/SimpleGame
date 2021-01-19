using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace SimpleGame.API.Render
{
    public interface IShader
    {
        public Matrix4 ViewMatrix { get; set; }
        public Matrix4 ProjectionMatrix { get; set; }
        public Matrix4 ModelMatrix { get; set; }

        public void SetMatrix4(string name, Matrix4 target);
        public void SetVector3(string name, Vector3 target);
        public void SetFloat(string name, float target);
        public void SetInt(string name, int target);
        public void Use();
    }
}
