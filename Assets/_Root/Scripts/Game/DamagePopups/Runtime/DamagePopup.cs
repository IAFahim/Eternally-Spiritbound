using System.Collections.Generic;
using System.Globalization;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.Stats.Runtime;
using Pancake.Common;
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

        private List<(TMP_Text tmp, float endTime)> texts;
        private const float FadeTime = .5f;

        public void Enable()
        {
            texts = new();
            App.AddListener(EUpdateMode.Update, Update);
        }

        private void Update()
        {
            if (texts.Count == 0) return;
            var cameraRot = focusManagerScript.mainCamera.transform.rotation;
            for (var i = texts.Count - 1; i >= 0; i--)
            {
                var text = texts[i];
                text.tmp.transform.rotation = cameraRot;

                if (!(Time.time > text.endTime)) continue;
                SharedAssetReferencePoolInactive.Return(damageTextAsset, text.tmp.gameObject);
                texts.RemoveAt(i);
            }
        }

        public void ShowPopup(Vector3 hitPosition, DamageResult damageResult)
        {
            var text = SharedAssetReferencePoolInactive.Request<TMP_Text>(
                damageTextAsset, hitPosition,
                focusManagerScript.mainCamera.transform.rotation,
                damageResult.VitimRootTransform
            );

            text.text = damageResult.TotalDamageDealt.ToString(CultureInfo.InvariantCulture);
            text.gameObject.SetActive(true);
            texts.Add((text, Time.time + FadeTime));
        }

        public void Disable()
        {
            texts.Clear();
            App.RemoveListener(EUpdateMode.Update, Update);
        }
    }
}