using System;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public struct OffensiveStats
    {
        public float damage;
        public float lifeTime;
        public float fireRate;
        public float cooldown;
        public float range;
        public float reloadTime;
        public float accuracy;
        public float recoil;
        public float clipSize;
        public float size;
        public float speed;
        public int penetration;


        public OffensiveStats Combine(OffensiveStats other)
        {
            return new OffensiveStats
            {
                damage = damage + other.damage,
                lifeTime = lifeTime + other.lifeTime,
                fireRate = fireRate + other.fireRate,
                cooldown = cooldown + other.cooldown,
                range = range + other.range,
                reloadTime = reloadTime + other.reloadTime,
                accuracy = accuracy + other.accuracy,
                recoil = recoil + other.recoil,
                clipSize = clipSize + other.clipSize,
                size = size + other.size,
                speed = speed + other.speed,
                penetration = penetration + other.penetration
            };
        }
    }
}