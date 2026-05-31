using Task_4.Models;
using Task_4.Services;
using Task_4.ViewModels;
using Xunit;

namespace Task_4.Tests;

public class TasksViewModelTests
{
    private static TasksViewModel CreateViewModel(params TaskItem[] tasks)
    {
        var service = new InMemoryTaskService(tasks);

        return new TasksViewModel(service);
    }

    [Fact]
    public void AddTask_WithEmptyTitle_DoesNotAddTask()
    {
        var vm = CreateViewModel();

        vm.NewTaskTitle = string.Empty;

        vm.AddTaskCommand.Execute(null);

        Assert.Empty(vm.Tasks);
        Assert.True(vm.HasNewTaskTitleError);
        Assert.Equal("Введите название задачи", vm.NewTaskTitleError);
    }

    [Fact]
    public void AddTask_WithValidTitle_AddsTask()
    {
        var vm = CreateViewModel();

        vm.NewTaskTitle = "Learn MVVM";

        vm.AddTaskCommand.Execute(null);

        Assert.Single(vm.Tasks);
        Assert.Equal("Learn MVVM", vm.Tasks[0].Title);
        Assert.Equal(string.Empty, vm.NewTaskTitle);
        Assert.False(vm.HasNewTaskTitleError);
    }

    [Fact]
    public void DeleteTask_WithSelectedTask_RemovesTask()
    {
        var first = new TaskItem(
            "Task 1",
            "",
            false,
            TaskPriority.Normal,
            DateTimeOffset.Now.AddDays(1));

        var second = new TaskItem(
            "Task 2",
            "",
            false,
            TaskPriority.Normal,
            DateTimeOffset.Now.AddDays(1));

        var vm = CreateViewModel(first, second);

        var selectedTask = vm.Tasks[0];

        vm.SelectedTask = selectedTask;

        vm.DeleteTaskCommand.Execute(null);

        Assert.DoesNotContain(selectedTask, vm.Tasks);
        Assert.Single(vm.Tasks);
        Assert.Null(vm.SelectedTask);
    }

    [Fact]
    public void Filter_Active_ShowsOnlyActiveTasks()
    {
        var activeTask1 = new TaskItem(
            "Active 1",
            "",
            false,
            TaskPriority.Normal,
            DateTimeOffset.Now.AddDays(1));

        var doneTask = new TaskItem(
            "Done",
            "",
            true,
            TaskPriority.Normal,
            DateTimeOffset.Now.AddDays(1));

        var activeTask2 = new TaskItem(
            "Active 2",
            "",
            false,
            TaskPriority.Normal,
            DateTimeOffset.Now.AddDays(1));

        var vm = CreateViewModel(activeTask1, doneTask, activeTask2);

        vm.SelectedFilter = TaskFilter.Active;

        Assert.Equal(2, vm.FilteredTasks.Count);
        Assert.All(vm.FilteredTasks, task => Assert.False(task.IsDone));
    }
}