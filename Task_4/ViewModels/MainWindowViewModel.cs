using Task_4.Services;

namespace Task_4.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public TasksViewModel TasksViewModel { get; }

    public MainWindowViewModel()
    {
        ITaskService taskService = new InMemoryTaskService();

        TasksViewModel = new TasksViewModel(taskService);
    }
}