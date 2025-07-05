namespace task07;

using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; }

    public DisplayNameAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute : Attribute
{
    public int Major { get; }
    public int Minor { get; }

    public VersionAttribute(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }
}

[DisplayName("Пример класса")]
[Version(1, 0)]
public class SampleClass
{
    [DisplayName("Тестовый метод")]
    public void TestMethod()
    {
        Console.WriteLine("Метод TestMethod вызван.");
    }

    [DisplayName("Числовое свойство")]
    public int Number { get; set; }

    public string? DefaultProperty { get; set; } 
}

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        var displayNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
        if (displayNameAttr != null)
        {
            Console.WriteLine($"Отображаемое имя: {displayNameAttr.DisplayName}");
        }

        var versionAttr = type.GetCustomAttribute<VersionAttribute>();
        if (versionAttr != null)
        {
            Console.WriteLine($"Версия: {versionAttr.Major}.{versionAttr.Minor}");
        }

        Console.WriteLine("\nМетоды:");
        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
        {
            var methodAttr = method.GetCustomAttribute<DisplayNameAttribute>();
            Console.WriteLine($" - {method.Name}" + 
                (methodAttr != null ? $" [{methodAttr.DisplayName}]" : ""));
        }

        Console.WriteLine("\nСвойства:");
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propAttr = prop.GetCustomAttribute<DisplayNameAttribute>();
            Console.WriteLine($" - {prop.Name}" + 
                (propAttr != null ? $" [{propAttr.DisplayName}]" : ""));
        }
    }
}