using System;
using System.Runtime.InteropServices;

namespace SimpleTest
{
    //[StructLayout(LayoutKind.Sequential)]
    public struct SVector3
    {
        public float X;
        public float Y;
        public float Z;
    }

    //[StructLayout(LayoutKind.Sequential)]
    public struct SVector2
    {
        public float X;
        public float Y;
    }

    //[StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public SVector3 Position;
        public SVector3 Normal;
        public SVector2 TexCoords;
    }


    class Program
    {
        static unsafe void Main(string[] args)
        {
            Vertex[] vertices = new Vertex[]
            {
                new Vertex
                {
                    Position = new SVector3 { X = 1, Y = 2, Z = 3 },
                    Normal = new SVector3 { X = 4, Y = 5, Z = 6 },
                    TexCoords = new SVector2 { X = 6, Y = 7 },
                },
                new Vertex
                {
                    Position = new SVector3 { X = 8, Y = 9, Z = 10 },
                    Normal = new SVector3 { X = 11, Y = 12, Z = 13 },
                    TexCoords = new SVector2 { X = 14, Y = 15 },
                }
            };
            fixed (Vertex* vertex = &vertices[0])
            {
                Console.WriteLine(vertex);
            }
        }
    }
}
