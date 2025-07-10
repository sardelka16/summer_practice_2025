namespace task09;
using task07;
using System;
using System.Reflection;
public class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: AssemblyAnalyzer <path_to_assembly.dll>"); //Поменять потом
            return;
        }
        string pathToLib = args[0];
        AnalyzeAssembly(pathToLib);
    }
    
    public static void AnalyzeAssembly(string pathToLib)
    {
        Assembly assembly = Assembly.LoadFrom(pathToLib);
        Console.WriteLine($"=== Библиотека: {assembly.GetName().Name} ===");
        Type[] types = assembly.GetTypes();

        foreach (var type in types)
        {
            if (!type.IsPublic)
                continue;

            Console.WriteLine($"\n[Класс] {type.FullName}");
            PrintAttributes(type.GetCustomAttributes());
            PrintConstructors(type);
            PrintMethods(type);
            PrintProperties(type);
            PrintFields(type);
        }
    }

    public static void PrintAttributes(IEnumerable<object> attributes)
    {
        if (attributes.Count() > 0)
        {
            Console.WriteLine("  [Атрибуты]");
            foreach (var attr in attributes)
            {
                Console.WriteLine($"    {attr.GetType().Name}");
            }
        }
    }

    public static void PrintConstructors(Type type)
    {
        ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        if (constructors.Length > 0)
        {
            Console.WriteLine("  [Конструкторы]");
            foreach (var constr in constructors)
            {
                Console.Write($"    {type.Name}(");
                PrintParameters(constr.GetParameters());
                Console.WriteLine(")");
                PrintAttributes(constr.GetCustomAttributes());
            }
        }
    }

    public static void PrintMethods(Type type)
    {
        MethodInfo[] methods = type.GetMethods(
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
            .Where(m => !m.IsSpecialName)
            .ToArray();
        if (methods.Length > 0)
        {
            Console.WriteLine("  [Методы]");
            foreach (var method in methods)
            {
                Console.Write($"    {method.ReturnType.Name} {method.Name}(");
                PrintParameters(method.GetParameters());
                Console.WriteLine(")");
                PrintAttributes(method.GetCustomAttributes());
            }
        }
    }

    public static void PrintProperties(Type type)
    {
        PropertyInfo[] properties = type.GetProperties(
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            
        if (properties.Length > 0)
        {
            Console.WriteLine("  [Свойства]");
            foreach (var prop in properties)
            {
                Console.WriteLine($"    {prop.PropertyType.Name} {prop.Name}");
                PrintAttributes(prop.GetCustomAttributes());
            }
        }
    }

    public static void PrintFields(Type type)
    {
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        if (fields.Length > 0)
        {
            Console.WriteLine("  [Поля]");
            foreach (var field in fields)
            {
                Console.WriteLine($"    {field.FieldType.Name} {field.Name}");
                PrintAttributes(field.GetCustomAttributes());
            }
        }
    }

    public static void PrintParameters(ParameterInfo[] parameters)
    {
        foreach (var param in parameters)
        {
            Console.Write($"{param.ParameterType.Name} {param.Name}");
            if (param.HasDefaultValue)
                Console.Write($" = {param.DefaultValue ?? "null"}");
            Console.Write(", ");
        }
    }
}