using System.Diagnostics;
using Dnj.Colab.Samples.StreamReaderEnumerable;

// https://learn.microsoft.com/es-es/dotnet/api/system.collections.generic.ienumerable-1?view=net-7.0
Stopwatch stopwatch = new();
Console.WriteLine("sample1k.csv");
stopwatch.Start();
SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample1k.csv");
stopwatch.Stop();
Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");

Console.WriteLine("---");
stopwatch.Start();
SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample1k.csv");
stopwatch.Stop();
Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");

Console.WriteLine(Environment.NewLine);

Console.WriteLine("sample5k.csv");
stopwatch.Start();
SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample5k.csv");
stopwatch.Stop();
Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");
Console.WriteLine("---");
stopwatch.Start();
SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample5k.csv");
stopwatch.Stop();
Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");

Console.WriteLine(Environment.NewLine);

Console.WriteLine("sample10k.csv");
stopwatch.Start();
SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample10k.csv");
stopwatch.Stop();
Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");
Console.WriteLine("---");
stopwatch.Start();
SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample10k.csv");
stopwatch.Stop();
Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");

Console.WriteLine(Environment.NewLine);

Console.WriteLine("sample15k.csv");
stopwatch.Start();
SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample15k.csv");
stopwatch.Stop();
Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");
Console.WriteLine("---");
stopwatch.Start();
SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample15k.csv");
stopwatch.Stop();
Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");

Console.WriteLine(Environment.NewLine);

Console.WriteLine("sample100k.csv");
stopwatch.Start();
SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample100k.csv");
stopwatch.Stop();
Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");
Console.WriteLine("---");
stopwatch.Start();
SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample100k.csv");
stopwatch.Stop();
Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");
