using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK.Mathematics;
using SimpleGame.Render;

namespace SimpleGame
{
    public class ModelLoader
    {
        public class LoaderResult
        {
            public List<Vector3> Vertex;
            public List<Vector2> UV;
            public List<Vector3> Normal;
        }

        public static LoaderResult LoadFromFile(string path)
        {
            StreamReader file = new StreamReader(path);
            string line;

            List<uint> vertexIndices = new List<uint>();
            List<uint> uvIndices = new List<uint>();
            List<uint> normalIndices = new List<uint>();

            List<Vector3> tempVertices = new List<Vector3>();
            List<Vector2> tempUv = new List<Vector2>();
            List<Vector3> tempNormal = new List<Vector3>();

            while ((line = file.ReadLine()) != null)
            {
                if (line.Length <= 2) continue;
                var start = line.Substring(0, 2);

                switch (start)
                {
                    case "# ":
                        break;
                    case "v ":
                        var args = line.Substring(2).Split(" ");
                        tempVertices.Add(new Vector3(
                            float.Parse(args[0]),
                            float.Parse(args[1]),
                            float.Parse(args[2])
                            ));
                        break;
                    case "vt":
                        args = line.Substring(3).Split(" ");
                        tempUv.Add(new Vector2(
                            float.Parse(args[0]),
                            float.Parse(args[1])
                            ));
                        break;
                    case "vn":
                        args = line.Substring(3).Split(" ");
                        tempNormal.Add(new Vector3(
                            float.Parse(args[0]),
                            float.Parse(args[1]),
                            float.Parse(args[2])
                            ));
                        break;
                    case "f ":
                        args = line.Substring(2).Split(" ");
                        for (int i = 0; i < 3; i++)
                        {
                            var data = args[i].Split("/");
                            vertexIndices.Add(uint.Parse(data[0]));
                            uvIndices.Add(uint.Parse(data[1]));
                            normalIndices.Add(uint.Parse(data[2]));
                        }
                        break;
                }
            }

            List<Vector3> outputVertex = new List<Vector3>();
            List<Vector2> outputUv = new List<Vector2>();
            List<Vector3> outputNormal = new List<Vector3>();

            for (int i = 0; i < vertexIndices.Count; i++)
            {
                var vertexIndex = vertexIndices[i];
                var uvIndex = uvIndices[i];
                var normalIndex = normalIndices[i];

                var vertex = tempVertices[(int)vertexIndex - 1];
                var uv = tempUv[(int)uvIndex - 1];
                var normal = tempNormal[(int)normalIndex - 1];

                outputVertex.Add(vertex);
                outputUv.Add(uv);
                outputNormal.Add(normal);
            }

            return new LoaderResult { Vertex = outputVertex, Normal = outputNormal, UV = outputUv };
        }

        /*
        
        public class ModelVertex
        {
            public float X;
            public float Y;
            public float Z;
        }
        public class ModelTexture
        {
            public float U;
            public float V;
        }
        public class ModelFace
        {
            public ModelVertex[] Vertices = new ModelVertex[3];
            public ModelTexture[] TexturesUV = new ModelTexture[3];

        } 
         
        public static List<Vertex> LoadFromFile(string path)
        {
            StreamReader file = new StreamReader(path);
            string line;

            var tempVertex = new List<ModelVertex>();
            var tempTexture = new List<ModelTexture>();
            var tempFaces = new List<ModelFace>();

            var outputVertex = new List<Vertex>();
            var outputIndices = new List<int>();

            while ((line = file.ReadLine()) != null)
            {
                line = line.Replace(".", ",");
                var start = line.Substring(0, 2);
                switch (start)
                {
                    case "# ":
                        break;
                    case "v ":
                        var args = line.Substring(2).Split(" ");
                        var x = args[0];
                        var y = args[1];
                        var z = args[2];
                        Console.WriteLine($"Vertex {x} {y} {z}");
                        tempVertex.Add(new ModelVertex
                        {
                            X = Convert.ToSingle(x),
                            Y = Convert.ToSingle(y),
                            Z = Convert.ToSingle(z)
                        });
                        break;
                    case "vt":
                        args = line.Substring(3).Split(" ");
                        x = args[0];
                        y = args[1];
                        Console.WriteLine($"Texture {x} {y}");
                        tempTexture.Add(new ModelTexture
                        {
                            U = Convert.ToSingle(x),
                            V = Convert.ToSingle(y)
                        });
                        break;
                    case "f ":
                        args = line.Substring(2).Split(" ");
                        var face = new ModelFace();
                        for(int i = 0; i < 3; i++)
                        {
                            var text = args[i];
                            var data = text.Split("/");
                            var vertex = int.Parse(data[0]);
                            var texture = int.Parse(data[1]);
                            var normal = int.Parse(data[2]);
                            face.Vertices[i] = tempVertex[vertex - 1];
                            face.TexturesUV[i] = tempTexture[texture - 1];
                        }
                        tempFaces.Add(face);
                        break;
                }
            }

            //Console.WriteLine(tempFaces.Count); // 12;
            //Console.WriteLine(tempVertex.Count); // 8;
            //Console.WriteLine(tempTexture.Count); // 14;

            for (int i = 0; i < tempFaces.Count; i++)
            {
                for (int i2 = 0; i2 < 3; i2++) {
                    var ver = tempFaces[i].Vertices[i2];
                    var verPos = new Vector3(ver.X, ver.Y, ver.Z);

                    var uv = tempFaces[i].TexturesUV[i2];
                    var uvPos = new Vector2(uv.U, uv.V);

                    var vertex = new Vertex(verPos, uvPos);
                    outputVertex.Add(vertex);
                }


            }

            return outputVertex;
        }
        */
    }
}
