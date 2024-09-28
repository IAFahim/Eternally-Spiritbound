using Soul.Interfaces.Runtime;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Soul.Combats.Runtime
{
    public interface IWeaponBase<in TAttacker, out TStrategy> : IParameterLessInitialize where TStrategy : ScriptableObject
    {
        public TStrategy Strategy { get; }

        public bool TryAttack(TAttacker attacker, Vector3 position, Vector3 direction, LayerMask layerMask, float normalizedRange = 0);
    }
}