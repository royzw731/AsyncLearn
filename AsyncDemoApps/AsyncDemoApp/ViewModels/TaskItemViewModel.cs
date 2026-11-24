using System.Windows.Input;

namespace ViewModels
{
    public class TaskItemViewModel : BaseViewModel
    {
        private double _progress;
        private string _statusText = string.Empty;
        private bool _isCancelEnabled = true;

        private readonly CancellationTokenSource _cts;

        public int TaskId { get; }

        public double Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusText
        {
            get => _statusText;
            set
            {
                if (_statusText != value)
                {
                    _statusText = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsCancelEnabled
        {
            get => _isCancelEnabled;
            set
            {
                if (_isCancelEnabled != value)
                {
                    _isCancelEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsRunning => IsCancelEnabled;

        public ICommand CancelCommand { get; }

        public TaskItemViewModel(int taskId)
        {
            TaskId = taskId;

            _cts = new CancellationTokenSource();

            StatusText = $"Task #{TaskId} - running...";

            CancelCommand = new Command(CancelTask);
        }

        private void CancelTask()
        {
            if (IsCancelEnabled)
            {
                _cts.Cancel();
                IsCancelEnabled = false;
            }
        }

        public async Task<bool> RunAsync()
        {
            try
            {
                Progress = 0;

                CancellationToken token = _cts.Token;

                for (int i = 1; i <= 100; i++)
                {
                    token.ThrowIfCancellationRequested();

                    // זמן המתנה אקראי בין 20 ל-100 מילישניות
                    int delay = Random.Shared.Next(20, 101);

                    await Task.Delay(delay, token);

                    Progress = i / 100.0;
                }

                StatusText = $"Task #{TaskId} - done!";

                return true;
            }
            catch (OperationCanceledException)
            {
                StatusText = $"Task #{TaskId} - cancelled";

                return false;
            }
            finally
            {
                IsCancelEnabled = false;
                _cts.Dispose();
                OnPropertyChanged(nameof(IsRunning));
            }
        }
    }
}