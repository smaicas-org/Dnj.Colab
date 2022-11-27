/* This file is copyright © 2022 Dnj.Colab repository authors.

Dnj.Colab content is distributed as free software: you can redistribute it and/or modify it under the terms of the General Public License version 3 as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

Dnj.Colab content is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the General Public License version 3 for more details.

You should have received a copy of the General Public License version 3 along with this repository. If not, see <https://github.com/smaicas-org/Dnj.Colab/blob/dev/LICENSE>. */

using System.Text;
using Newtonsoft.Json;

namespace Dnj.Colab.Samples.Markov.Services;

public class TextGenerationDataModel : ITextGenerationDataModel
{
    private const string ModelPath = "./Model.json";

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

    public Dictionary<string, Trigram> Model { get; set; } = new();

    public async Task PersistAsync()
    {
        string serializedModel = JsonConvert.SerializeObject(Model);
        await using FileStream fs = File.Create(ModelPath);
        byte[] buffer = Encoding.UTF8.GetBytes(serializedModel);
        await fs.WriteAsync(buffer);
        fs.Close();
    }

    public async Task<int> CountAsync()
    {
        return Model.Count;
    }
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
        PrefixWords = new[] { prefix1, prefix2 };
        Suffixes = new List<string>();
    }

    public void Add(string suffix)
    {
        Suffixes.Add(suffix);
    }
}