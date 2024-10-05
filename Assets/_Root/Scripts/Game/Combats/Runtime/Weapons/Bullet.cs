using _Root.Scripts.Game.Items.Runtime;
using _Root.Scripts.Game.Stats.Runtime.Model;
using Soul.Modifiers.Runtime;
using Soul.Reactives.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    [CreateAssetMenu(fileName = "Bullet", menuName = "Scriptable/Weapon/Bullet")]
    public class Bullet : GameItem
    {
        [Header("Weapon Strategy")] public OffensiveStats<float> offensiveStats;

        [SerializeField] private float fireRate = 1;
        [SerializeField] private float minRange = 1;
        [SerializeField] private float maxRange = 10;

        public OffensiveStats<float> GetCalculateOffensiveStats(OffensiveStats<Modifier> playerStats)
        {
            return new OffensiveStats<float>
            {
                damage = offensiveStats.damage + playerStats.damage.Value,
                lifeTime = offensiveStats.lifeTime + playerStats.lifeTime.Value,
                fireRate = fireRate + playerStats.fireRate.Value,
                cooldown = offensiveStats.cooldown + playerStats.cooldown.Value,
                range = offensiveStats.range + playerStats.range.Value,
                reloadTime = offensiveStats.reloadTime + playerStats.reloadTime.Value,
                accuracy = offensiveStats.accuracy + playerStats.accuracy.Value,
                recoil = offensiveStats.recoil + playerStats.recoil.Value,
                size = offensiveStats.size + playerStats.size.Value,
                speed = offensiveStats.speed + playerStats.speed.Value,
                defensePenetration = offensiveStats.defensePenetration + playerStats.defensePenetration.Value,
                elementalDamage = offensiveStats.elementalDamage + playerStats.elementalDamage.Value,
                penetration = new LimitStat<float>
                {
                    current = new Reactive<float>(offensiveStats.penetration.current.Value +
                                                  playerStats.penetration.current.Value),
                    max = offensiveStats.penetration.max + playerStats.penetration.max.Value
                }
            };
        }

        public virtual float Damage => offensiveStats.damage;
        public virtual float FireRate => fireRate;
        public virtual float Range(float normalizedRange) => normalizedRange * (maxRange - minRange) + minRange;
        public static implicit operator OffensiveStats<float>(Bullet strategy) => strategy.offensiveStats;
    }
}