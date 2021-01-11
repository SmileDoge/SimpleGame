using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace SimpleGame.Render
{

    public class Vertex
    {
        public static readonly int VertexInfoLength = 5;

        private Vector3 _pos;
        private Vector2 _uvPos;
        private Color4 _color;

        public Vertex(Vector3 pos, Vector2 uvPos) : this(pos, uvPos, Color4.White)
        {
        }

        public Vertex(Vector3 pos, Vector2 uvPos, Color4 color)
        {
            _pos = pos;
            _uvPos = uvPos;
            _color = color;
        }

        public float[] GetData()
        {
            float[] data = new float[]
            {
                _pos.X,
                _pos.Y,
                _pos.Z,
                _uvPos.X,
                _uvPos.Y/*,
                _color.R,
                _color.G,
                _color.B,
                _color.A
                */
            };
            return data;
        }

        public static float[] GetFullData(Vertex[] vertices)
        {
            float[] data = new float[VertexInfoLength * vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                float[] vertex = vertices[i].GetData();
                data[i * VertexInfoLength + 0] = vertex[0]; // Pos X
                data[i * VertexInfoLength + 1] = vertex[1]; // Pos Y
                data[i * VertexInfoLength + 2] = vertex[2]; // Pos Z
                data[i * VertexInfoLength + 3] = vertex[3]; // UV Pos X
                data[i * VertexInfoLength + 4] = vertex[4]; // UV Pos Y
            }

            return data;
        }
    }
}
