using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Dnj.Colab.Samples.StreamReaderEnumerable;

[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public class StreamReaderBenchmark
{
    [Benchmark]
    public void StreamReaderEnumerable1K() => SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample1k.csv");

    [Benchmark]
    public void StreamReader1K() => SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample1k.csv");

    [Benchmark]
    public void YieldStreamReader1K() => SREnumerable.TestYieldReadingFile($"{Directory.GetCurrentDirectory()}/sample1k.csv");

    [Benchmark]
    public void StreamReaderEnumerable5K() => SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample5k.csv");

    [Benchmark]
    public void StreamReader5K() => SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample5k.csv");

    [Benchmark]
    public void YieldStreamReader5K() => SREnumerable.TestYieldReadingFile($"{Directory.GetCurrentDirectory()}/sample5k.csv");

    [Benchmark]
    public void StreamReaderEnumerable10K() => SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample10k.csv");

    [Benchmark]
    public void StreamReader10K() => SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample10k.csv");

    [Benchmark]
    public void YieldStreamReader10K() => SREnumerable.TestYieldReadingFile($"{Directory.GetCurrentDirectory()}/sample10k.csv");

    [Benchmark]
    public void StreamReaderEnumerable15K() => SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample15k.csv");

    [Benchmark]
    public void StreamReader15K() => SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample15k.csv");

    [Benchmark]
    public void YieldStreamReader15K() => SREnumerable.TestYieldReadingFile($"{Directory.GetCurrentDirectory()}/sample15k.csv");

    [Benchmark]
    public void StreamReaderEnumerable100K() => SREnumerable.TestStreamReaderEnumerable($"{Directory.GetCurrentDirectory()}/sample100k.csv");

    [Benchmark]
    public void StreamReader100K() => SREnumerable.TestReadingFile($"{Directory.GetCurrentDirectory()}/sample100k.csv");

    [Benchmark]
    public void YieldStreamReader100K() => SREnumerable.TestYieldReadingFile($"{Directory.GetCurrentDirectory()}/sample100k.csv");
}
