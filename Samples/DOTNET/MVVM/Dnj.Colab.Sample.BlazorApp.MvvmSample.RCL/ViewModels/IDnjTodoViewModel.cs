using System.ComponentModel;
using Dnj.Colab.Samples.BlazorApps.MvvmSample.RCL.Model;

namespace Dnj.Colab.Samples.BlazorApps.MvvmSample.RCL.ViewModels;

public interface IDnjTodoViewModel : INotifyPropertyChanged
{
    bool IsBusy { get; set; }
    int TodoItems { get; }
    TodoItem TodoItem { get; set; }
    List<TodoItem> TodoItemList { get; }

    void SaveTodoItem(TodoItem todoitem);
}
