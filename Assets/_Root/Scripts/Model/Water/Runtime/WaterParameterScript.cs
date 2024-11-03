using _Root.Scripts.Model.ParameterScript.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Model.Water.Runtime
{
    [CreateAssetMenu(fileName = "WaterParameters", menuName = "Scriptable/Parameters/Water Parameters")]
    public class WaterParameterScript : ParameterScript<WaterParameters>
    {
        [SerializeField] private float lastTime = 0;
        [SerializeField] private float updateFrequency = 0.2f;
        [ShowInInspector] private float _waveOffset;

        private void OnEnable()
        {
            lastTime = Time.time;
        }

        public float WaveOffset
        {
            get
            {
                if (Time.time - lastTime > updateFrequency)
                {
                    Debug.Log("Update wave offset");
                    _waveOffset = Mathf.Sin(Time.time * value.waveFrequency) * value.waveIntensity;
                    lastTime = Time.time;
                }
                Debug.Log("Return wave offset");
                return _waveOffset;
            }
        }
    }
}