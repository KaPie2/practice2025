using task17;
using task18;
using ScottPlot;

class Program
{
    static void Main()
    {
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);

        int commandsCount = 5;
        int stepsPerCommand = 10;

        var commands = new List<LongRunningCommand>();

        for (int i = 0; i < commandsCount; i++)
            commands.Add(new LongRunningCommand(scheduler, stepsPerCommand));

        server.Start();

        foreach (var cmd in commands)
            server.EnqueueCommand(cmd);
        while (commands.Exists(c => !c.IsCompleted))
            Thread.Sleep(50);

        server.RequestSoftStop();
        server.InternalThread?.Join();

        var executionTimes = commands.Select(cmd => cmd.StepExecutionTimesMs).ToList();

        var plt = new ScottPlot.Plot();
        double[] commandNumbers = Enumerable.Range(1, commandsCount).Select(x => (double)x).ToArray();
        double[] avgTimes = commands.Select(c => c.StepExecutionTimesMs.Average()).ToArray();
        plt.Title("Среднее время выполнения команд");
        plt.XLabel("Среднее время (мс)");
        plt.YLabel("Номер команды");
        plt.Add.Scatter(avgTimes, commandNumbers);
        plt.SavePng(@"C:\Users\pirog\practice2025\execution_times_plot.png", 600, 300);

        string report = string.Empty;
        for (int i = 0; i < commands.Count; i++)
        {
            var times = executionTimes[i];
            report += $"Команда №{i + 1}:\n";
            report += $"  Всего шагов: {times.Count}\n";
            report += $"  Среднее время: {times.Average():F2} мс\n";
            report += $"  Минимальное время: {times.Min()} мс\n";
            report += $"  Максимальное время: {times.Max()} мс\n";
            report += $"  Общее время: {times.Sum()} мс\n\n";
        }
        File.WriteAllText(@"C:\Users\pirog\practice2025\report.txt", report);
    }
}
