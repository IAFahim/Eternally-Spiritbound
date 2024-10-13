using System;
using _Root.Scripts.Game.Items.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    [CreateAssetMenu(fileName = "weapon", menuName = "Scriptable/Weapon/New")]
    [Serializable]
    public class Weapon : GameItem
    {
        public Bullet bullet;
        public override void OnAddedToInventory(GameObject user, int amount)
        {
            base.OnAddedToInventory(user, amount);
            user.GetComponent<IWeaponLoader>().Add(this);
        }
    }
}