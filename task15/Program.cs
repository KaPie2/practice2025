using ScottPlot;
using System.Diagnostics;
using task14;

namespace task15;

class Program
{
    static void Main()
    {
        double a = -100;
        double b = 100;
        Func<double, double> func = Math.Sin;
        double exactValue = 0;

        double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
        int fixedThreads = 4;

        double minStep = 0;
        foreach (var step in steps)
        {
            double result = DefiniteIntegral.Solve(a, b, func, step, fixedThreads);
            double approximationError = Math.Abs(result - exactValue);
            if (approximationError <= 1e-4)
            {
                minStep = step;
                break;
            }
        }

        int[] threadCounts = { 1, 2, 4, 8, 16 };
        int repeat = 10;

        double[] avgTimes = new double[threadCounts.Length];

        for (int i = 0; i < threadCounts.Length; i++)
        {
            int threads = threadCounts[i];
            double totalTime = 0;
            for (int j = 0; j < repeat; j++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                double res = DefiniteIntegral.Solve(a, b, func, minStep, threads);
                sw.Stop();
                totalTime += sw.Elapsed.TotalMilliseconds;
            }
            avgTimes[i] = totalTime / repeat;
        }

        double minTime = avgTimes[0];
        int optimalThreads = threadCounts[0];
        for (int i = 1; i < avgTimes.Length; i++)
        {
            if (avgTimes[i] < minTime)
            {
                minTime = avgTimes[i];
                optimalThreads = threadCounts[i];
            }
        }

        var plot = new Plot();
        plot.Add.Scatter(avgTimes, threadCounts.Select(x => (double)x).ToArray());
        plot.Title("Время выполнения функции Solve");
        plot.XLabel("Время (мс)");
        plot.YLabel("Количество потоков");
        plot.SavePng(@"C:\Users\pirog\practice2025\performance_plot.png", 600, 400);

        double singleThreadTime = 0;
        for (int j = 0; j < repeat; j++)
        {
            Stopwatch sw = Stopwatch.StartNew();
            double res = DefiniteIntegral.SolveSingleThread(a, b, func, minStep);
            sw.Stop();
            singleThreadTime += sw.Elapsed.TotalMilliseconds;
        }
        singleThreadTime /= repeat;

        double diffPercent = ((singleThreadTime - minTime) / singleThreadTime) * 100;

        string report = $"Выбранный размер шага: {minStep}\n" +
                        $"Оптимальное число потоков: {optimalThreads}\n" +
                        $"Время работы оптимальной многопоточной версии: {minTime} ms\n" +
                        $"Время работы однопоточной версии: {singleThreadTime} ms\n" +
                        $"Разница в скорости (в процентах): {diffPercent:F2}%\n";

        File.WriteAllText(@"C:\Users\pirog\practice2025\result_report.txt", report);
    }
}
