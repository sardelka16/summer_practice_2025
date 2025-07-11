namespace plugin1;
using pluginLib;
using pluginLoadAttribute;

[PluginLoad("PluginA", Dependencies = new[] { typeof(PluginB) })]
public class PluginA : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Plugin A executed");
    }
}

[PluginLoad("PluginB")]
public class PluginB : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Plugin B executed");
    }
}

[PluginLoad("PluginC", Dependencies = new[] { typeof(PluginA) })]
public class PluginC : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Plugin C executed");
    }
}