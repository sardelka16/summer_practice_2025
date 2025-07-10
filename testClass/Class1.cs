namespace testClass;
using task07;

[DisplayName("Тестовый класс для 9 задания")]
[Version(1, 0)]
public class TestClass
{
    public string TestField;
    
    public TestClass() {}
    public TestClass(int param) {}

    [Obsolete]
    public void TestMethod() {}
    
    public int TestProperty { get; set; }
}
