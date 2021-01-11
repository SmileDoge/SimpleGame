using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using FreeTypeSharp.Native;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SimpleGame.Render;
using static FreeTypeSharp.Native.FT;

namespace SimpleGame.FreeTypeHelper
{
    public class FontRenderer
    {
        private int _vao;
        private int _vbo;

        private Shader _shader;

        private Matrix4 _projection;

        private float _width;
        private float _height;

        public FontRenderer(float widthScreen, float heightScreen)
        {
            _width = widthScreen;
            _height = heightScreen;


            _projection = Matrix4.CreateOrthographic(_width, _height, -10f, 10f);

            //_projection = Matrix4.CreateOrthographicOffCenter(0f, widthScreen, heightScreen, 0f, -10f, 10f);
            //_projection = Matrix4.CreateOrthographicOffCenter(-widthScreen, widthScreen, -heightScreen, heightScreen, -10f, 10f);

            /*
            _projection = Matrix4.CreateOrthographicOffCenter(
                -1.0f,
                1.0f * aspect,
                -1.0f,
                1.0f * aspect,
                -5.0f, 5.0f);
            */

        }

        public void UpdateProjection(float widthScreen, float heightScreen)
        {
            _width = widthScreen;
            _height = heightScreen;

            _projection = Matrix4.CreateOrthographic(_width, _height, -10f, 10f);
        }

        public void InitalizeGL()
        {
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 6 * 4, IntPtr.Zero, BufferUsageHint.DynamicDraw);


            _shader = new Shader("./Resources/textShader.vert", "./Resources/textShader.frag");
            _shader.SetMatrix4("projection", _projection);
        }

        public void PrintText(string text, float x, float y, float scale, Vector3 color)
        {
            _shader.Use();
            _shader.SetMatrix4("projection", _projection);
            _shader.SetVector3("textColor", color);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindVertexArray(_vao);

            x = _width * (x - 0.5f);
            y = (_height-48*2) * (-y + 0.5f);
            
            for (int i = 0; i < text.Length; i++)
            {
                Character c;
                if (Character.Characters.TryGetValue(text[i].ToString(), out Character value)) 
                {
                    c = value;
                }
                else
                {
                    c = Character.Characters["?"];
                }

                float xpos = x + c.Bearing.X * scale;
                float ypos = y - (c.Size.Y - c.Bearing.Y) * scale;

                float w = c.Size.X * scale;
                float h = c.Size.Y * scale;

                float[] vertices = new float[]
                {
                    xpos, ypos+h, 0.0f, 0.0f,
                    xpos, ypos, 0.0f, 1.0f,
                    xpos + w, ypos, 1.0f, 1.0f,

                    xpos, ypos+h, 0.0f, 0.0f,
                    xpos + w, ypos, 1.0f, 1.0f,
                    xpos + w, ypos+h, 1.0f, 0.0f
                };

                GL.BindTexture(TextureTarget.Texture2D, c.Texture);

                GL.EnableVertexAttribArray(0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
                GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

                x += (c.Advance >> 6) * scale;
            }
        }
    }

    public class Character
    {
        public static Dictionary<string, Character> Characters = new Dictionary<string, Character>();

        private string _char;

        private int _textureGl;

        private Vector2i _size;
        private Vector2i _bearing;

        private int _advance;

        public int Texture { get { return _textureGl; } }
        public Vector2i Size { get { return _size; } }
        public Vector2i Bearing { get { return _bearing; } }
        public int Advance { get { return _advance; } }

        public Character(string Char)
        {
            _char = Char;

            if (!Characters.ContainsKey(_char))
            {
                Characters[_char] = this;
            }
        }

        public void InitializeGL()
        {
            var freetype = FreeType.instance;

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            freetype.LoadChar(_char);
            var bitmap = freetype.GetBitmap(); 

            _size = new Vector2i((int)bitmap.width, (int)bitmap.rows);
            _bearing = freetype.GetBearing();
            _advance = freetype.GetAdvance().X;

            _textureGl = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _textureGl);
            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgb,
                (int)bitmap.width,
                (int)bitmap.rows,
                0,
                PixelFormat.Red,
                PixelType.UnsignedByte,
                bitmap.buffer
                );

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            
        }
    }

    public class FreeType
    {
        private IntPtr _library;
        private IntPtr _face;

        private unsafe FT_FaceRec* _faceRec;

        public static FreeType instance;

        public unsafe FreeType(string path)
        {
            if (instance == null) instance = this;

            FT_Init_FreeType(out _library);

            FT_New_Face(_library, path, 0, out _face);

            SetPixelSize(0, 48);

            //FT_Set_Char_Size(_face, (IntPtr)0, (IntPtr)(16*64), 800 ,600);
            //_faceRec = Marshal.PtrToStructure<FT_FaceRec>(_face);
            _faceRec = (FT_FaceRec*)(void*)_face;
        }

        public void Done()
        {
            FT_Done_Face(_face);
            FT_Done_FreeType(_library);
        }

        public FT_Bitmap GetBitmap()
        {
            FT_Bitmap result;
            unsafe
            {
                result = _faceRec->glyph->bitmap;
            }
            return result;
        }

        public Vector2i GetBearing()
        {
            int x;
            int y;

            unsafe
            {
                x = _faceRec->glyph->bitmap_left;
                y = _faceRec->glyph->bitmap_top;
            }

            return new Vector2i(x,y); 
        }

        public Vector2i GetAdvance()
        {
            uint x;
            uint y;

            unsafe
            {
                x = (uint)_faceRec->glyph->advance.x;
                y = (uint)_faceRec->glyph->advance.y;
            }

            return new Vector2i((int)x, (int)y);
        }

        public void SetPixelSize(uint width, uint height)
        {
            FT_Set_Pixel_Sizes(_face, width, height);
        }

        public void LoadChar(string Char)
        {
            uint code = (uint)char.ConvertToUtf32(Char, 0);
            var index = FT_Get_Char_Index(_face, code);

            FT_Load_Glyph(_face, index, FT_LOAD_RENDER);
        }
    }
}
