using System;
using System.Reflection;

namespace task07;

public class DisplayNameAttribute : Attribute
{
    public string DisplayName = string.Empty;
    public DisplayNameAttribute(string DisplayName)
    {
        this.DisplayName = DisplayName;
    }
}

public class VersionAttribute : Attribute
{
    public int Major { get; }
    public int Minor { get; }
    
    public VersionAttribute(int Major, int Minor)
    {
        this.Major = Major;
        this.Minor = Minor;
    }
}

[DisplayName("Пример класса")]
[Version(1, 0)]
public class SampleClass
{
    [DisplayName("Тестовый метод")]
    public void TestMethod() { }
    
    [DisplayName("Числовое свойство")]
    public int Number { get; set; }
}

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        var display_name = type.GetCustomAttribute<DisplayNameAttribute>();
        if (display_name != null) Console.WriteLine(display_name.DisplayName);

        var version = type.GetCustomAttribute<VersionAttribute>();
        if (version != null) Console.WriteLine($"{version.Major}.{version.Minor}");

        foreach (var method in type.GetMethods())
        {
            var method_attr = method.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();
            if (method_attr != null) Console.WriteLine($"{method.Name}: {method_attr.DisplayName}");
        }

        foreach (var property in type.GetProperties())
        {
            var property_attr = property.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();
            if (property_attr != null) Console.WriteLine($"{property.Name}: {property_attr.DisplayName}");
        }
    }
}
