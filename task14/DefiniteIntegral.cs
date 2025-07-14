using System.Diagnostics;
using System.Threading;

namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        int totalSteps = (int)Math.Ceiling((b - a) / step);
        double[] partialSums = new double[threadsNumber];

        int stepsPerThread = totalSteps / threadsNumber;
        int remainder = totalSteps % threadsNumber;

        Parallel.For(0, threadsNumber, i =>
        {
            int startStep = i * stepsPerThread;
            int endStep = startStep + stepsPerThread;
            if (i == threadsNumber - 1)
                endStep += remainder;

            double localSum = 0;

            for (int j = startStep; j < endStep; j++)
            {
                double xStart = a + j * step;
                double xEnd = Math.Min(xStart + step, b);
                localSum += (function(xStart) + function(xEnd)) / 2 * (xEnd - xStart);
            }

            partialSums[i] = localSum;
        });

        double totalSum = 0;
        foreach (var sum in partialSums)
            totalSum += sum;

        return totalSum;
    }

    public static double SolveSingleThread(double a, double b, Func<double, double> function, double step)
    {
        double sum = 0.0;
        for (double x = a; x < b; x += step)
        {
            double xEnd = Math.Min(x + step, b);
            sum += (function(x) + function(xEnd)) / 2 * (xEnd - x);
        }
        return sum;
    }
}
