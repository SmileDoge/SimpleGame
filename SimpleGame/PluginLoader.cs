using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using SimpleGame.API;

namespace SimpleGame
{
    public class PluginInformation
    {
        public Assembly Assembly;
        public IPlugin Plugin;
        public string PluginID;
        public string PluginName;
        public string PluginAuthor;
    }

    public class PluginLoader
    {
        private Dictionary<string, PluginInformation> _loadedPlugins = new Dictionary<string, PluginInformation>();
        private string _loadingPath;

        public Dictionary<string, PluginInformation> LoadedPlugins { get { return _loadedPlugins; } }

        public PluginLoader(string path = @"\Plugins")
        {
            _loadingPath = path;
        }

        public void LoadPlugin(string name)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var pluginPath = rootPath + Path.Combine(Path.Combine(_loadingPath, name), name + ".dll");
            var context = AssemblyLoadContext.Default;
            Console.WriteLine(rootPath);
            Console.WriteLine(pluginPath);
            var assembly = context.LoadFromAssemblyPath(pluginPath);
            foreach(var type in assembly.GetTypes())
            {
                var attribute = type.GetCustomAttribute(typeof(PluginAttribute)) as PluginAttribute;
                if (attribute == null && type.GetInterfaces().Contains(typeof(IPlugin)) == false) return;
                IPlugin plugin = Activator.CreateInstance(type) as IPlugin;

                PluginInformation pluginInformation = new PluginInformation
                {
                    Assembly = assembly,
                    Plugin = plugin,
                    PluginAuthor = attribute.Author,
                    PluginID = attribute.PluginID,
                    PluginName = attribute.Name,
                };

                _loadedPlugins.Add(attribute.PluginID, pluginInformation);

                Console.WriteLine("Loading " + pluginInformation.PluginName);
                plugin.OnEnable();
            }
        }
    }
}
