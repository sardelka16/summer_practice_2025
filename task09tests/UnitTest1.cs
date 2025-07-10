namespace task09tests;

using task09;
using testClass;
public class AssemblyAnalyzerTests : IDisposable
{
    private readonly string _testAssemblyPath;
    private readonly StringWriter _outputWriter;
    private readonly TextWriter _originalOutput;

    public AssemblyAnalyzerTests()
    {
        // Подготовка тестовой среды
        _testAssemblyPath = "C:/Users/andrei/Desktop/Practice/summer_practice_2025/testClass/bin/Debug/net9.0/testClass.dll";
        _outputWriter = new StringWriter();
        _originalOutput = Console.Out;
        Console.SetOut(_outputWriter);
    }

    public void Dispose()
    {
        // Восстановление стандартного вывода
        Console.SetOut(_originalOutput);
        _outputWriter.Dispose();
        
    }

    private string CreateTestAssembly()
    {
        // В реальном проекте здесь должна быть компиляция тестовой сборки
        return Path.Combine("testdata", "testClass.dll"); // Пример пути
    }

    [Fact]
    public void AnalyzeAssembly_ShouldNotThrow_WhenValidAssembly()
    {
        var exception = Record.Exception(() => Program.AnalyzeAssembly(_testAssemblyPath));
        Assert.Null(exception);
    }

    [Fact]
    public void AnalyzeAssembly_ShouldOutputAssemblyName()
    {
        Program.AnalyzeAssembly(_testAssemblyPath);
        var output = _outputWriter.ToString();
        Assert.Contains("=== Библиотека: testClass ===", output);
    }

    [Fact]
    public void PrintAttributes_ShouldDisplayAttributeNames()
    {
        var type = typeof(TestClass);
        Program.PrintAttributes(type.GetCustomAttributes(inherit: false));
        var output = _outputWriter.ToString();
        Assert.Contains("[Атрибуты]", output);
        Assert.Contains("DisplayNameAttribute", output);
        Assert.Contains("VersionAttribute", output);
    }

    [Fact]
    public void PrintConstructors_ShouldListAllConstructors()
    {
        var type = typeof(TestClass);
        Program.PrintConstructors(type);
        var output = _outputWriter.ToString();
        Assert.Contains("[Конструкторы]", output);
        Assert.Contains("TestClass()", output);
        Assert.Contains("TestClass(Int32 param, )", output);
    }

    [Fact]
    public void PrintMethods_ShouldIncludeObsoleteAttribute()
    {
        var type = typeof(TestClass);
        Program.PrintMethods(type);
        var output = _outputWriter.ToString();
        Assert.Contains("[Методы]", output);
        Assert.Contains("ObsoleteAttribute", output);
        Assert.Contains("Void TestMethod()", output);
    }

    [Fact]
    public void PrintParameters_ShouldHandleParameterlessMethods()
    {
        var method = typeof(TestClass).GetMethod("TestMethod");
        Program.PrintParameters(method.GetParameters());
        var output = _outputWriter.ToString();
        Assert.Equal("", output.Trim());
    }
}