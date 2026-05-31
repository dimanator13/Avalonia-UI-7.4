using System;

namespace Task_4.Models;

public class TaskItem
{
    public string Title { get; set; }

    public string Description { get; set; }

    public bool IsDone { get; set; }

    public TaskPriority Priority { get; set; }

    public DateTimeOffset DueDate { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public TaskItem(
        string title,
        string description,
        bool isDone,
        TaskPriority priority,
        DateTimeOffset dueDate)
    {
        Title = title;
        Description = description;
        IsDone = isDone;
        Priority = priority;
        DueDate = dueDate;
        CreatedAt = DateTimeOffset.Now;
    }
}