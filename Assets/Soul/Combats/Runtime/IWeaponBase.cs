using Soul.Interfaces.Runtime;
using UnityEngine;

namespace Soul.Combats.Runtime
{
    public interface
        IWeaponBase<in TAttackInfo, out TAttack, out TStrategy> : IParameterLessInitialize
        where TStrategy : ScriptableObject
    {
        public TStrategy Strategy { get; }


        public void Attack(
            GameObject attacker, Vector3 position, Vector3 direction, LayerMask layerMask,
            TAttackInfo attackInfo, float normalizedRange
        );
    }
}