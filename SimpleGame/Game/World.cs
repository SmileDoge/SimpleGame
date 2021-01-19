using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Mathematics;
using SimpleGame.API.Game;

namespace SimpleGame.Game
{
    public class World : IWorld
    {

        private Dictionary<int, IObject> _objects = new Dictionary<int, IObject>();

        public Vector3 LightColor = new Vector3(1f, 1f, 1f);
        public Vector3 LightPos = new Vector3(1.2f, 1.0f, 2.0f);

        public IObject GetObject(int id)
        {
            var result = _objects.TryGetValue(id, out var value);
            return result ? value : null;
        }

        public List<IObject> GetObjects()
        {
            return _objects.Values.ToList();
        }

        public int AddObject(IObject obj)
        {
            int id;
            do
            {
                id = new Random().Next(1, 100000);
            } 
            while (_objects.ContainsKey(id));
            obj.ObjectID = id;

            _objects.Add(id, obj);

            return id;
        }

        public void DeleteObject(int id)
        {
            if (_objects.ContainsKey(id))
            {
                _objects.Remove(id);
            }
        }

        public void Render()
        {
            foreach (var obj in _objects)
            {
                obj.Value.Render();
            }
        }
    }
}
