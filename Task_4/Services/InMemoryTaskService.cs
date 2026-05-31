using System;
using System.Collections.Generic;
using Task_4.Models;

namespace Task_4.Services;

public class InMemoryTaskService : ITaskService
{
    private readonly List<TaskItem> _tasks;

    public InMemoryTaskService(IEnumerable<TaskItem>? tasks = null)
    {
        _tasks = tasks is null
            ? CreateDefaultTasks()
            : new List<TaskItem>(tasks);
    }

    public IEnumerable<TaskItem> GetTasks()
    {
        return _tasks;
    }

    public TaskItem CreateTask(string title)
    {
        var task = new TaskItem(
            title,
            string.Empty,
            false,
            TaskPriority.Normal,
            DateTimeOffset.Now.AddDays(1));

        _tasks.Add(task);

        return task;
    }

    public void DeleteTask(TaskItem task)
    {
        _tasks.Remove(task);
    }

    public void MarkAsDone(TaskItem task)
    {
        task.IsDone = true;
    }

    private static List<TaskItem> CreateDefaultTasks()
    {
        return new List<TaskItem>
        {
            new TaskItem(
                "Выучить MVVM",
                "Разобраться с Model, ItemViewModel и Service",
                false,
                TaskPriority.High,
                DateTimeOffset.Now.AddDays(1)),

            new TaskItem(
                "Сделать практику",
                "Добавить валидацию и тесты",
                false,
                TaskPriority.Normal,
                DateTimeOffset.Now.AddDays(-1)),

            new TaskItem(
                "Повторить Binding",
                "SelectedItem, DataTemplate, ObservableCollection",
                true,
                TaskPriority.Low,
                DateTimeOffset.Now)
        };
    }
}