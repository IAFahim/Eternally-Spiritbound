using UnityEngine;

namespace Soul2.Maths.Runtime
{
    public class MathHelper
    {
        public static float Lerp(float value, float target, float rate, float deltaTime)
        {
            if (deltaTime == 0f) { return value; }
            return Mathf.Lerp(target, value, LerpRate(rate, deltaTime));
        }
        
     private static float LerpRate(float rate, float deltaTime)
        {
            rate = Mathf.Clamp01(rate);
            float invRate = - Mathf.Log(1.0f - rate, 2.0f) * 60f;
            return Mathf.Pow(2.0f, -invRate * deltaTime);
        }
    }
}