using _Root.Scripts.Game.Weapons.Runtime.Attacks;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime.Projectiles
{
    public interface IProjectile: IInitializable<AttackOrigin>
    {
        public GameObject GameObject { get; }
    }
}