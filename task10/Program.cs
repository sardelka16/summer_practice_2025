namespace  task10;
using pluginLoader;
class Program
{
    public static void Main()
    {
        string pathToPlugins = "../plugins";
        var loader = new PluginLoader(pathToPlugins);
        loader.LoadPlugins();
    }
}