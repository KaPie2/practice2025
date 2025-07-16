using Xunit;
using task17;

namespace task17tests;

public class ServerThreadTests
{
    public class TestCommand : ICommand
    {
        public void Execute() { }
    }

    private bool IsThreadRunning(Thread? thread) => thread != null && thread.IsAlive;

    [Fact]
    public void HardStopCommand_StopsThreadImmediately()
    {
        var server = new ServerThread();
        server.Start();

        Thread.Sleep(100);

        server.EnqueueCommand(new HardStopCommand(server));

        var internalThread = server.InternalThread;
        internalThread?.Join(1000);

        Assert.False(IsThreadRunning(internalThread));
    }

    [Fact]
    public void SoftStopCommand_StopsThreadAfterAllCommands()
    {
        var server = new ServerThread();
        server.Start();

        server.EnqueueCommand(new TestCommand());

        server.EnqueueCommand(new SoftStopCommand(server));

        var internalThread = server.InternalThread;
        internalThread?.Join(1000);

        Assert.False(IsThreadRunning(internalThread));
    }

    [Fact]
    public void HardStopCommand_ThrowsIfCalledOutsideCorrectThread()
    {
        var server = new ServerThread();
        server.Start();

        var hardStopCmd = new HardStopCommand(server);

        var ex = Assert.Throws<InvalidOperationException>(() => hardStopCmd.Execute());
        Assert.Contains("HardStop должна выполняться", ex.Message);

        server.RequestHardStop();
        var internalThread = server.InternalThread;
        internalThread?.Join(1000);
    }

    [Fact]
    public void SoftStopCommand_ThrowsIfCalledOutsideCorrectThread()
    {
        var server = new ServerThread();
        server.Start();

        var softStopCmd = new SoftStopCommand(server);

        var ex = Assert.Throws<InvalidOperationException>(() => softStopCmd.Execute());
        Assert.Contains("SoftStop должна выполняться", ex.Message);

        server.RequestHardStop();
        var internalThread = server.InternalThread;
        internalThread?.Join(1000);
    }
}
