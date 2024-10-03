using _Root.Scripts.Game.Combats.Runtime.Attacks;
using _Root.Scripts.Game.Items.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    [CreateAssetMenu(fileName = "Bullet", menuName = "Scriptable/Weapon/Bullet")]
    public class Bullet : GameItem
    {
        [Header("Weapon Strategy")] public AttackInfo attackInfo;
        
        [SerializeField] private float fireRate = 1;
        [SerializeField] private float minRange = 1;
        [SerializeField] private float maxRange = 10;

        public virtual float Damage => attackInfo.damage;
        public virtual float FireRate => fireRate;
        public virtual float Range(float normalizedRange) => normalizedRange * (maxRange - minRange) + minRange;
        public static implicit operator AttackInfo(Bullet strategy) => strategy.attackInfo;
        
    }
}