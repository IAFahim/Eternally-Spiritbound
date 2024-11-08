using System;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public struct OffensiveStats<T>
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
        public EnableLimitStat<T> penetration;


        public OffensiveStats(
            T damage, T lifeTime, T fireRate, T cooldown, T range, T reloadTime, T accuracy, T recoil,
            T size, T speed, T defensePenetration, T elementalDamage, EnableLimitStat<T> penetration
        )
        {
            this.damage = damage;
            this.lifeTime = lifeTime;
            this.fireRate = fireRate;
            this.cooldown = cooldown;
            this.range = range;
            this.reloadTime = reloadTime;
            this.accuracy = accuracy;
            this.recoil = recoil;
            this.size = size;
            this.speed = speed;
            this.defensePenetration = defensePenetration;
            this.elementalDamage = elementalDamage;
            this.penetration = penetration;
        }
    }
}