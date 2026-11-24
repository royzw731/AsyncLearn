using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        private int _taskCounter;
        private int _completedTasksCount;

        public ObservableCollection<TaskItemViewModel> Tasks { get; }
            = new ObservableCollection<TaskItemViewModel>();

        public int CompletedTasksCount
        {
            get => _completedTasksCount;
            set
            {
                if (_completedTasksCount != value)
                {
                    _completedTasksCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand StartTaskCommand { get; }
        public ICommand ClearAllCommand { get; }
        public ICommand CancelAllCommand { get; }

        public TasksViewModel()
        {
            Title = "ניהול משימות";

            StartTaskCommand = new Command(async () =>
                await StartNewTaskAsync());

            ClearAllCommand = new Command(async () =>
                await ClearAllTasksAsync());

            CancelAllCommand = new Command(CancelAllTasks);
        }

        private async Task StartNewTaskAsync()
        {
            // הגבלת מספר המשימות הפעילות ל-4
            int activeTasks = Tasks.Count(t => t.IsRunning);

            if (activeTasks >= 4)
            {
                await ShowAlertAsync(
                    "הגבלת משימות",
                    "לא ניתן להפעיל יותר מ-4 משימות במקביל.",
                    "אישור");

                return;
            }

            _taskCounter++;

            // יצירת משימה חדשה
            TaskItemViewModel newTask =
                new TaskItemViewModel(_taskCounter);

            // הוספה לרשימה הדינאמית
            Tasks.Add(newTask);

            // הפעלת המשימה האסינכרונית
            bool completed = await newTask.RunAsync();

            // עדכון מונה המשימות שהסתיימו בהצלחה
            if (completed)
            {
                CompletedTasksCount++;
            }
        }

        private void CancelAllTasks()
        {
            foreach (var task in Tasks)
            {
                if (task.IsRunning)
                {
                    task.CancelCommand.Execute(null);
                }
            }
        }

        private async Task ClearAllTasksAsync()
        {
            bool answer = await ShowConfirmAsync(
                "ניקוי רשימה",
                "האם אתה בטוח שברצונך לנקות את כל המשימות?",
                "כן",
                "לא");

            if (answer)
            {
                // ביטול משימות שעדיין פעילות
                CancelAllTasks();

                // ניקוי הרשימה
                Tasks.Clear();
            }
        }
    }
}