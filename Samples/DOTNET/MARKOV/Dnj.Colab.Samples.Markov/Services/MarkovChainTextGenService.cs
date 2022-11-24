using System.Text;
using Dnj.Colab.Samples.Markov.Exceptions;
using Dnj.Colab.Samples.Markov.Resources;

namespace Dnj.Colab.Samples.Markov.Services;

public class MarkovChainTextGenService : IMarkovChainTextGenService
{
    private readonly ITextGenerationDataModel _dataModel;
    private bool _trained = false;

    private static readonly Random Random = new();

    public MarkovChainTextGenService(ITextGenerationDataModel dataModel)
    {
        _dataModel = dataModel ?? throw new ArgumentNullException(nameof(dataModel));
        if (_dataModel.Model.Count > 0)
        {
            _trained = true;
        }
    }

    /// <exception cref="FormatException">sentences.Length < 2</exception>
    public async Task TrainAsync(string text)
    {
        this._trained = false;
        text = CleanText(text);
        string[] sentences = text.Split('.');
        if (sentences.Length < 2)
        {
            throw new FormatException(MarkovChainRes.The_text_provided_has_no_periods__Please_use_a_text_with_sentences_seperated_with_periods_);
        }
        for (int i = 0; i != sentences.Length; i++)
        {
            string[] words = sentences[i].Split(' ');
            if (words.Length < 3) { break; }
            for (int j = 0; j != words.Length - 2; j++)
            {
                string index = words[j] + words[j + 1];
                if (!_dataModel.Model.ContainsKey(index))
                {
                    _dataModel.Model[index] = new Trigram(words[j], words[j + 1]);
                }
                _dataModel.Model[index].Add(words[j + 2]);
            }
        }
        _trained = true;
        await _dataModel.PersistAsync();
    }

    /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public async Task TrainAsync(FileStream fs)
    {
        fs.Seek(0, SeekOrigin.Begin);
        StreamReader reader = new(fs, Encoding.UTF8);
        await TrainAsync(reader.ReadToEnd());
    }
    /// <exception cref="MarkovChainTextGenServiceException">not trained.</exception>
    /// <exception cref="OutOfMemoryException">The length of the resulting string overflows the maximum allowed length (<see cref="System.Int32.MaxValue">Int32.MaxValue</see>).</exception>
    public async Task<string> GenerateText(int length = 20)
    {
        if (!_trained)
        {
            throw new MarkovChainTextGenServiceException(MarkovChainRes.The_model_has_not_been_trained_yet_);
        }

        List<string> keyList = new(_dataModel.Model.Keys);
        List<string> sentence = new();
        string[] index = _dataModel.Model[keyList[Random.Next(keyList.Count)]].PrefixWords;
        sentence.Add(index[0]);
        sentence.Add(index[1]);
        for (int i = 1; i < length; i++)
        {
            index[0] = sentence[i - 1];
            index[1] = sentence[i];
            try
            {
                List<string> suffixes = _dataModel.Model[index[0] + index[1]].Suffixes;
                string choice = suffixes[Random.Next(suffixes.Count)];
                sentence.Add(choice);
            }
            catch (KeyNotFoundException)
            {
                index = _dataModel.Model[keyList[Random.Next(keyList.Count)]].PrefixWords;
                sentence.Add(index[0]);
                sentence.Add(index[1]);
            }
        }
        return string.Join(" ", sentence);
    }

    private string CleanText(string text)
    {
        string cleaned = text.Replace("...", "");
        cleaned = cleaned.Replace("\n", "");
        return cleaned;
    }
}

public interface IMarkovChainTextGenService
{
    Task TrainAsync(string text);
    Task TrainAsync(FileStream fs);
    Task<string> GenerateText(int length = 20);
}
