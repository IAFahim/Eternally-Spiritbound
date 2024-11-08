using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Damages
{
    [RequireComponent(typeof(EntityStatsComponent))]
    public class ContactDamage : MonoBehaviour
    {
        public LayerMask targetLayer;
        private EntityStatsComponent _entityStatsReference;

        private void Awake()
        {
            _entityStatsReference = GetComponent<EntityStatsComponent>();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (C.Contains(other.gameObject.layer, targetLayer))
            {
                if (other.gameObject.TryGetComponent<IDamage>(out var damage))
                {
                    damage.TryKill(1, out _);
                }
            }
        }
    }
}