using Soul.Interfaces.Runtime;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Soul.Combats.Runtime
{
    public interface IWeapon<in TAttacker, out TStrategy> : IParameterLessInitialize where TStrategy : ScriptableObject, IParameterLessInitialize
    {
        public TStrategy Strategy { get; }
        public bool TryAttack(TAttacker attacker, Vector3 position, Vector3 direction, LayerMask layerMask);
    }
}