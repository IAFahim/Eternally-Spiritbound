using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime.Projectiles
{
    public class HomingProjectile : ProjectileBaseComponent
    {
        protected override void OnUpdate(float timePassed)
        {
            if (AttackOrigin.target.Value != null) transform.LookAt(AttackOrigin.target.Value.transform);
            transform.position += transform.forward * (AttackOrigin.offensiveStats.speed * Time.deltaTime);
        }
    }
}