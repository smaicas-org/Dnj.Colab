using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dnj.Colab.Samples.MvvmSample.RCL.Model;

namespace Dnj.Colab.Samples.MvvmSample.RCL.ViewModels;

public class DnjTodoViewModel : IDnjTodoViewModel
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    private bool isBusy = false;
    public bool IsBusy
    {
        get => isBusy;
        set
        {
            isBusy = value;
            OnPropertyChanged();
        }
    }

    private List<TodoItem> _toDoItemList = new();
    public List<TodoItem> TodoItemList
    {
        get => _toDoItemList;
        private set
        {
            _toDoItemList = value;
            OnPropertyChanged();
        }

    }

    private TodoItem toDoItem = new();
    public TodoItem TodoItem
    {
        get => toDoItem;
        set
        {
            toDoItem = value;
            OnPropertyChanged();
        }
    }
    public int TodoItems => TodoItemList.Count(i => i.Done.Equals(false));

    public void SaveTodoItem(TodoItem todoitem)
    {
        IsBusy = true;
        if (todoitem.Id.Equals(Guid.Empty))
        {
            todoitem.Id = Guid.NewGuid();
        }
        else
        {
            _toDoItemList.Remove(todoitem);
        }

        _toDoItemList.Add(todoitem);

        OnPropertyChanged(nameof(TodoItemList));
        IsBusy = false;
    }
}
