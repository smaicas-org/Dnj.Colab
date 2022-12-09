using BenchmarkDotNet.Running;
using Dnj.Colab.Samples.StreamReaderEnumerable;

// https://learn.microsoft.com/es-es/dotnet/api/system.collections.generic.ienumerable-1?view=net-7.0
BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<StreamReaderBenchmark>();

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("Benchmark ended!. Press any key to close");
Console.ResetColor();
Console.ReadKey();