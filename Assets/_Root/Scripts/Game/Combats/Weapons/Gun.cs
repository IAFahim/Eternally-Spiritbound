using Soul.Combats.Runtime;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace _Root.Scripts.Game.Combats.Weapons
{
    public class Gun : MonoBehaviour , IWeaponBase<GameObject, WeaponStrategyBase>
    {
        public WeaponStrategyBase strategy;
        public WeaponStrategyBase Strategy => strategy;
        
        public void Initialize()
        {
            
        }

        public bool TryAttack(GameObject attacker, Vector3 position, Vector3 direction, LayerMask layerMask,
            float normalizedRange = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}