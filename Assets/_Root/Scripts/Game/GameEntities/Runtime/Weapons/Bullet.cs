using _Root.Scripts.Game.Combats.Runtime.Weapons;
using _Root.Scripts.Game.Items.Runtime;
using _Root.Scripts.Game.Stats.Runtime.Model;
using Soul.Modifiers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    [CreateAssetMenu(fileName = "Bullet", menuName = "Scriptable/Weapon/Bullet")]
    public class Bullet : GameItem
    {
        [Header("Weapon Strategy")] [SerializeField]
        private WeaponOffensiveStats offensiveStats;

        [SerializeField] private float minRange = 1;
        [SerializeField] private float maxRange = 10;

        public WeaponOffensiveStats GetWeaponOffensiveStats(OffensiveStats<Modifier> playerStats)
        {
            return offensiveStats.Build(playerStats);
        }

        public virtual float Damage => offensiveStats.damage;
        public virtual float Range(float normalizedRange) => normalizedRange * (maxRange - minRange) + minRange;
        public static implicit operator WeaponOffensiveStats(Bullet strategy) => strategy.offensiveStats;
    }
}