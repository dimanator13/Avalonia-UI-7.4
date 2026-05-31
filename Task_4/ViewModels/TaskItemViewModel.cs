using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Task_4.Models;

namespace Task_4.ViewModels;

public partial class TaskItemViewModel : ObservableObject
{
    private readonly TaskItem _model;

    public TaskItem Model => _model;

    public TaskItemViewModel(TaskItem model)
    {
        _model = model;
    }

    public string Title
    {
        get => _model.Title;
        set
        {
            if (_model.Title == value)
                return;

            _model.Title = value;
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => _model.Description;
        set
        {
            if (_model.Description == value)
                return;

            _model.Description = value;
            OnPropertyChanged();
        }
    }

    public bool IsDone
    {
        get => _model.IsDone;
        set
        {
            if (_model.IsDone == value)
                return;

            _model.IsDone = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(IsOverdue));
        }
    }

    public TaskPriority Priority
    {
        get => _model.Priority;
        set
        {
            if (_model.Priority == value)
                return;

            _model.Priority = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(IsHighPriority));
        }
    }

    public DateTimeOffset DueDate
    {
        get => _model.DueDate;
        set
        {
            if (_model.DueDate == value)
                return;

            _model.DueDate = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(IsOverdue));
        }
    }

    public DateTimeOffset CreatedAt => _model.CreatedAt;

    public bool IsOverdue =>
        DueDate.Date < DateTimeOffset.Now.Date && !IsDone;

    public bool IsHighPriority =>
        Priority == TaskPriority.High;

    public void RefreshState()
    {
        OnPropertyChanged(nameof(IsDone));
        OnPropertyChanged(nameof(IsOverdue));
        OnPropertyChanged(nameof(IsHighPriority));
    }
}