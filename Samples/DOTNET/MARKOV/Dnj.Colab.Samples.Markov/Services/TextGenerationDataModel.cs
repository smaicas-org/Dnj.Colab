using System.Text;
using Newtonsoft.Json;

namespace Dnj.Colab.Samples.Markov.Services;

public class TextGenerationDataModel : ITextGenerationDataModel
{
    private const string ModelPath = "./Model.json";
    public Dictionary<string, Trigram> Model { get; set; } = new();

    public TextGenerationDataModel()
    {
        if (File.Exists(ModelPath))
        {
            FileStream fs = File.OpenRead(ModelPath);
            StreamReader sr = new(fs);
            Model = JsonConvert.DeserializeObject<Dictionary<string, Trigram>>(sr.ReadToEnd());
            fs.Close();
        }
    }
    public async Task PersistAsync()
    {
        string serializedModel = JsonConvert.SerializeObject(Model);
        await using FileStream fs = File.Create(ModelPath);
        byte[] buffer = Encoding.UTF8.GetBytes(serializedModel);
        await fs.WriteAsync(buffer);
        fs.Close();
    }

    public async Task<int> CountAsync() => Model.Count;
}

public interface ITextGenerationDataModel
{
    Dictionary<string, Trigram> Model { get; set; }

    Task PersistAsync();
    Task<int> CountAsync();
}
public class Trigram
{

    public string[] PrefixWords;
    public List<string> Suffixes;

    public Trigram(string prefix1, string prefix2)
    {
        PrefixWords = new string[] { prefix1, prefix2 };
        this.Suffixes = new List<string>();
    }

    public void Add(string suffix) => Suffixes.Add(suffix);
}
