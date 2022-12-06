using BenchmarkDotNet.Running;
using Dnj.Colab.Samples.SortingAlgorithms;

BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<SortingBenchmark>();

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("Benchmark ended!. Press any key to close");
Console.ResetColor();
Console.ReadKey();

