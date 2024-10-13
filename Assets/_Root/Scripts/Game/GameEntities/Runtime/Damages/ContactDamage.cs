using _Root.Scripts.Game.Stats.Runtime.Controller;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Damages
{
    public class ContactDamage : MonoBehaviour
    {
        public LayerMask targetLayer;
        private IEntityStatsReference _entityStatsReference;

        private void Awake()
        {
            _entityStatsReference = GetComponent<IEntityStatsReference>();
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