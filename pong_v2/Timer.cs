namespace SpaceShooter
{
    public class MyTimer
    {
        private double _interval;
        private double _lastTick;

        public void Reset(double interval)
        {
            _interval = interval;
            _lastTick = Game1.GameTime.TotalGameTime.TotalMilliseconds;
        }

        public bool IsTimeUp()
        {
            double now = Game1.GameTime.TotalGameTime.TotalMilliseconds;
            if (now - _lastTick >= _interval)
            {
                _lastTick = now;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}