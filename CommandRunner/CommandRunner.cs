using CommandLib;
using System.Reflection;

class Program
{
    static void Main()
    {
        var assembly = Assembly.LoadFrom("FileSystemCommands.dll");

        var sizeCmdType = assembly.GetType("FileSystemCommands.DirectorySizeCommand")!;
        var sizeCommand = (ICommand)Activator.CreateInstance(sizeCmdType, @"C:\Users\pirog\practice2025\")!;
        sizeCommand.Execute();

        var sizeResult = (long)sizeCmdType.GetProperty("TotalSize")!.GetValue(sizeCommand)!;
        Console.WriteLine($"Размер директории: {sizeResult}");

        var findCmdType = assembly.GetType("FileSystemCommands.FindFilesCommand")!;
        var findCommand = (ICommand)Activator.CreateInstance(findCmdType, @"C:\Users\pirog\practice2025\CommandRunner", "*.cs")!;
        findCommand.Execute();

        var foundFiles = (string[])findCmdType.GetProperty("ResultFiles")!.GetValue(findCommand)!;
        Console.WriteLine("\nНайденные файлы:");
        if (foundFiles.Length == 0) Console.WriteLine("Файлы не найдены");
        else
        {
            foreach (var file in foundFiles)
            {
                Console.WriteLine(file);
            }
        }
    }
}
