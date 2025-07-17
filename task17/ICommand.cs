using task18;

namespace task17;

public interface ICommand
{
    void Execute();
}

public class TestCommand: ICommand
{
    private readonly int id;
    private readonly IScheduler scheduler;
    private readonly int maxCalls;
    private int counter = 0;

    public TestCommand(int id, IScheduler scheduler, int maxCalls)
    {
        this.id = id;
        this.scheduler = scheduler;
        this.maxCalls = maxCalls;
    }

    public int ExecuteCount => counter;
    
    public void Execute()
    {
        Console.WriteLine($"Поток {id} вызов {++counter}");
        if (counter < maxCalls)
        {
            scheduler.Add(this);
        }
    }
}
