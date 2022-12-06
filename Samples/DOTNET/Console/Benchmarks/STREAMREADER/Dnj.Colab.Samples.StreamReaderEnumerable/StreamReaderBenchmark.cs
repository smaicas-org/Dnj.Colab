using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Dnj.Colab.Samples.StreamReaderEnumerable;

[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public class StreamReaderBenchmark
{
    [Benchmark]
    public void StreamReaderEnumerable1k() => SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample1k.csv");
    [Benchmark]
    public void StreamReader1k() => SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample1k.csv");
    [Benchmark]
    public void StreamReaderEnumerable5k() => SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample5k.csv");
    [Benchmark]
    public void StreamReader5k() => SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample5k.csv"); [Benchmark]
    public void StreamReaderEnumerable10k() => SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample10k.csv");
    [Benchmark]
    public void StreamReader10k() => SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample10k.csv"); [Benchmark]
    public void StreamReaderEnumerable15k() => SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample15k.csv");
    [Benchmark]
    public void StreamReader15k() => SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample15k.csv"); [Benchmark]
    public void StreamReaderEnumerable100k() => SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample100k.csv");
    [Benchmark]
    public void StreamReader100k() => SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample100k.csv");
}
