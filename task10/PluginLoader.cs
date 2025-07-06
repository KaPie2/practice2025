using System.Reflection;

namespace task10;

public interface IPluginCommand
{
    public void Execute();
}

[AttributeUsage(AttributeTargets.Class)]
public class PluginLoadAttribute : Attribute
{
    public string[] Dependencies { get; } = Array.Empty<string>();

    public PluginLoadAttribute(params string[] dependencies)
    {
        Dependencies = dependencies;
    }
}

public class PluginLoader
{
    private string _path;

    public PluginLoader(string path)
    {
        _path = path;
    }

    public void LoadAndRunPlugins()
    {
        var plugins = new List<(Type Type, string Name, string[] Dependencies)>();

        foreach (var dllPath in Directory.GetFiles(_path, "*.dll"))
        {
            var assembly = Assembly.LoadFrom(dllPath);
            foreach (var type in assembly.GetTypes())
            {
                var attribute = type.GetCustomAttribute<PluginLoadAttribute>();
                if (attribute != null && typeof(IPluginCommand).IsAssignableFrom(type))
                {
                    plugins.Add((type, type.Name, attribute.Dependencies));
                }
            }
        }

        var loaded = new HashSet<string>();

        while (loaded.Count < plugins.Count)
        {
            bool loadedInThisPass = false;
            foreach (var plugin in plugins)
            {
                if (loaded.Contains(plugin.Name))
                    continue;

                if (plugin.Dependencies.All(d => loaded.Contains(d)))
                {
                    var instance = (IPluginCommand)Activator.CreateInstance(plugin.Type)!;
                    instance.Execute();

                    loaded.Add(plugin.Name);
                    loadedInThisPass = true;
                }
            }

            if (!loadedInThisPass)
            {
                break;
            }
        }
    }
}
