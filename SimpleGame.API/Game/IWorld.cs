using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleGame.API.Game
{
    public interface IWorld
    {

        public int AddObject(IObject obj);
        public void DeleteObject(int id);
        public IObject GetObject(int id);
        public List<IObject> GetObjects();
    }
}
