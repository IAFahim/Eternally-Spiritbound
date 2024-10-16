using System;
using System.Collections.Generic;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using _Root.Scripts.Game.Stats.Runtime.Controller;
using Pancake.Common;
using Soul.Modifiers.Runtime;
using Soul.Reactives.Runtime;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityProgressBar;

namespace _Root.Scripts.Presentation.UIProviders.Runtime
{
    [CreateAssetMenu(fileName = "BoatNavigationUIProviderScriptable",
        menuName = "Scriptable/FocusProviders/BoatFocusProviderScriptable")]
    public class BoatFocusProviderScriptable : FocusProviderScriptable
    {
        public AssetReferenceGameObject healthBarAsset;
        public AssetReferenceGameObject cinemachineAsset;

        public float timeScaleStopDuration = .2f;
        public Material targetFlashMaterial;

        private ProgressBar _healthBarCache;
        private CinemachineCamera _cinemachineCache;
        private Material _targetOriginalMaterial;
        private Renderer _targetRenderer;
        private GameObject _targetGameObject;
        private EntityStats _entityStats;
        private Reactive<float> _health;
        private Modifier _maxHealth;


        public override void SetFocus(Dictionary<AssetReferenceGameObject, GameObject> activeElements,
            Transform uISpawnPointTransform, GameObject targetGameObject, Action returnFocusCallBack)
        {
            _targetGameObject = targetGameObject;
            SetCache(
                activeElements, uISpawnPointTransform,
                (healthBarAsset, SetupHealthBar, true),
                (cinemachineAsset, SetupCinemachine, false)
            );
        }


        private void SetupCinemachine(GameObject spawnedCinemachine)
        {
            _cinemachineCache = spawnedCinemachine.GetComponent<CinemachineCamera>();
            _cinemachineCache.Follow = _targetGameObject.transform;
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

        private void RestoreTimeScale()
        {
            Time.timeScale = 1;
            _targetRenderer.material = _targetOriginalMaterial;
        }

        public override void OnFocusLost(GameObject targetGameObject)
        {
            CleanHealthBar();
        }
    }
}