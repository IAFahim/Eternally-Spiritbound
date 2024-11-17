using System;
using _Root.Scripts.Model.Stats.Runtime;
using Soul.Modifiers.Runtime;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    [Serializable]
    public class WeaponOffensiveStats
    {
        public OffensiveStats<float> value;

        public OffensiveStats<float> Add(OffensiveStats<Modifier> playerStats)
        {
            return new OffensiveStats<float>
            (
                value.damage + playerStats.damage.Value,
                value.lifeTime + playerStats.lifeTime.Value,
                value.fireRate + playerStats.fireRate.Value,
                value.cooldown + playerStats.cooldown.Value,
                value.range + playerStats.range.Value,
                value.reloadTime + playerStats.reloadTime.Value,
                value.accuracy + playerStats.accuracy.Value,
                value.recoil + playerStats.recoil.Value,
                value.size + playerStats.size.Value,
                value.speed + playerStats.speed.Value,
                value.defensePenetration + playerStats.defensePenetration.Value,
                value.elementalDamage + playerStats.elementalDamage.Value,
                value.penetration + playerStats.penetration
            );
        }
    }
}