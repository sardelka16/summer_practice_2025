namespace task14;

using System;
using System.Threading;

public class ThreadData
{
    public Func<double, double> Function;
    public double Start;
    public double End;
    public double Step;
    public double PartialResult;
}
public class DefiniteIntegral
{
    private static double totalResult = 0.0;
    private static readonly object lockObj = new object();
    private static Barrier barrier;

    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        totalResult = 0.0;
        barrier = new Barrier(threadsNumber + 1);

        double intervalLength = (b - a) / threadsNumber;
        Thread[] threads = new Thread[threadsNumber];

        for (int i = 0; i < threadsNumber; i++)
        {
            double start = a + i * intervalLength;
            double end = (i == threadsNumber - 1) ? b : start + intervalLength;

            ThreadData data = new ThreadData
            {
                Function = function,
                Start = start,
                End = end,
                Step = step
            };

            threads[i] = new Thread(ComputePartialIntegral);
            threads[i].Start(data);
        }

        barrier.SignalAndWait();
        return totalResult;
    }

    private static void ComputePartialIntegral(object data)
    {
        ThreadData threadData = (ThreadData)data;
        double sum = 0.0;
        double x = threadData.Start;
        double end = threadData.End;

        while (x < end)
        {
            double nextX = Math.Min(x + threadData.Step, end);
            sum += (threadData.Function(x) + threadData.Function(nextX)) * (nextX - x) / 2;
            x = nextX;
        }

        threadData.PartialResult = sum;

        lock (lockObj)
        {
            totalResult += sum;
        }

        barrier.SignalAndWait();
    }
}