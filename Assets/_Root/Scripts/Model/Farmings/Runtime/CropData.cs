using System;
using _Root.Scripts.Model.Assets.Runtime;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Farmings.Runtime
{
    [Serializable]
    public class CropData
    {
        public SeedAsset seedAsset;
        public UnityDateTime plantTime;

        private float _duration;
        private float _startTime;
        private float _growEndTime;
        private UnityDateTime _endDateTime;

        public void Initialize(SeedAsset seed, DateTime plantTimeUTC, TimeSpan growthTime)
        {
            seedAsset = seed;
            plantTime = new UnityDateTime(plantTimeUTC);
            _duration = (float)growthTime.TotalSeconds;

            _startTime = Time.time + (float)plantTime.DateTime.Subtract(DateTime.UtcNow).TotalSeconds;
            _growEndTime = _startTime + _duration;
            _endDateTime = new UnityDateTime(plantTime.DateTime.Add(growthTime));
        }

        public bool IsOverGrowthTime() => Time.time >= _growEndTime;
        public float GetGrowthProgress() => Mathf.Clamp01((Time.time - _startTime) / _duration);
    }
}