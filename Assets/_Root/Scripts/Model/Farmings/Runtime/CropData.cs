using System;
using _Root.Scripts.Model.Assets.Runtime;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Farmings.Runtime
{
    [Serializable]
    public class CropData
    {
        public AssetScript asset;
        public UnityDateTime plantTime;

        [NonSerialized] public Mesh[] Meshes;

        private float _duration;
        private float _startTime;
        private float _growEndTime;
        private UnityDateTime _endDateTime;

        public void Initialize(AssetScript crop, DateTime plantTimeUTC, TimeSpan growthTime, Mesh[] cropMeshes)
        {
            asset = crop;
            plantTime = new UnityDateTime(plantTimeUTC);
            _duration = (float)growthTime.TotalSeconds;
            Meshes = cropMeshes;

            _startTime = Time.time + (float)plantTime.DateTime.Subtract(DateTime.UtcNow).TotalSeconds;
            _growEndTime = _startTime + _duration;
            _endDateTime = new UnityDateTime(plantTime.DateTime.Add(growthTime));
        }

        public bool IsOverGrowthTime() => Time.time >= _growEndTime;
        public float GetGrowthProgress() => Mathf.Clamp01((Time.time - _startTime) / _duration);
    }
}