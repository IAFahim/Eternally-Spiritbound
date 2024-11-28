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

        public float fadeTime = .3f;
        public Vector3 startSize = new(1, 1, 1);
        public Vector3 endSize = new(.1f, .1f, .1f);


        private List<(TMP_Text tmp, float endTime)> texts;


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
                AnimateSizeOverTime(text.tmp, text.endTime);

                if (!(Time.time > text.endTime)) continue;
                SharedAssetReferencePoolInactive.Return(damageTextAsset, text.tmp.gameObject);
                texts.RemoveAt(i);
            }
        }

        private void AnimateSizeOverTime(TMP_Text tmp, float endTime)
        {
            var time = Time.time;
            var progress = (endTime - time) / fadeTime;
            var size = Vector3.Lerp(startSize, endSize, progress);
            tmp.transform.localScale = size;
        }

        public void ShowPopup(Vector3 hitPosition, DamageResult damageResult)
        {
            var positionTop = damageResult.EntityStatsComponent.entityStats.vitality.Top(
                damageResult.EntityStatsComponent.transform
            );
            var text = SharedAssetReferencePoolInactive.Request<TMP_Text>(
                damageTextAsset,
                positionTop,
                focusManagerScript.mainCamera.transform.rotation
            );

            text.transform.localScale = startSize;
            text.text = damageResult.TotalDamageDealt.ToString(CultureInfo.InvariantCulture);
            text.gameObject.SetActive(true);
            texts.Add((text, Time.time + fadeTime));
        }

        public void Disable()
        {
            texts.Clear();
            App.RemoveListener(EUpdateMode.Update, Update);
        }

        struct TweenSettings
        {
            public float duration;
        }
    }
}