using System;
using System.Reflection;
using System.Collections.Generic;

namespace task05;

public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    public IEnumerable<string> GetPublicMethods() { }
    public IEnumerable<string> GetMethodParams(string methodname) { }
    public IEnumerable<string> GetAllFields() { }
    public IEnumerable<string> GetProperties() { }
    public bool HasAttribute<T>() where T : Attribute { }
}