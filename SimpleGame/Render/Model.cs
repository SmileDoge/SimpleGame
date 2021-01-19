using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Assimp;
using Assimp.Configs;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SimpleGame.Render
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SVector3
    {
        public float X;
        public float Y;
        public float Z;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SVector2
    {
        public float X;
        public float Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public SVector3 Position;
        public SVector3 Normal;
        public SVector2 TexCoords;
    }

    public class MeshV2
    {
        public Vertex[] Vertices;
        public uint[] Indices;

        private int _vao;
        private int _vbo;
        private int _ebo;

        public MeshV2(Vertex[] vertices, uint[] indices)
        {
            Vertices = vertices;
            Indices = indices;

            setupMesh();
        }

        private void setupMesh()
        {
            var vertexSize = Marshal.SizeOf(typeof(Vertex));

            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            GL.BindVertexArray(_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * vertexSize, Vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexSize, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, vertexSize, Marshal.OffsetOf<Vertex>("Normal"));

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, vertexSize, Marshal.OffsetOf<Vertex>("TexCoords"));

        }

        public void Render()
        {
            GL.BindVertexArray(_vao);
            GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }

    public class Model
    {
        private static Dictionary<string, Model> _models = new Dictionary<string, Model>();

        private List<MeshV2> _meshes = new List<MeshV2>();

        public Model(string path)
        {
            var importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));

            var scene = importer.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs);

            processNode(scene.RootNode, scene);
        }

        private void processNode(Node node, Scene scene)
        {
            foreach (var index in node.MeshIndices)
            {
                var mesh = scene.Meshes[index];
                _meshes.Add(processMesh(mesh, scene));
            }

            foreach (var index in node.Children)
            {
                processNode(index, scene);
            }
        }

        private MeshV2 processMesh(Assimp.Mesh mesh, Scene scene)
        {
            var vertices = new List<Vertex>();
            var indices = new List<uint>();

            for(int i = 0; i < mesh.VertexCount; i++)
            {
                Vertex vertex = new Vertex();

                vertex.Position.X = mesh.Vertices[i].X;
                vertex.Position.Y = mesh.Vertices[i].Y;
                vertex.Position.Z = mesh.Vertices[i].Z;

                vertex.Normal.X = mesh.Normals[i].X;
                vertex.Normal.Y = mesh.Normals[i].Y;
                vertex.Normal.Z = mesh.Normals[i].Z;


                vertex.TexCoords.X = mesh.TextureCoordinateChannels[0][i].X;
                vertex.TexCoords.Y = mesh.TextureCoordinateChannels[0][i].Y;

                Console.WriteLine(vertex.TexCoords.X + " " + vertex.TexCoords.Y);

                /*sds
                /*sdsds
                /*sdsdsds
                /*sdsdsdss
                if (mesh.HasTextureCoords(0))
                {
                }
                else
                {
                    vertex.TexCoords.X = 0f;
                    vertex.TexCoords.Y = 0f;
                }
                */

                vertices.Add(vertex);
            }

            for (int i = 0; i < mesh.FaceCount; i++) 
            {
                Face face = mesh.Faces[i];
                for (int j = 0; j < face.IndexCount; j++)
                    indices.Add((uint)face.Indices[j]);
            }

            return new MeshV2(vertices.ToArray(), indices.ToArray());
        }

        public void Render()
        {
            foreach (var mesh in _meshes)
            {
                mesh.Render();
            }
        }
        public static bool RegisterModel(string id, Model model)
        {
            if (_models.ContainsKey(id)) return false;
            _models.Add(id, model);
            return true;
        }
        public static Model GetModel(string id)
        {
            if (_models.TryGetValue(id, out var result))
            {
                return result;
            }
            return null;
        }
    }

    [Obsolete]
    public class Mesh
    {

        private Vector3[] _vertex;
        private Vector2[] _uv;

        private int _vao;
        private int _vboVertex;
        private int _vboUV;

        public Mesh(Vector3[] vertices, Vector2[] uv)
        {
            _vertex = vertices;
            _uv = uv;
        }

        public unsafe void InitializeGL()
        {
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vboVertex = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboVertex);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertex.Length * sizeof(Vector3), _vertex, BufferUsageHint.StaticDraw);


            _vboUV = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboUV);
            GL.BufferData(BufferTarget.ArrayBuffer, _uv.Length * sizeof(Vector2), _uv, BufferUsageHint.StaticDraw);
        }

        public void Render()
        {
            //GL.BindVertexArray(_vao);

            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboVertex);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboUV);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.DrawArrays(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, 0, _vertex.Length);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
        }


    }
    /*
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
            //var loaderFactory = new ObjLoaderFactory();
            //var loader = loaderFactory.Create();
            //var result = loader.Load(new FileStream(path, FileMode.Open));

            //Console.WriteLine(result.Vertices.Count);
            //Console.WriteLine(result.Textures.Count);
            //Console.WriteLine(result.Groups[0].Faces.Count);
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
    */
}
