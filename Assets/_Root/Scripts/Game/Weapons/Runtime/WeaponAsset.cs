using System;
using _Root.Scripts.Game.Weapons.Runtime.Projectiles;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable/Weapon/New")]
    [Serializable]
    public class WeaponAsset : AssetScript
    {
        [SerializeField] private OffensiveStatsParameters offensiveStatsParameters;
        [SerializeField] private ProjectileAsset[] supportedProjectileAssets;

        public OffensiveStatsParameters OffensiveStatsParameters => offensiveStatsParameters;
        public ProjectileAsset[] SupportedProjectileAssets => supportedProjectileAssets;

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