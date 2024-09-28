using _Root.Scripts.Game.Combats.Attacks;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Weapons
{
    public class Bullet : MonoBehaviour, IWeaponSubComponent
    {
        [SerializeField] private Rigidbody rb;
        private Attack attackReference;
        
        public void Attack(Attack attack)
        {
            attackReference = attack;
            rb.linearVelocity = attack.Origin.direction * attack.Info.speed;
        }

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }
    }
}