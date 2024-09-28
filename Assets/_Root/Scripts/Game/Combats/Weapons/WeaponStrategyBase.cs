using _Root.Scripts.Game.Combats.Attacks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Combats.Weapons
{
    [CreateAssetMenu(fileName = "WeaponStrategyBase", menuName = "Scriptable/Weapon/WeaponStrategyBase")]
    public class WeaponStrategyBase : ScriptableObject
    {
        [Header("Weapon Strategy")] public AttackInfo attackInfo;

        [SerializeField] private float fireRate = 1;
        [SerializeField] private float minRange = 1;
        [SerializeField] private float maxRange = 10;
        public AssetReferenceGameObject assetReferenceGameObject;

        public Attack.Builder GetAttackBuilderFromStrategy(float normalizedRange)
        {
            return new Attack.Builder()
                .SetDamage(Damage)
                .SetSpeed(attackInfo.speed)
                .SetSize(attackInfo.size)
                .SetLifeTime(attackInfo.lifeTime)
                .SetRange(Range(normalizedRange))
                .SetDamageType(attackInfo.damageTypes);
        }

        public virtual float Damage => attackInfo.damage;
        public virtual float FireRate => fireRate;
        public virtual float Range(float normalizedRange) => normalizedRange * (maxRange - minRange) + minRange;
        
        public static implicit operator AttackInfo(WeaponStrategyBase strategy) => strategy.attackInfo;
        
    }
}