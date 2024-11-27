using System.Globalization;
using _Root.Scripts.Game.Stats.Runtime;
using Pancake.Pools;
using Soul.Pools.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Weapons.Runtime.Damages
{
    [CreateAssetMenu(fileName = "DamagePopup", menuName = "Scriptable/Weapon/DamagePopup")]
    public class DamagePopup : ScriptableObject
    {
        public AssetReferenceGameObject damageTextAsset;

        public void ShowPopup(Vector3 hitPosition, DamageResult damageResult)
        {
            var text = SharedAssetReferencePoolInactive.Request<TMP_Text>(
                damageTextAsset, hitPosition, Quaternion.identity
            );
            
            text.text = damageResult.TotalDamageDealt.ToString(CultureInfo.InvariantCulture);
        }
    }
}