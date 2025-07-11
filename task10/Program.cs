namespace  task10;
using pluginLoader;
class Program
{
    public static void Main()
    {
        string pathToPlugins = "C:/Users/andrei/Desktop/Practice/summer_practice_2025/plugins";
        var loader = new PluginLoader(pathToPlugins);
        loader.LoadPlugins();
    }
}