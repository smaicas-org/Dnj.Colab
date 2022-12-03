using Dnj.Colab.Samples.StreamReaderEnumerable;

// https://learn.microsoft.com/es-es/dotnet/api/system.collections.generic.ienumerable-1?view=net-7.0

Console.WriteLine("sample1k.csv");

SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample1k.csv");
Console.WriteLine("---");
SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample1k.csv");

Console.WriteLine(Environment.NewLine);

Console.WriteLine("sample5k.csv");
SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample5k.csv");
Console.WriteLine("---");
SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample5k.csv");

Console.WriteLine(Environment.NewLine);

Console.WriteLine("sample10k.csv");
SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample10k.csv");
Console.WriteLine("---");
SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample10k.csv");

Console.WriteLine(Environment.NewLine);

Console.WriteLine("sample15k.csv");
SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample15k.csv");
Console.WriteLine("---");
SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample15k.csv");

Console.WriteLine(Environment.NewLine);

Console.WriteLine("sample100k.csv");
SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample100k.csv");
Console.WriteLine("---");
SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample100k.csv");

Console.WriteLine(Environment.NewLine);