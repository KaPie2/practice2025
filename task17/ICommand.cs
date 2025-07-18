using task18;
using System.Diagnostics;

namespace task17;

public interface ICommand
{
    void Execute();
}

public class TestCommand : ICommand
{
    private readonly int id;
    private readonly IScheduler scheduler;
    private readonly int maxCalls;
    private int counter = 0;
    private Stopwatch sw = new Stopwatch();

    public TestCommand(int id, IScheduler scheduler, int maxCalls)
    {
        this.id = id;
        this.scheduler = scheduler;
        this.maxCalls = maxCalls;
    }

    public int Id => id;

    public int ExecuteCount => counter;

    public long ElapsedMs => sw.ElapsedMilliseconds;

    public void Execute()
    {
        if (counter == 0)
            sw.Start();

        Console.WriteLine($"Поток {id} вызов {++counter}");

        Thread.Sleep(new Random().Next(1, 10));

        if (counter < maxCalls)
        {
            scheduler.Add(this);
        }
        else
        {
            sw.Stop();
        }
    }
}
