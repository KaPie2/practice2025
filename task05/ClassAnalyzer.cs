using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace task05;

public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    public IEnumerable<string> GetPublicMethods()
    {
        return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Select(m => m.Name);
    }
    public IEnumerable<string> GetMethodParams(string methodname)
    {
        var method = _type.GetMethods().FirstOrDefault(m => m.Name == methodname);
        if (method == null)
        {
            return Enumerable.Empty<string>();
        }
        return method.GetParameters().Select(m => $"{m.ParameterType.Name} {m.Name}");
    }
    public IEnumerable<string> GetAllFields()
    {
        return _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Select(m => m.Name);
    }
    public IEnumerable<string> GetProperties()
    {
        return _type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(m => m.Name);
    }
    public bool HasAttribute<T>() where T : Attribute
    {
        return _type.IsDefined(typeof(T));
    }
}
