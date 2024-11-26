using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime.Damages
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
                if (other.gameObject.TryGetComponent<EntityStatsComponent>(out var entityStatsComponent))
                {
                    entityStatsComponent.entityStats.Damage(1, out _);
                }
            }
        }
    }
}