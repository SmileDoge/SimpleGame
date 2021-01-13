using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleGame.API.Game;

namespace SimpleGame.Game
{
    public class World : IWorld
    {
        private Dictionary<int, IObject> _objects;

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
            var random = new Random();
            var id = random.Next(1, 100000);
            while (_objects.ContainsKey(id))
            {
                id = random.Next(1, 100000);
            }
            obj.ObjectID = id;

            return id;
        }

        public void DeleteObject(int id)
        {
            if (_objects.ContainsKey(id))
            {
                _objects.Remove(id);
            }
        }
    }
}
