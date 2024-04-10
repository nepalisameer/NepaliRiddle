namespace NepaliRiddle.States
{
    public class CountDownState : IDisposable
    {
        System.Timers.Timer? _timer;
        public int CurrentCountDown { get; private set; }
        private int _countDown;
        public string CurrentCountDownText => $"{TimeSpan.FromSeconds(CurrentCountDown):mm\\:ss}";
        public event Action? CountDownFinished;
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
            _countDown = timeOutInSec;
            CurrentCountDown = timeOutInSec;
            _timer?.Dispose();
            _timer = new System.Timers.Timer(1000)
            {
                AutoReset = true
            };
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }
        public void ResetCountDown()
        {
            _timer?.Stop();
            CurrentCountDown = _countDown;
            _timer?.Start();
            OnValueChanged?.Invoke();

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
