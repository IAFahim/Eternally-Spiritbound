using System.Collections.Generic;
using _Root.Scripts.Game.Stats.Runtime.Controller;
using _Root.Scripts.Game.UiLoaders.Runtime;
using Pancake.Common;
using Soul.Modifiers.Runtime;
using Soul.Reactives.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityProgressBar;

namespace _Root.Scripts.Presentation.UIProviders.Runtime
{
    [CreateAssetMenu(fileName = "BoatNavigationUIProviderScriptable",
        menuName = "Scriptable/UIProviders/BoatNavigationUIProviderScriptable")]
    public class BoatNavigationUIProviderScriptable : UIProviderScriptable
    {
        public AssetReferenceGameObject healthBarAsset;
        public float timeScaleStopDuration = .2f;
        public Material targetFlashMaterial;

        private Material _targetOriginalMaterial;
        private Renderer _targetRenderer;
        private ProgressBar _healthBarCache;
        private GameObject _targetGameObject;
        private EntityStats _entityStats;
        private Reactive<float> _health;
        private Modifier _maxHealth;

        public override void EnableUI(Dictionary<AssetReferenceGameObject, GameObject> activeUiElements,
            Transform uISpawnPointTransform,
            GameObject targetGameObject)
        {
            _targetGameObject = targetGameObject;
            SetCache(
                activeUiElements, uISpawnPointTransform,
                (healthBarAsset, SetupHealthBar)
            );
        }

        private void SetupHealthBar(GameObject spawnedHealthBar)
        {
            _targetRenderer = _targetGameObject.GetComponentInChildren<Renderer>();
            _targetOriginalMaterial = _targetRenderer.material;
            _healthBarCache = spawnedHealthBar.GetComponent<ProgressBar>();
            _entityStats = _targetGameObject.GetComponent<IEntityStatsReference>().EntityStats;
            _health = _entityStats.vitality.health.current;
            _maxHealth = _entityStats.vitality.health.max;
            _health.OnChange += OnCurrentHealthChange;
            _healthBarCache.SetValueWithoutNotify(_health.Value);
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

        public void RestoreTimeScale()
        {
            Time.timeScale = 1;
            _targetRenderer.material = _targetOriginalMaterial;
        }

        public override void DisableUI(GameObject targetGameObject)
        {
            CleanHealthBar();
        }
    }
}