using ScottPlot;
using task17;
using task18;

namespace task19;

class Program
{
    static void Main()
    {
        int commandCount = 5;
        int callsPerCommand = 3;

        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);

        var commands = new List<TestCommand>();
        for (int i = 1; i <= commandCount; i++)
        {
            var cmd = new TestCommand(i, scheduler, callsPerCommand);
            scheduler.Add(cmd);
            commands.Add(cmd);
        }

        server.Start();

        while (commands.Any(c => c.ExecuteCount < callsPerCommand))
            Thread.Sleep(10);

        server.EnqueueCommand(new HardStopCommand(server));
        server.InternalThread?.Join();

        double[] realTimes = commands.Select(cmd => (double)cmd.ElapsedMs).ToArray();
        int[] commandNumbers = { 1, 2, 3, 4, 5 };

        var plt = new ScottPlot.Plot();
        plt.Title("Выполнение TestCommand");
        plt.XLabel("Время (мс)");
        plt.YLabel("Количество команд");
        plt.Add.Scatter(realTimes, commandNumbers.Select(x => (double)x).ToArray());

        plt.SavePng(@"C:\Users\pirog\practice2025\command_execution_plot.png", 600, 300);

        double totalExecutionTime = realTimes.Sum();
        double averageTimePerCommand = realTimes.Average();
        string report =
            $"Отчет выполнения команд:\n" +
            $"Количество команд: {commandCount}\n" +
            $"Вызовов каждой команды: {callsPerCommand}\n" +
            $"Общее количество вызовов Execute: {commandCount * callsPerCommand}\n" +
            $"Тип остановки: HardStop\n\n" +
            $"Максимальное время выполнения: {realTimes.Max()} мс\n" +
            $"Минимальное время выполнения: {realTimes.Min()} мс\n" +
            $"Среднее время на команду: {averageTimePerCommand:F2} мс\n" +
            $"Общее время выполнения всех команд: {totalExecutionTime} мс\n";
        File.WriteAllText(@"C:\Users\pirog\practice2025\command_report.txt", report);
    }
}
