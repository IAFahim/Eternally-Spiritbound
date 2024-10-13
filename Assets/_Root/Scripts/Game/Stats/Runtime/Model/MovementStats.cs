using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public class MovementStats<T>: ICloneable
    {
        public T speed;
        public T pickupRange;
        public T detectRange;

        public object Clone()
        {
            return new MovementStats<T>
            {
                speed = speed,
                pickupRange = pickupRange,
                detectRange = detectRange
            };
        }
    }
}