using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public class OffensiveStats<T>
    {
        public T damage;
        public T lifeTime;
        public T fireRate;
        public T cooldown;
        public T range;
        public T reloadTime;
        public T accuracy;
        public T recoil;
        public T size;
        public T speed;
        public T defensePenetration;
        public T elementalDamage;
        public LimitStat<T> penetration;
    }
}