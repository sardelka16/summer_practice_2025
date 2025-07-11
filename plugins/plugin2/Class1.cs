namespace plugin2;

using plugin1;
using pluginLib;
using pluginLoadAttribute;

[PluginLoad("PluginD", Dependencies = new[] { typeof(PluginC)})]
public class PluginD : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Plugin D executed");
    }
}


public class NotPlugin
{
    public void Execute()
    {
        Console.WriteLine("NotPlugin executed");
    }
}