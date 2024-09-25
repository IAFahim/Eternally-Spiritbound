using UnityEngine;

namespace Soul.Stats.Runtime
{
    public interface IDamageBase<in T>
    {
        public bool TryDamage(T type, Vector3 position, float amount, out float damageTaken);
    }
}