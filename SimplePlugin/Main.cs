using System;
using SimpleGame.API;

namespace SimplePlugin
{
    [Plugin("simpleplugin", "SimplePlugin")]
    public class Main : IPlugin
    {
        public void OnEnable()
        {
            Console.WriteLine("Plugin enabled");
        }
        public void OnDisable()
        {
            Console.WriteLine("Plugin disable");
        }
    }
}
