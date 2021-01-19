using System;
using System.Collections.Generic;
using System.Text;
using SimpleGame.API.Render;

namespace SimpleGame.Render
{
    public class Material : IMaterial
    {
        private static Dictionary<string, Material> _materials = new Dictionary<string, Material>();

        public IShader Shader { get; set; }
        public ITexture Texture { get; set; }

        public void Render()
        {
            Shader.Use();
            Texture.Use((int)OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
        }

        public static bool RegisterMaterial(string id, Material material)
        {
            if (_materials.ContainsKey(id)) return false;
            _materials.Add(id, material);
            return true;
        }

        public static Material GetMaterial(string id)
        {
            if (_materials.TryGetValue(id, out var result))
            {
                return result;
            }
            return null;
        }
    }
}
