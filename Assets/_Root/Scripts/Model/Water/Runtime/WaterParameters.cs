using System;
using UnityEngine;

namespace _Root.Scripts.Model.Water.Runtime
{
    [Serializable]
    public struct WaterParameters
    {
        [Header("Buoyancy Settings")] public float waterLevel;
        public float waterDrag;
        public float buoyancyForce;
        public float waveIntensity;
        public float waveFrequency;
    }
}