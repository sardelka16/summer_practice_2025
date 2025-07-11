namespace pluginLoader;

using System.Reflection;
using pluginLoadAttribute;
using pluginLib;

public class PluginLoader
{
    private readonly string _pluginsPath;
    private readonly List<Assembly> _loadedAssemblies = new List<Assembly>();
    private readonly List<Type> _pluginTypes = new List<Type>();

    public PluginLoader(string pluginsPath)
    {
        _pluginsPath = pluginsPath;
    }

    public void LoadPlugins()
    {

        LoadAllAssemblies();
        FindAllPluginTypes();
        var sortedPlugins = SortByDependenses();
        ExecutePlugins(sortedPlugins);
    }
    
    public void LoadAllAssemblies()
    {
        if (!Directory.Exists(_pluginsPath))
        {
            throw new DirectoryNotFoundException($"Directoty not found: {_pluginsPath}");
        }

        foreach (var dll in Directory.GetFiles(_pluginsPath, "*.dll", SearchOption.AllDirectories))
        {
            try
            {
                var assembly = Assembly.LoadFrom(dll);
                _loadedAssemblies.Add(assembly);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load {dll}: {ex.Message}");
            }
        }
    }

    public void FindAllPluginTypes()
    {
        var uniqPlugins = new HashSet<string>();

        foreach (var assembly in _loadedAssemblies.DistinctBy(a=>a.Location))
        {
            try
            {
                foreach (var type in assembly.GetTypes())
                {
                    var attr = type.GetCustomAttribute<PluginLoadAttribute>();
                    if (attr != null)
                    {
                        if (uniqPlugins.Add(type.FullName))
                        {
                            _pluginTypes.Add(type);
                            Console.WriteLine($"Found plugin: {type.FullName} from {assembly.Location}");
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"Failed to get types from {assembly.FullName}: {ex.Message}");
            }
        }
    }
    
    public List<Type> SortByDependenses()
    {
        return _pluginTypes.OrderBy(type => 
        {
            var attr = type.GetCustomAttribute<PluginLoadAttribute>();
            return attr?.Dependencies?.Length ?? 0;
        }).ToList();
    }

    public void ExecutePlugins(List<Type> pluginTypes)
    {
        foreach (var type in pluginTypes)
        {
            try
            {
                var instance = Activator.CreateInstance(type) as IPlugin;
                if (instance == null)
                {
                    Console.WriteLine($"Type {type.FullName} does not implement IPlugin");
                    continue;
                }

                Console.WriteLine($"Executing plugin: {type.FullName}");
                instance.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to execute plugin {type.FullName}: {ex.Message}");
            }
        }
    }
}
