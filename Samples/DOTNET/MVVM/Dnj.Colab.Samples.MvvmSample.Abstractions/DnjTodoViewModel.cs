/* This file is copyright © 2022 Dnj.Colab repository authors.

Dnj.Colab content is distributed as free software: you can redistribute it and/or modify it under the terms of the General Public License version 3 as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

Dnj.Colab content is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the General Public License version 3 for more details.

You should have received a copy of the General Public License version 3 along with this repository. If not, see <https://github.com/smaicas-org/Dnj.Colab/blob/dev/LICENSE>. */

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dnj.Colab.Samples.MvvmSample.RCL.Model;
using Dnj.Colab.Samples.MvvmSample.RCL.ViewModels;

namespace Dnj.Colab.Samples.MvvmSample.ViewModels.Abstractions;

public abstract class DnjTodoViewModel : IDnjTodoViewModel
{
    private List<TodoItem> _toDoItemList = new();
    private bool isBusy;

    private TodoItem toDoItem = new();
    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsBusy
    {
        get => isBusy;
        set
        {
            isBusy = value;
            OnPropertyChanged();
        }
    }

    public virtual List<TodoItem> TodoItemList
    {
        get => _toDoItemList;
        set
        {
            _toDoItemList = value;
            OnPropertyChanged();
        }
    }

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

    public virtual async Task SaveTodoItem(TodoItem todoitem)
    {
        IsBusy = true;
        if (todoitem.Id.Equals(default))
            todoitem.Id = Guid.NewGuid();
        else
            _toDoItemList.Remove(todoitem);

        _toDoItemList.Add(todoitem);

        OnPropertyChanged(nameof(TodoItemList));
        IsBusy = false;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}