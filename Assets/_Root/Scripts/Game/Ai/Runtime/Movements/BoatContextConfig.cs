using System;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Movements
{
    [Serializable]
    public struct BoatContextConfig
    {
        public IntervalTicker ticker;
        public float avoidanceWeight;
        public float seekWeight;
        public int directions;
        public float dangerDecayDistance;
        public OverlapNonAlloc obstacleDetector;

        public void Initialize(Transform transform)
        {
            obstacleDetector.Initialize(transform);
        }
    }
}