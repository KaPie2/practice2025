using Xunit;
using Moq;
using task05;

namespace task05tests;

public class TestClass
{
    public int PublicField;
    private string _privateField = string.Empty;
    public int Property { get; set; }

    public void Method() { }
    public int MethodWithParams(string input, int count) => 0;
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
    }

    [Fact]
    public void GetMethodParams_ReturnsParametersAndReturnType()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parameters = analyzer.GetMethodParams("MethodWithParams");

        Assert.Contains("String input", parameters);
        Assert.Contains("Int32 count", parameters);
    }

    [Fact]
    public void GetMethodParams_ReturnsEmptyParameters()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parameters = analyzer.GetMethodParams("Method");
        Assert.Empty(parameters);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetAllProperties_ReturnsCorrectProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties();

        Assert.Contains("Property", properties);
    }

    [Fact]
    public void HasAttribute_ReturnsCorrectProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        var attribute = analyzer.HasAttribute<SerializableAttribute>();
        Assert.True(attribute);
    }
}