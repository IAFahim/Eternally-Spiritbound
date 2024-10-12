using System;
using _Root.Scripts.Game.Stats.Runtime.Model;
using Soul.Modifiers.Runtime;
using Soul.Reactives.Runtime;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    [Serializable]
    public class WeaponOffensiveStats : OffensiveStats<float>
    {
        public WeaponOffensiveStats(float damage, float lifeTime, float fireRate, float cooldown, float range,
            float reloadTime, float accuracy, float recoil, float size, float speed, float defensePenetration,
            float elementalDamage, EnableLimitStat<float> penetration) : base(damage, lifeTime, fireRate, cooldown,
            range, reloadTime, accuracy, recoil, size, speed, defensePenetration, elementalDamage, penetration)
        {
        }

        public WeaponOffensiveStats Build(OffensiveStats<Modifier> playerStats)
        {
            return new WeaponOffensiveStats
            (
                damage += playerStats.damage.Value,
                lifeTime += playerStats.lifeTime.Value,
                fireRate = fireRate + playerStats.fireRate.Value,
                cooldown += playerStats.cooldown.Value,
                range += playerStats.range.Value,
                reloadTime += playerStats.reloadTime.Value,
                accuracy += playerStats.accuracy.Value,
                recoil += playerStats.recoil.Value,
                size += playerStats.size.Value,
                speed += playerStats.speed.Value,
                defensePenetration += playerStats.defensePenetration.Value,
                elementalDamage += playerStats.elementalDamage.Value,
                penetration = new EnableLimitStat<float>
                {
                    enabled = penetration.enabled && playerStats.penetration.enabled,
                    current = new Reactive<float>(
                        penetration.current.Value + playerStats.penetration.current.Value
                    ),
                    max = penetration.max + playerStats.penetration.max.Value
                }
            );
        }
    }
}