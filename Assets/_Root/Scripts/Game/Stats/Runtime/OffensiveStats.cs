using System;

namespace _Root.Scripts.Game.Stats.Runtime
{
    [Serializable]
    public class OffensiveStats<T>
    {
        public T damage;
        public T fireRate;
        public T cooldown;
        public T attackRange;
        public T reloadTime;
        public T accuracy;
        public T recoil;
        public T attackSize;
        public T attackSpeed;
        public T defensePenetration;
        public T elementalDamage;
    }
}