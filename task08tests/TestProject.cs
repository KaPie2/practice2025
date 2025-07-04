using Xunit;
using FileSystemCommands;

namespace task08tests;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        if (Directory.Exists(testDir)) Directory.Delete(testDir, true);
        Directory.CreateDirectory(testDir);

        string file1 = Path.Combine(testDir, "test1.txt");
        string file2 = Path.Combine(testDir, "test2.txt");

        File.WriteAllText(file1, "Hello");
        File.WriteAllText(file2, "World");

        var command = new DirectorySizeCommand(testDir);
        command.Execute();

        long result = new FileInfo(file1).Length + new FileInfo(file2).Length;
        Assert.Equal(result, command.TotalSize);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        if (Directory.Exists(testDir)) Directory.Delete(testDir, true);
        Directory.CreateDirectory(testDir);

        string file1 = Path.Combine(testDir, "file1.txt");
        string file2 = Path.Combine(testDir, "file2.log");

        File.WriteAllText(file1, "Text");
        File.WriteAllText(file2, "Log");

        var command = new FindFilesCommand(testDir, "*.txt");
        command.Execute();

        Assert.NotNull(command.ResultFiles);
        Assert.Single(command.ResultFiles);
        Assert.Contains(file1, command.ResultFiles);
        Assert.DoesNotContain(file2, command.ResultFiles);

        Directory.Delete(testDir, true);
    }
}
