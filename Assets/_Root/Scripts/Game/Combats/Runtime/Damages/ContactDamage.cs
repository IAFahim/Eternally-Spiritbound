using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Damages
{
    public class ContactDamage : MonoBehaviour
    {
        public LayerMask targetLayer;
        private void OnTriggerEnter(Collider other)
        {
            if (targetLayer == (targetLayer | (1 << other.gameObject.layer)))
            {
                
            }
        }
    }
}