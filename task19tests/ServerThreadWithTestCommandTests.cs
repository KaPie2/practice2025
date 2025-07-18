using Xunit;
using task17;
using task18;

namespace task19tests;

public class ServerThreadWithTestCommandTests
{
    [Fact]
    public void FiveCommands_ExecuteThreeTimesEach_ThenHardStop()
    {
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);

        var commands = new TestCommand[5];
        for (int i = 0; i < 5; i++)
        {
            commands[i] = new TestCommand(i + 1, scheduler, 3);
            scheduler.Add(commands[i]);
        }

        server.Start();
        Thread.Sleep(2000);
        server.EnqueueCommand(new HardStopCommand(server));

        bool finished = server.InternalThread!.Join(5000);
        Assert.True(finished);

        foreach (var cmd in commands)
        {
            Assert.Equal(3, cmd.ExecuteCount);
        }
    }
}
