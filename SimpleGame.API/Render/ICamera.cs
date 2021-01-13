using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace SimpleGame.API.Render
{
    public interface ICamera
    {
        public Vector3 Position { get; set; }
        public Vector3 Front { get; set; }
        public Vector3 Up { get; set; }
        public Vector3 Right { get; set; }

        public float AspectRatio { get; set; }
        public float Sensitivity { get; set; }
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Fov { get; set; }
        public float CameraSpeed { get; set; }

    }
}
