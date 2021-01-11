using System;

namespace SimpleGame.API
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginAttribute : Attribute
    {
        public string PluginID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }

        public PluginAttribute(string pluginid, string name)
        {
            PluginID = pluginid;
            Name = name;
            Author = "unknown";
        }

        public PluginAttribute(string pluginid, string name, string author)
        {
            PluginID = pluginid;
            Name = name;
            Author = author;
        }
    }

    public interface IPlugin
    {
        public void OnEnable();
        public void OnDisable();
    };
}
