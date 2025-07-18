using task17;
using System.Collections.Concurrent;

namespace task18;

public class RoundRobinScheduler : IScheduler
{
    private readonly ConcurrentQueue<ICommand> _commands = new ConcurrentQueue<ICommand>();

    public bool HasCommand() => _commands.TryPeek(out _);

    public ICommand? Select()
    {
        _commands.TryDequeue(out ICommand? cmd);
        return cmd;
    }

    public void Add(ICommand cmd)
    {
        if (cmd != null)
            _commands.Enqueue(cmd);
    }
}
