namespace NepaliRiddle.States
{
    public class CountDownState : IDisposable
    {
        System.Timers.Timer? _timer;
        public int CurrentCountDown { get; private set; }
        public event Func<Task>? CountDownFinished;
        public event Action? OnValueChanged;

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Elapsed -= _timer_Elapsed;
                _timer.Dispose();
            }
        }

        public void StartCountDown(int timeOutInSec = 20)
        {
            CurrentCountDown = timeOutInSec;
            if (_timer != null)
            {
                _timer.Elapsed -= _timer_Elapsed;
                _timer.Dispose();

            }
            _timer = new System.Timers.Timer(1000)
            {
                AutoReset = true
            };
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }
        public void StopCountDown(bool setCountdownToZero = true)
        {
            if (_timer != null && _timer.Enabled)
            {
                _timer?.Stop();
            }
            if (setCountdownToZero)
            {
                CurrentCountDown = 0;
            }
        }
        public void StopCountDown(int chageCountdownTo)
        {
            StopCountDown(false);
            CurrentCountDown = chageCountdownTo;
        }
        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            CurrentCountDown -= 1;
            if (CurrentCountDown <= 0)
            {
                _timer!.Stop();
                CountDownFinished?.Invoke();
            }
            OnValueChanged?.Invoke();
        }


    }
}
