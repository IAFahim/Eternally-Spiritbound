using UnityEngine;

namespace Soul.Tickers.Runtime
{
    [System.Serializable]
    public class IntervalTicker
    {
        [SerializeField] private int currentTick;
        [SerializeField] private int interval = 5;

        public IntervalTicker()
        {
            currentTick = 0;
        }

        public IntervalTicker(int interval)
        {
            this.interval = Mathf.Max(1, interval);
            currentTick = 0;
        }


        public bool TryTick()
        {
            currentTick++;

            if (currentTick >= interval)
            {
                currentTick = 0;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            currentTick = 0;
        }

        public float Progress => (float)currentTick / interval;
    }
}