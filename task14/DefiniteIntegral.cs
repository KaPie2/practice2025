using System.Diagnostics;
using System.Threading;

namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        double totalResult = 0.0;
        int cntSteps = (int)Math.Ceiling((b - a) / step);
        double lengthPerThread = (b - a) / threadsNumber;
        Barrier barrier = new Barrier(threadsNumber + 1);

        for (int i = 0; i < threadsNumber; i++)
        {
            double start = a + i * lengthPerThread;
            double end = (i == threadsNumber - 1) ? b : start + lengthPerThread;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                double sumOfAreas = 0.0;
                for (double xStart = start; xStart < end; xStart += step)
                {
                    double xEnd = Math.Min(xStart + step, end);
                    sumOfAreas += (function(xStart) + function(xEnd)) / 2 * (xEnd - xStart);
                }

                Interlocked.Exchange(ref totalResult, totalResult + sumOfAreas);
                barrier.SignalAndWait();
            });
        }
        barrier.SignalAndWait();
        return totalResult;
    }
}
