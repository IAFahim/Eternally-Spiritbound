using System;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    [CreateAssetMenu(fileName = "weapon", menuName = "Scriptable/Weapon/New")]
    [Serializable]
    public class WeaponAsset : AssetScript
    {
        [FormerlySerializedAs("bulletScript")] [FormerlySerializedAs("bullet")]
        public BulletAsset bulletAsset;

        [SerializeField] private OffensiveStatsParameterScript offensiveStatsParameterScript;
        public OffensiveStatsParameterScript OffensiveStatsParameterScript => offensiveStatsParameterScript;

        public override bool OnTryAddToInventory(
            AssetScriptStorageComponent assetScriptStorageComponent,
            int amount,
            out int addedAmount,
            out int afterAddAmount)
        {
            assetScriptStorageComponent.GetComponent<IWeaponLoader>().Add(this);
            return base.OnTryAddToInventory(assetScriptStorageComponent, amount, out addedAmount, out afterAddAmount);
        }

        public override bool OnTryRemovedFromInventory(AssetScriptStorageComponent assetScriptStorageComponent,
            int amount,
            out int removedAmount,
            out int afterRemoveAmount
        )
        {
            assetScriptStorageComponent.GetComponent<IWeaponLoader>().Remove(this);
            return base.OnTryRemovedFromInventory(
                assetScriptStorageComponent,
                amount,
                out removedAmount,
                out afterRemoveAmount
            );
        }

        public virtual void PlaceWeapon(Transform parent, Transform weaponTransform)
        {
            weaponTransform.localPosition = Vector3.zero;
            weaponTransform.localRotation = Quaternion.identity;
        }
    }
}