using System;
using System.Threading;

namespace Golf_With_Friends_Cheats
{
    public class ActiveTimer
    {
        private Timer _timer;
        private int _dueTime;
        private int _period;
        private bool _isActive;
        private Action _action;

        public ActiveTimer(Action action, int dueTime)
        {
            _dueTime = dueTime;
            _action = action;
        }

        public void Start()
        {
            _isActive = true;
            _period = 0;
            _timer = new Timer(OnTimerActive, null, _dueTime, _period);
        }

        public void Stop()
        {
            _isActive = false;
            _dueTime = Timeout.Infinite;
            _period = Timeout.Infinite;
        }

        private void OnTimerActive(object state)
        {
            _timer = new Timer(OnTimerActive, null, _dueTime, _period);
            if (_isActive)
            {
                _action.Invoke();
            }
        }
    }
}
