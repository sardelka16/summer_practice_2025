using task14;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using ScottPlot;
class Program
{
    private static readonly double a = -100;
    private static readonly double b = 100;
    private static readonly Func<double, double> SIN = Math.Sin;
    private static readonly double eps = 1e-4;


    static void Main()
    {

        var step = FindBestStep();
        var threadsResults = TimeOfSolveResearch(step);
        var best = threadsResults.OrderBy(x => x.Value).First();

        var plot = new Plot();
        double[] threads = threadsResults.Select(x => (double)x.Key).ToArray();
        double[] times = threadsResults.Select(x => x.Value).ToArray();
        var scatterPlot = plot.Add.Scatter(threads, times);
        scatterPlot.LegendText = "Время выполнения";
        scatterPlot.MarkerSize = 7;
        scatterPlot.LineWidth = 2;
        plot.Title("Зависимость времени выполнения от количества потоков");
        plot.YLabel("Время выполнения (мс)");
        plot.XLabel("Количество потоков");
        plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
                positions: threads,
                labels: threads.Select(x => x.ToString()).ToArray());
        plot.SavePng("threads_performance.png", width: 800, height: 600);
        System.Console.WriteLine($"Оптимальное количество потоков: {best.Key}");
    }

    static Dictionary<int, double> TimeOfSolveResearch(double step)
    {
        var threadsResults = new Dictionary<int, double>();
        double result = 0;
        var stopwatchForOneThread = Stopwatch.StartNew();
        for (int times = 0; times < 20; times++)
        {
            result += DefiniteIntegral.SolveWithoutThreads(a, b, SIN, step);
        }
        stopwatchForOneThread.Stop();
        threadsResults.Add(1, stopwatchForOneThread.Elapsed.TotalMilliseconds / 20);

        for (int threads = 2; threads <= 1024; threads *= 2)
        {
            result = 0;
            var stopwatchForManyThreads = Stopwatch.StartNew();
            for (int times = 0; times < 20; times++)
            {
                result += DefiniteIntegral.Solve(a, b, SIN, step, threads);
            }
            stopwatchForManyThreads.Stop();
            threadsResults.Add(threads, stopwatchForManyThreads.Elapsed.TotalMilliseconds / 20);
        }
        foreach (var res in threadsResults)
        {
            System.Console.WriteLine($"Количество потоков: {res.Key,-4} | время выполнения: {res.Value}");
        }
        return threadsResults;
    }

    static double FindBestStep()
    {
        double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
        var stepsResults = new Dictionary<double, double>();
        foreach (var step in steps)
        {
            stepsResults.Add(step, 0.0);
        }

        foreach (var step in steps)
        {
            var stopwatchForSteps = Stopwatch.StartNew();
            double result = DefiniteIntegral.Solve(a, b, SIN, step, 1);
            stopwatchForSteps.Stop();
            if (Math.Abs(result - 0.0) >= eps)
            {
                System.Console.WriteLine($"При шаге {step} ответ не является удовлетворительным!");
                stepsResults[step] = double.MaxValue;
            }
            else
            {
                stepsResults[step] = stopwatchForSteps.Elapsed.TotalMilliseconds;
            }

            Console.WriteLine($"Шаг: {step,-6} | Время: {stopwatchForSteps.Elapsed.TotalMilliseconds} мс");
        }
        var minStep = from step in stepsResults
                      orderby step.Value
                      select step.Key;
        System.Console.WriteLine($"\n Наилучший шаг - {minStep.First()}");
        return minStep.First();
    }
}