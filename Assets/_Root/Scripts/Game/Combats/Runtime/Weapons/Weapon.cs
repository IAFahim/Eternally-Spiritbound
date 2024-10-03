using System;
using _Root.Scripts.Game.Items.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    [CreateAssetMenu(fileName = "weapon", menuName = "Scriptable/Weapon/New")]
    [Serializable]
    public class Weapon : GameItem
    {
        public Bullet bullet;
        public override void Initialize(GameObject user)
        {
            base.Initialize(user);
            user.GetComponent<IWeaponLoader>().Add(this);
        }
    }
}