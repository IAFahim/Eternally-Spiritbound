using System;
using _Root.Scripts.Model.Assets.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    [CreateAssetMenu(fileName = "weapon", menuName = "Scriptable/Weapon/New")]
    [Serializable]
    public class Weapon : AssetScript
    {
        public Bullet bullet;

        public override bool OnTryAddToInventory(AssetScriptStorageComponent assetScriptStorageComponent, int amount,
            out int addedAmount)
        {
            assetScriptStorageComponent.GetComponent<IWeaponLoader>().Add(this);
            return base.OnTryAddToInventory(assetScriptStorageComponent, amount, out addedAmount);
        }

        public override bool OnTryRemovedFromInventory(AssetScriptStorageComponent assetScriptStorageComponent, int amount,
            out int removedAmount)
        {
            assetScriptStorageComponent.GetComponent<IWeaponLoader>().Remove(this);
            return base.OnTryRemovedFromInventory(assetScriptStorageComponent, amount, out removedAmount);
        }
    }
}