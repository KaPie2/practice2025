using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace task11;

public interface ICalculator
{
    int Add(int a, int b);
    int Minus(int a, int b);
    int Mul(int a, int b);
    int Div(int a, int b);
}

public static class CalculatorLoader
{
    public static ICalculator CreateCalculator(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var refs = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location));

        var comp = CSharpCompilation.Create("CalcAssembly", new[] { tree }, refs,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var ms = new MemoryStream();
        comp.Emit(ms);
        ms.Seek(0, SeekOrigin.Begin);

        var assembly = Assembly.Load(ms.ToArray());
        var type = assembly.GetType("Calculator");
        return (ICalculator)Activator.CreateInstance(type!)!;
    }
}
