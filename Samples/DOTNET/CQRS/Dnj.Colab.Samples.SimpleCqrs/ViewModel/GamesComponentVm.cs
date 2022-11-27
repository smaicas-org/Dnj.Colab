using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Dnj.Colab.Samples.SimpleCqrs.Features;
using Dnj.Colab.Samples.SimpleCqrs.Mediator.Exceptions;
using Dnj.Colab.Samples.SimpleCqrs.RCL.Models;
using Dnj.Colab.Samples.SimpleCqrs.RCL.ViewModels;
using FluentValidation.Results;
using MediatR;

namespace Dnj.Colab.Samples.SimpleCqrs.ViewModel;

public class GamesComponentVm : IGamesComponentVm
{
    private readonly IMediator _mediator;

    public GamesComponentVm(IMediator mediator) => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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
        GetAllGamesCommand command = new() { };
        Games = await _mediator.Send(command);
        OnPropertyChanged(nameof(Games));
    }

    public async Task DeleteGame(GameDto dto)
    {
        DeleteGameCommand command = new()
        {
            Game = dto
        };
        Features.Responses.GenericStateResponse res = await _mediator.Send(command);
        OnPropertyChanged(nameof(Games));
    }

    public async Task EditGame(GameDto dto)
    {
        CurrentGame = dto;
        OnPropertyChanged(nameof(CurrentGame));
    }

    /// INotifyDataErrorInfo Implementation
    public async Task AddErrors(IEnumerable<ValidationFailure> failures)
    {
        foreach (ValidationFailure validationFailure in failures)
        {
            var propNameArr = validationFailure.PropertyName.Split(".");
            if (_errors.ContainsKey(propNameArr[^1]))
            {
                await Task.Run(() => _errors[propNameArr[^1]].ToList().Add(validationFailure));
            }
            else
            {
                await Task.Run(() =>
                    _errors[propNameArr[^1]] = new List<ValidationFailure>()
                    {
                        validationFailure
                    });
            }
        }
    }

    private readonly Dictionary<string, IEnumerable<ValidationFailure>> _errors = new();

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
    public IEnumerable GetErrors(string? propertyName) =>
        (propertyName != null && _errors.ContainsKey(propertyName!))
            ? _errors[propertyName]!
            : new List<ValidationFailure>();

    public bool HasErrors => _errors.Count > 0;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    protected virtual void OnErrorsChanged([CallerMemberName] string? propertyName = null) => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

}
