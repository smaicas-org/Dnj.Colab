using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Dnj.Colab.Samples.SortingAlgorithms;

[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public class SortingBenchmark
{
    readonly int[] _orderedArray = new int[10000];
    readonly int[] _unorderedArray = new int[10000];
    public SortingBenchmark()
    {
        for (int i = 0; i < 99; i++)
        {
            _orderedArray[i] = i;
        }
        for (int i = 99; i > -1; i--)
        {
            _unorderedArray[i] = i;
        }
    }

    private void OriginalSort(Array array) => Array.Sort(array);
    private void BubbleSort<T>(IList<T> array) where T : IComparable<T>
    {
        int i, j;
        T temp;

        for (i = array.Count - 1; i > 0; i--)
        {
            for (j = 0; j < i; j++)
            {
                if (array[j].CompareTo(array[j + 1]) > 0)
                {
                    temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        }
    }

    [Benchmark]
    public void OriginalSortBestCase() => OriginalSort(_orderedArray);

    [Benchmark]
    public void OriginalSortWorstCase() => OriginalSort(_unorderedArray);

    [Benchmark]
    public void BubbleSortBestCase() => BubbleSort(_orderedArray);

    [Benchmark]
    public void BubbleSortWorstCase() => BubbleSort(_unorderedArray);
}
