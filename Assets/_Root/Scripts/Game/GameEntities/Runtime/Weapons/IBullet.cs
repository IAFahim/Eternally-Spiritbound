using _Root.Scripts.Game.GameEntities.Runtime.Attacks;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public interface IBullet: IInitializable<AttackOrigin>
    {
        public GameObject GameObject { get; }
    }
}