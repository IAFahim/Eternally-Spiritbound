using Soul.Interfaces.Runtime;
using UnityEngine;

namespace Soul.Combats.Runtime
{
    public interface
        IWeaponBase<in TAttackOrigin, in TAttackInfo, out TStrategy> : IParameterLessInitialize
        where TStrategy : ScriptableObject
    {
        public TStrategy Strategy { get; }
        public void Attack(TAttackOrigin attackOrigin, TAttackInfo attackInfo);
    }
}