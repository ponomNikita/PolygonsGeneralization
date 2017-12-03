using System;
using System.Windows.Forms;

namespace PolygonGeneralization.WinForms.Controls
{
    public class ScrollTimer
    {
        private const int SCROLL_INTERVAL = 500;

        private readonly Timer _timer;
        private int _value;

        public ScrollTimer()
        {
            _timer = new Timer();
            _timer.Interval = SCROLL_INTERVAL;
            _timer.Tick += _timer_Tick;
        }

        public int Result { get; private set; }

        public event EventHandler ScrollEvent;

        public void Reset(int delta)
        {
            _value += delta;
            _timer.Start();
        }

        private void _timer_Tick(object sender, System.EventArgs e)
        {
            _timer.Stop();
            Result = _value;
            _value = 0;
            OnScrollEvent();
        }

        protected virtual void OnScrollEvent()
        {
            ScrollEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}