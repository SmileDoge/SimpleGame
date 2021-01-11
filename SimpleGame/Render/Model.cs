using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ObjLoader.Loader.Loaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SimpleGame.Render
{
    public class Model
    {
        private float[] _vertexData;
        private uint[] _indicesData;

        private int _elementBufferObject;
        private int _vertexBufferObject;
        private int _vertexArrayObject;

        public bool UseIndices { get; private set; }


        public Model(float[] vertexData, uint[] indices)
        {
            init(vertexData, indices);
            
        }

        public Model(string path)
        {
            var loaderFactory = new ObjLoaderFactory();
            var loader = loaderFactory.Create();
            var result = loader.Load(new FileStream(path, FileMode.Open));

            Console.WriteLine(result.Vertices.Count);
            Console.WriteLine(result.Textures.Count);
            Console.WriteLine(result.Groups[0].Faces.Count);
            /*
            Vertex[] vertices = new[]
            {
                new Vertex(new Vector3(0.5f, 0.5f, 0.0f), new Vector2(1.0f, 1.0f)),
                new Vertex(new Vector3(0.5f, -0.5f, 0.0f), new Vector2(1.0f, 0.0f)),
                new Vertex(new Vector3(-0.5f, -0.5f, 0.0f), new Vector2(0.0f, 0.0f)),
                new Vertex(new Vector3(-0.5f, 0.5f, 0.0f), new Vector2(0.0f, 1.0f)),
            };
            */
            /*
            var indices = new uint[]
            {
                0, 1, 3,
                1, 2, 3
            };
            */
            Vertex[] vertices = new[]
            {
                new Vertex(new Vector3(-1f, 1f, 0f), new Vector2(0f, 0f)),
                new Vertex(new Vector3(1f, 1f, 0f), new Vector2(1f, 0f)),
                new Vertex(new Vector3(-1f, -1f, 0f), new Vector2(0f, 1f)),
                new Vertex(new Vector3(1f, -1f, 0f), new Vector2(1f, 1f)),
            };
            var indices = new uint[]
            {
                1, 2, 0,
                1, 3, 2
            };

            init(Vertex.GetFullData(vertices), indices);
        }

        private void init(float[] vertexData, uint[] indices)
        {
            _vertexData = vertexData;
            if (indices != null)
            {
                _indicesData = indices;
                UseIndices = true;
            }
            else
            {
                UseIndices = false;
            }
        }

        public void InitializeGL()
        {
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertexData.Length * sizeof(float), _vertexData, BufferUsageHint.StaticDraw);

            if (UseIndices) {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indicesData.Length * sizeof(uint), _indicesData, BufferUsageHint.StaticDraw);
            }

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }

        public void Render(int vertices = 3)
        {
            GL.BindVertexArray(_vertexArrayObject);
            if (UseIndices)
            {
                GL.DrawElements(PrimitiveType.Triangles, _indicesData.Length, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.Triangles, 0, vertices);
            }
        }
    }
}
