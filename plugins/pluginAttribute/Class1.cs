namespace pluginLoadAttribute;

[AttributeUsage(AttributeTargets.Class)]
public class PluginLoadAttribute : Attribute
{
    public string Name { get; }
    public Type[] Dependencies { get; set; } = Array.Empty<Type>();
    public PluginLoadAttribute(string name)
    {
        Name = name;
    }
}
