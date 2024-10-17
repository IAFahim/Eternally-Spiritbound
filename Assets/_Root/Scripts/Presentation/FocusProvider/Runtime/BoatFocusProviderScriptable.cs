using System;
using System.Collections.Generic;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using _Root.Scripts.Game.Stats.Runtime.Controller;
using Pancake.Common;
using Soul.Modifiers.Runtime;
using Soul.Reactives.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityProgressBar;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    [CreateAssetMenu(fileName = "BoatFocusProvider", menuName = "Scriptable/FocusProviders/Boat")]
    public class BoatFocusProviderScriptable : FocusProviderCinemachineScriptable
    {
        public AssetReferenceGameObject healthBarAsset;

        public float timeScaleStopDuration = .2f;
        public Material targetFlashMaterial;

        private ProgressBar _healthBarCache;
        private Material _targetOriginalMaterial;
        private Renderer _targetRenderer;
        private EntityStats _entityStats;
        private Reactive<float> _health;
        private Modifier _maxHealth;


        public override void SetFocus(Dictionary<AssetReferenceGameObject, GameObject> activeElements,
            TransformReferences transformReferences, GameObject targetGameObject, Action returnFocusCallBack)
        {
            TargetGameObject = targetGameObject;
            BuildCache(
                activeElements,
                (healthBarAsset, SetupHealthBar, transformReferences.stillCanvasTransformPoint),
                (cinemachineAsset, SetupCinemachine, null)
            );
        }


        private void SetupHealthBar(GameObject spawnedHealthBar)
        {
            _targetRenderer = TargetGameObject.GetComponentInChildren<Renderer>();
            _targetOriginalMaterial = _targetRenderer.material;
            _healthBarCache = spawnedHealthBar.GetComponent<ProgressBar>();
            _entityStats = TargetGameObject.GetComponent<IEntityStatsReference>().EntityStats;
            _health = _entityStats.vitality.health.current;
            _maxHealth = _entityStats.vitality.health.max;
            _health.OnChange += OnCurrentHealthChange;
            _healthBarCache.SetValueWithoutNotify(_health.Value);
            _healthBarCache.gameObject.SetActive(true);
        }

        private void CleanHealthBar()
        {
            _health.OnChange -= OnCurrentHealthChange;
        }


        private void OnCurrentHealthChange(float old, float current)
        {
            var difference = old - current;
            if (difference > 0)
            {
                Time.timeScale = 0f;
            }

            _targetRenderer.material = targetFlashMaterial;
            _healthBarCache.Value = current / _maxHealth.Value;
            App.Delay(timeScaleStopDuration, RestoreTimeScale, useRealTime: true);
        }

        private void RestoreTimeScale()
        {
            Time.timeScale = 1;
            _targetRenderer.material = _targetOriginalMaterial;
        }

        public override void OnFocusLost(GameObject targetGameObject)
        {
            base.OnFocusLost(targetGameObject);
            CleanHealthBar();
        }
    }
}