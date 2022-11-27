/* This file is copyright © 2022 Dnj.Colab repository authors.

Dnj.Colab content is distributed as free software: you can redistribute it and/or modify it under the terms of the General Public License version 3 as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

Dnj.Colab content is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the General Public License version 3 for more details.

You should have received a copy of the General Public License version 3 along with this repository. If not, see <https://github.com/smaicas-org/Dnj.Colab/blob/dev/LICENSE>. */

using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Dnj.Colab.Samples.SimpleCqrs.Features;
using Dnj.Colab.Samples.SimpleCqrs.Features.Responses;
using Dnj.Colab.Samples.SimpleCqrs.Mediator.Exceptions;
using Dnj.Colab.Samples.SimpleCqrs.RCL.Models;
using Dnj.Colab.Samples.SimpleCqrs.RCL.ViewModels;
using FluentValidation.Results;
using MediatR;

namespace Dnj.Colab.Samples.SimpleCqrs.ViewModel;

public class GamesComponentVm : IGamesComponentVm
{
    private readonly Dictionary<string, IEnumerable<ValidationFailure>> _errors = new();
    private readonly IMediator _mediator;

    public GamesComponentVm(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public List<GameDto> Games { get; set; } = new();
    public GameDto CurrentGame { get; set; } = new();

    public async Task CreateGame()
    {
        CreateOrUpdateGameCommand command = new()
        {
            Game = CurrentGame
        };
        try
        {
            GameDto res = await _mediator.Send(command);
        }
        catch (DnjPipelineValidationException ex)
        {
            await AddErrors(ex.ValidationFailures);
            OnErrorsChanged();
        }

        CurrentGame = new GameDto();
        OnPropertyChanged();
    }

    public async Task GetAllGames()
    {
        GetAllGamesCommand command = new();
        Games = await _mediator.Send(command);
        OnPropertyChanged(nameof(Games));
    }

    public async Task DeleteGame(GameDto dto)
    {
        DeleteGameCommand command = new()
        {
            Game = dto
        };
        GenericStateResponse res = await _mediator.Send(command);
        OnPropertyChanged(nameof(Games));
    }

    public async Task EditGame(GameDto dto)
    {
        CurrentGame = dto;
        OnPropertyChanged(nameof(CurrentGame));
    }

    public string GetErrorsDisplay(string propertyName)
    {
        StringBuilder res = new();
        foreach (ValidationFailure error in GetErrors(propertyName))
        {
            res.Append(error.ErrorMessage);
            res.Append(Environment.NewLine);
        }

        return res.ToString();
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        return propertyName != null && _errors.ContainsKey(propertyName!)
            ? _errors[propertyName]!
            : new List<ValidationFailure>();
    }

    public bool HasErrors => _errors.Count > 0;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// INotifyDataErrorInfo Implementation
    protected async Task AddErrors(IEnumerable<ValidationFailure> failures)
    {
        foreach (ValidationFailure validationFailure in failures)
        {
            string[] propNameArr = validationFailure.PropertyName.Split(".");
            if (_errors.ContainsKey(propNameArr[^1]))
                await Task.Run(() => _errors[propNameArr[^1]].ToList().Add(validationFailure));
            else
                await Task.Run(() =>
                    _errors[propNameArr[^1]] = new List<ValidationFailure>
                    {
                        validationFailure
                    });
        }
    }

    protected virtual void OnErrorsChanged([CallerMemberName] string? propertyName = null)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}