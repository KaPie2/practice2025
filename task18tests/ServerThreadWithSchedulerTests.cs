using System.Threading;
using Xunit;
using task17;
using task18;

public class ServerThreadWithSchedulerTests
{
    private ServerThread _server;
    private RoundRobinScheduler _scheduler;

    public ServerThreadWithSchedulerTests()
    {
        _scheduler = new RoundRobinScheduler();
        _server = new ServerThread(_scheduler);
    }

    private class TestCommand : ICommand
    {
        public bool Executed = false;
        public void Execute() => Executed = true;
    }

    [Fact]
    public void SimpleCommand_Executes()
    {
        var cmd = new TestCommand();
        _server.EnqueueCommand(cmd);
        _server.Start();

        Thread.Sleep(200);

        _server.RequestHardStop();
        _server.InternalThread?.Join();

        Assert.True(cmd.Executed);
    }

    [Fact]
    public void LongRunningCommand_CompletesAllSteps()
    {
        int totalSteps = 3;
        var longCmd = new LongRunningCommand(_scheduler, totalSteps);
        _server.EnqueueCommand(longCmd);
        _server.Start();

        Thread.Sleep(totalSteps * 150);

        _server.RequestHardStop();
        _server.InternalThread?.Join();

        Assert.True(longCmd.IsCompleted);
        Assert.Equal(0, longCmd.StepsLeft);
    }

    [Fact]
    public void Scheduler_ExecutesMultipleLongCommands()
    {
        var longCmd1 = new LongRunningCommand(_scheduler, 2);
        var longCmd2 = new LongRunningCommand(_scheduler, 2);

        _server.EnqueueCommand(longCmd1);
        _server.EnqueueCommand(longCmd2);
        _server.Start();

        Thread.Sleep(600);

        _server.RequestHardStop();
        _server.InternalThread?.Join();

        Assert.True(longCmd1.IsCompleted);
        Assert.True(longCmd2.IsCompleted);

        Assert.Equal(0, longCmd1.StepsLeft);
        Assert.Equal(0, longCmd2.StepsLeft);
    }

    [Fact]
    public void SoftStop_WaitsForCompletion()
    {
        var longCmd = new LongRunningCommand(_scheduler, 5);
        _server.EnqueueCommand(longCmd);
        _server.Start();

        _server.RequestSoftStop();

        _server.InternalThread?.Join(3000);
        Assert.NotNull(_server.InternalThread);
        Assert.False(_server.InternalThread.IsAlive);
        
        Assert.True(longCmd.IsCompleted);
        Assert.Equal(0, longCmd.StepsLeft);
    }
}
