using System;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    [CreateAssetMenu(fileName = "weapon", menuName = "Scriptable/Weapon/New")]
    [Serializable]
    public class Weapon : AssetScript
    {
        [FormerlySerializedAs("bullet")] public BulletScript bulletScript;
        [SerializeField] private OffensiveStatsParameterScript offensiveStatsParameterScript;
        public OffensiveStatsParameterScript OffensiveStatsParameterScript => offensiveStatsParameterScript;

        public override bool OnTryAddToInventory(AssetScriptStorageComponent assetScriptStorageComponent, int amount,
            out int addedAmount)
        {
            assetScriptStorageComponent.GetComponent<IWeaponLoader>().Add(this);
            return base.OnTryAddToInventory(assetScriptStorageComponent, amount, out addedAmount);
        }

        public override bool OnTryRemovedFromInventory(AssetScriptStorageComponent assetScriptStorageComponent,
            int amount,
            out int removedAmount)
        {
            assetScriptStorageComponent.GetComponent<IWeaponLoader>().Remove(this);
            return base.OnTryRemovedFromInventory(assetScriptStorageComponent, amount, out removedAmount);
        }

        public virtual void PlaceWeapon(Transform parent, Transform weaponTransform)
        {
            weaponTransform.localPosition = Vector3.zero;
            weaponTransform.localRotation = Quaternion.identity;
        }
    }
}