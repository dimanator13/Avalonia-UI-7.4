using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Task_4.Models;
using Task_4.Services;

namespace Task_4.ViewModels;

public partial class TasksViewModel : ViewModelBase
{
    private readonly ITaskService _taskService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasSelectedTask))]
    [NotifyCanExecuteChangedFor(nameof(DeleteTaskCommand))]
    [NotifyCanExecuteChangedFor(nameof(MarkAsDoneCommand))]
    private TaskItemViewModel? _selectedTask;

    [ObservableProperty]
    private string _newTaskTitle = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasNewTaskTitleError))]
    private string _newTaskTitleError = string.Empty;

    [ObservableProperty]
    private TaskFilter _selectedFilter = TaskFilter.All;

    public bool HasSelectedTask => SelectedTask is not null;

    public bool HasNewTaskTitleError =>
        !string.IsNullOrWhiteSpace(NewTaskTitleError);

    public ObservableCollection<TaskItemViewModel> Tasks { get; } = new();

    public ObservableCollection<TaskItemViewModel> FilteredTasks { get; } = new();

    public ObservableCollection<TaskFilter> AvailableFilters { get; } = new()
    {
        TaskFilter.All,
        TaskFilter.Active,
        TaskFilter.Done,
        TaskFilter.Overdue
    };

    public ObservableCollection<TaskPriority> AvailablePriorities { get; } = new()
    {
        TaskPriority.Low,
        TaskPriority.Normal,
        TaskPriority.High
    };

    public TasksViewModel(ITaskService taskService)
    {
        _taskService = taskService;

        foreach (var task in _taskService.GetTasks())
        {
            Tasks.Add(new TaskItemViewModel(task));
        }

        RefreshFilteredTasks();
    }

    [RelayCommand]
    private void AddTask()
    {
        if (!ValidateNewTaskTitle())
            return;

        var task = _taskService.CreateTask(NewTaskTitle.Trim());

        var taskViewModel = new TaskItemViewModel(task);

        Tasks.Add(taskViewModel);

        RefreshFilteredTasks();

        SelectedTask = taskViewModel;
        NewTaskTitle = string.Empty;
        NewTaskTitleError = string.Empty;
    }

    [RelayCommand(CanExecute = nameof(HasSelectedTask))]
    private void DeleteTask()
    {
        if (SelectedTask is null)
            return;

        _taskService.DeleteTask(SelectedTask.Model);

        Tasks.Remove(SelectedTask);
        SelectedTask = null;

        RefreshFilteredTasks();
    }

    [RelayCommand(CanExecute = nameof(HasSelectedTask))]
    private void MarkAsDone()
    {
        if (SelectedTask is null)
            return;

        _taskService.MarkAsDone(SelectedTask.Model);

        SelectedTask.RefreshState();

        RefreshFilteredTasks();
    }

    private bool ValidateNewTaskTitle()
    {
        if (string.IsNullOrWhiteSpace(NewTaskTitle))
        {
            NewTaskTitleError = "Введите название задачи";
            return false;
        }

        if (NewTaskTitle.Trim().Length < 3)
        {
            NewTaskTitleError = "Название должно быть минимум 3 символа";
            return false;
        }

        NewTaskTitleError = string.Empty;
        return true;
    }

    partial void OnNewTaskTitleChanged(string value)
    {
        if (HasNewTaskTitleError && value.Trim().Length >= 3)
        {
            NewTaskTitleError = string.Empty;
        }
    }

    partial void OnSelectedFilterChanged(TaskFilter value)
    {
        RefreshFilteredTasks();
    }

    private void RefreshFilteredTasks()
    {
        FilteredTasks.Clear();

        foreach (var task in Tasks)
        {
            if (SelectedFilter == TaskFilter.All)
            {
                FilteredTasks.Add(task);
            }
            else if (SelectedFilter == TaskFilter.Active && !task.IsDone)
            {
                FilteredTasks.Add(task);
            }
            else if (SelectedFilter == TaskFilter.Done && task.IsDone)
            {
                FilteredTasks.Add(task);
            }
            else if (SelectedFilter == TaskFilter.Overdue && task.IsOverdue)
            {
                FilteredTasks.Add(task);
            }
        }
    }
}