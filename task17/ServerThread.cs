using System.Collections.Concurrent;
using System.Threading;
using task18;

namespace task17;

public class ServerThread
{
    private readonly ConcurrentQueue<ICommand> _queue = new ConcurrentQueue<ICommand>();
    private readonly IScheduler? _scheduler;
    private bool _running = false;
    private bool _softStopRequested = false;
    private Thread? _thread;
    public ServerThread(IScheduler scheduler)
    {
        _scheduler = scheduler;
    }

    public void Start()
    {
        _running = true;
        _softStopRequested = false;
        _thread = new Thread(Run);
        _thread.Start();
    }

    private void Run()
    {
        while (_running)
        {
            if (_queue.TryDequeue(out ICommand? cmd))
            {
                cmd.Execute();
                continue;
            }
            if (_scheduler!.HasCommand())
            {
                var cmdSch = _scheduler.Select();
                cmdSch?.Execute();
                continue;
            }
            if (_softStopRequested)
                _running = false;
            else
                Thread.Sleep(50);
        }
    }

    public void EnqueueCommand(ICommand command)
    {
        _queue.Enqueue(command);
    }

    public void RequestHardStop()
    {
        _running = false;
    }

    public void RequestSoftStop()
    {
        _softStopRequested = true;
    }

    public Thread? InternalThread => _thread;
}

public class HardStopCommand : ICommand
{
    private ServerThread _serverThread;
    public HardStopCommand(ServerThread serverThread)
    {
        _serverThread = serverThread;
    }

    public void Execute()
    {
        if (Thread.CurrentThread != _serverThread.InternalThread)
            throw new InvalidOperationException("HardStop должна выполняться в потоке обработки команд!");
        _serverThread.RequestHardStop();
    }
}

public class SoftStopCommand : ICommand
{
    private ServerThread _serverThread;
    public SoftStopCommand(ServerThread serverThread)
    {
        _serverThread = serverThread;
    }

    public void Execute()
    {
        if (Thread.CurrentThread != _serverThread.InternalThread)
            throw new InvalidOperationException("SoftStop должна выполняться в потоке обработки команд!");
        _serverThread.RequestSoftStop();
    }
}

public class LongRunningCommand : ICommand
{
    private readonly IScheduler _scheduler;
    private int _stepsLeft;

    public LongRunningCommand(IScheduler scheduler, int totalSteps)
    {
        _scheduler = scheduler;
        _stepsLeft = totalSteps;
    }

    public void Execute()
    {
        if (_stepsLeft == 0) return;

        Thread.Sleep(100);
        _stepsLeft--;

        if (_stepsLeft > 0)
        {
            _scheduler.Add(this);
        }
    }
    public int StepsLeft => _stepsLeft;
    public bool IsCompleted => _stepsLeft == 0;
}
