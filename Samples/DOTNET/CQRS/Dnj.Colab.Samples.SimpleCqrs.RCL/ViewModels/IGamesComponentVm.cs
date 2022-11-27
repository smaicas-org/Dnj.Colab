using System.ComponentModel;
using Dnj.Colab.Samples.SimpleCqrs.RCL.Models;

namespace Dnj.Colab.Samples.SimpleCqrs.RCL.ViewModels;
public interface IGamesComponentVm : INotifyPropertyChanged, INotifyDataErrorInfo
{
    List<GameDto> Games { get; set; }
    GameDto CurrentGame { get; set; }
    Task CreateGame();
    Task GetAllGames();
    Task DeleteGame(GameDto dto);
    Task EditGame(GameDto dto);
    string GetErrorsDisplay(string propertyName);
}
