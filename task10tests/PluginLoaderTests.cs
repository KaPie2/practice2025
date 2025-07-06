using Xunit;
using task10;

namespace task10tests;

[PluginLoad]
public class SimplePlugin : IPluginCommand
{
    public void Execute()
    {
        Console.WriteLine("Простой плагин выполнился!");
    }
}

[PluginLoad("SimplePlugin")]
public class DependentPlugin : IPluginCommand
{
    public void Execute()
    {
        Console.WriteLine("Зависимый плагин выполнился!");
    }
}

public class PluginLoaderTests: IDisposable
{
    private string _testFolder = Path.Combine(Directory.GetCurrentDirectory(), "TestPlugins");
    public PluginLoaderTests()
    {
        if (Directory.Exists(_testFolder)) Directory.Delete(_testFolder, true);
        Directory.CreateDirectory(_testFolder);
    }
    public void Dispose()
    {
        if (Directory.Exists(_testFolder)) Directory.Delete(_testFolder, true);
    }

    [Fact]
    public void CanLoadSimplePlugin()
    {
        File.Copy(typeof(SimplePlugin).Assembly.Location, Path.Combine(_testFolder, "SimplePlugin.dll"), overwrite: true);

        var output = new StringWriter();
        Console.SetOut(output);

        var loader = new PluginLoader(_testFolder);
        loader.LoadAndRunPlugins();

        var consoleOutput = output.ToString();
        Assert.Contains("Простой плагин выполнился", consoleOutput);
    }

    [Fact]
    public void LoadsPluginsInCorrectOrder()
    {
        File.Copy(typeof(SimplePlugin).Assembly.Location, Path.Combine(_testFolder, "SimplePlugin.dll"), overwrite: true);
        File.Copy(typeof(DependentPlugin).Assembly.Location, Path.Combine(_testFolder, "DependentPlugin.dll"), overwrite: true);

        var output = new StringWriter();
        Console.SetOut(output);

        var loader = new PluginLoader(_testFolder);
        loader.LoadAndRunPlugins();

        var consoleOutput = output.ToString();
        int indexSimple = consoleOutput.IndexOf("Простой плагин выполнился");
        int indexDependent = consoleOutput.IndexOf("Зависимый плагин выполнился");

        Assert.True(indexSimple >= 0);
        Assert.True(indexDependent >= 0);
        Assert.True(indexSimple < indexDependent);
    }
}
