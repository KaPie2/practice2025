using CommandLib;

namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    private string _targetFolder;
    public long TotalSize { get; set; }

    public DirectorySizeCommand(string targetFolder)
    {
        _targetFolder = targetFolder;
        TotalSize = 0;
    }

    public void Execute()
    {
        if (Directory.Exists(_targetFolder))
        {
            TotalSize = new DirectoryInfo(_targetFolder)
            .GetFiles("*", SearchOption.AllDirectories)
            .Sum(file => file.Length);
        }
    }
}

public class FindFilesCommand : ICommand
{
    private string _searchLocation;
    private string _mask;
    public string[] ResultFiles { get; set; } = new string[0];

    public FindFilesCommand(string searchLocation, string mask)
    {
        _searchLocation = searchLocation;
        _mask = mask;
    }

    public void Execute()
    {
        if (Directory.Exists(_searchLocation))
        {
            ResultFiles = Directory.GetFiles(_searchLocation, _mask, SearchOption.AllDirectories);
        }
        
    }
}
