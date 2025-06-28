namespace task05;
using Xunit;

public class TestClass
{
    public int PublicField;
    private string? _privateField;
    public int Property { get; set; }

    public void Method() { }
    public void MethodWithParameters(string parametr1, int parametr2) { }
}

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
        Assert.Contains("MethodWithParameters", methods);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateAndPublicFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
        Assert.Contains("PublicField", fields);
    }

    [Fact]
    public void GetMethodParams_ReturnsCorrectParameterNames()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parameters = analyzer.GetMethodParams("MethodWithParameters");

        Assert.Contains("parametr1", parameters);
        Assert.Contains("parametr2", parameters);
        Assert.Equal(2, parameters.Count());
    }

    [Fact]
    public void GetProperties_ReturnsPropertyNames()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties();

        Assert.Contains("Property", properties);
    }

    [Fact]
    public void HasAttribute_ReturnsTrueForAppliedAttribute()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));

        Assert.True(analyzer.HasAttribute<SerializableAttribute>());
    }

    [Fact]
    public void HasAttribute_ReturnsFalseForNotAppliedAttribute()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));

        Assert.False(analyzer.HasAttribute<SerializableAttribute>());
    }
}