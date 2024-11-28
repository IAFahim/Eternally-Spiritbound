using System.Globalization;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.Stats.Runtime;
using Soul.Pools.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.DamagePopups.Runtime
{
    [CreateAssetMenu(fileName = "DamagePopup", menuName = "Scriptable/Weapon/DamagePopup")]
    public class DamagePopup : ScriptableObject
    {
        public AssetReferenceGameObject damageTextAsset;
        public FocusManagerScript focusManagerScript;
        
        public void ShowPopup(Vector3 hitPosition, DamageResult damageResult)
        {
            var text = SharedAssetReferencePoolInactive.Request<TMP_Text>(
                damageTextAsset, hitPosition, focusManagerScript.mainCamera.transform.rotation
            );
            
            text.text = damageResult.TotalDamageDealt.ToString(CultureInfo.InvariantCulture);
            text.gameObject.SetActive(true);
        }
    }
}