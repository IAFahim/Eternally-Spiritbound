using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime.Projectiles
{
    public class GoForwardProjectile: ProjectileBaseComponent
    {
        private void OnTriggerEnter(Collider other)
        {
            DamageInformPentrationCounterUp(other.transform.root);
        }

        protected override void OnUpdate(float timePassed)
        {
            transform.position += transform.forward * (AttackOrigin.offensiveStats.speed * Time.deltaTime);
        }
    }
}