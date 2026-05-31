using System.Collections.Generic;
using Task_4.Models;

namespace Task_4.Services;

public interface ITaskService
{
    IEnumerable<TaskItem> GetTasks();

    TaskItem CreateTask(string title);

    void DeleteTask(TaskItem task);

    void MarkAsDone(TaskItem task);
}