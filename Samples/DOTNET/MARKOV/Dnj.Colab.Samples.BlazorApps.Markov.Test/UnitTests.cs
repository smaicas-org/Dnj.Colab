using Dnj.Colab.Samples.BlazorApps.Markov.Exceptions;
using Dnj.Colab.Samples.BlazorApps.Markov.Services;

namespace Dnj.Colab.Samples.BlazorApps.Markov.Test;

public class UnitTests
{
    [Fact]
    public async Task Train_FailsIfNoPeriodsInText()
    {
        string TrainTextBad = @"Frase sin puntos";
        MarkovChainTextGenService mk = new();
        Assert.ThrowsAsync<FormatException>(async () => await mk.TrainAsync(TrainTextBad));
    }
    [Fact]
    public void Train_GeneratesModel()
    {
        string TrainTextBad = @"Uno dos tres cuatro. Cinco Seis Siete Ocho.";
        MarkovChainTextGenService mk = new();
        mk.TrainAsync(TrainTextBad);
        Assert.True(mk.Model["Unodos"] != null);
        Assert.True(mk.Model["Unodos"].PrefixWords[0] == "Uno");
        Assert.True(mk.Model["Unodos"].PrefixWords[1] == "dos");
        Assert.True(mk.Model["Unodos"].Suffixes[0] == "tres");
        Assert.True(mk.Model["dostres"] != null);
        Assert.True(mk.Model["dostres"].PrefixWords[0] == "dos");
        Assert.True(mk.Model["dostres"].PrefixWords[1] == "tres");
        Assert.True(mk.Model["dostres"].Suffixes[0] == "cuatro");
        // Model has 5 Trigrams
        Assert.True(mk.Model.Count == 5);
    }

    [Fact]
    public async Task GenerateText_FailsIfNotTrained()
    {
        string TrainTextBad = @"Uno dos tres cuatro. Cinco Seis Siete Ocho.";
        MarkovChainTextGenService mk = new();
        Assert.ThrowsAsync<MarkovChainTextGenServiceException>(async () => await mk.GenerateText(20));
    }
    [Fact]
    public async Task GenerateText_GeneratesNumberOfWords()
    {
        string TrainTextBad = @"Uno dos tres cuatro. Cinco Seis Siete Ocho.";
        MarkovChainTextGenService mk = new();
        await mk.TrainAsync(TrainTextBad);
        string tx = await mk.GenerateText(20);
        Assert.True(tx.Split(" ").Length > 20);
    }
}