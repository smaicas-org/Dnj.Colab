/* This file is copyright © 2022 Dnj.Colab repository authors.

Dnj.Colab content is distributed as free software: you can redistribute it and/or modify it under the terms of the General Public License version 3 as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

Dnj.Colab content is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the General Public License version 3 for more details.

You should have received a copy of the General Public License version 3 along with this repository. If not, see <https://github.com/smaicas-org/Dnj.Colab/blob/dev/LICENSE>. */

using Dnj.Colab.Samples.Markov.Exceptions;
using Dnj.Colab.Samples.Markov.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Dnj.Colab.Samples.Markov.Test;

public class Test_MarkovChainTextGetService : IClassFixture<DnjTestingWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private readonly DnjTestingWebApplicationFactory<Program>
        _factory;

    public Test_MarkovChainTextGetService(DnjTestingWebApplicationFactory<Program> factory)
    {
        File.Delete("./Model.json");
        _factory = factory;
        WebApplicationFactoryClientOptions clientOptions = new()
        {
            AllowAutoRedirect = true,
            BaseAddress = new Uri("http://localhost"),
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        };

        _client = _factory.CreateClient(clientOptions);
    }

    [Fact]
    public async Task Train_FailsIfNoPeriodsInText()
    {
        AsyncServiceScope svcScope = _factory.Services.CreateAsyncScope();
        IMarkovChainTextGenService svc =
            (IMarkovChainTextGenService)svcScope.ServiceProvider.GetRequiredService(typeof(IMarkovChainTextGenService));

        const string trainTextBad = @"Frase sin puntos";
        await Assert.ThrowsAsync<FormatException>(async () => await svc.TrainAsync(trainTextBad)).ConfigureAwait(false);
    }

    [Fact]
    public async Task Train_GeneratesModel()
    {
        AsyncServiceScope svcScope = _factory.Services.CreateAsyncScope();
        IMarkovChainTextGenService svc =
            (IMarkovChainTextGenService)svcScope.ServiceProvider.GetRequiredService(typeof(IMarkovChainTextGenService));

        const string trainTextBad = @"Uno dos tres cuatro. Cinco Seis Siete Ocho.";

        await svc.TrainAsync(trainTextBad);

        ITextGenerationDataModel modelSvc =
            (ITextGenerationDataModel)svcScope.ServiceProvider.GetRequiredService(typeof(ITextGenerationDataModel));
        int ndata = await modelSvc.CountAsync();
        Assert.True(ndata > 0);
    }

    [Fact]
    public async Task Train_GeneratesModel_ExpandModel()
    {
        AsyncServiceScope svcScope = _factory.Services.CreateAsyncScope();
        IMarkovChainTextGenService svc =
            (IMarkovChainTextGenService)svcScope.ServiceProvider.GetRequiredService(typeof(IMarkovChainTextGenService));

        string trainText = @"Uno dos tres cuatro. Cinco Seis Siete Ocho.";

        await svc.TrainAsync(trainText);

        ITextGenerationDataModel modelSvc =
            (ITextGenerationDataModel)svcScope.ServiceProvider.GetRequiredService(typeof(ITextGenerationDataModel));
        int currentCount = await modelSvc.CountAsync();
        Assert.True(currentCount > 0);

        trainText = @"Nueve diez once doce. Trece catorce quince diecis�is";
        await svc.TrainAsync(trainText);

        int newCount = await modelSvc.CountAsync();
        Assert.True(currentCount < newCount);
    }

    [Fact]
    public async Task GenerateText_FailsIfNotTrained()
    {
        AsyncServiceScope svcScope = _factory.Services.CreateAsyncScope();
        IMarkovChainTextGenService svc =
            (IMarkovChainTextGenService)svcScope.ServiceProvider.GetRequiredService(typeof(IMarkovChainTextGenService));

        Assert.ThrowsAsync<MarkovChainTextGenServiceException>(async () => await svc.GenerateText());
    }

    [Fact]
    public async Task GenerateText_GeneratesNumberOfWords()
    {
        AsyncServiceScope svcScope = _factory.Services.CreateAsyncScope();
        IMarkovChainTextGenService svc =
            (IMarkovChainTextGenService)svcScope.ServiceProvider.GetRequiredService(typeof(IMarkovChainTextGenService));

        string TrainTextBad = @"Uno dos tres cuatro. Cinco Seis Siete Ocho.";

        await svc.TrainAsync(TrainTextBad);
        string tx = await svc.GenerateText();
        Assert.True(tx.Split(" ").Length > 20);
    }
}