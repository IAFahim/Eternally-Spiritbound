using System;
using System.Collections.Generic;
using _Root.Scripts.Game.FocusProvider.Runtime;
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
    public class BoatFocusConsumerScriptable : FocusConsumerCinemachineScriptable
    {
        public AssetReferenceGameObject healthBarAsset;
        public AssetReferenceGameObject joyStickAsset;

        public float timeScaleStopDuration = .2f;
        public Material targetFlashMaterial;

        private ProgressBar _healthBarCache;
        private GameObject _joyStickCache;

        private Material _targetOriginalMaterial;
        private Renderer _targetRenderer;
        private EntityStats _entityStats;
        private Reactive<float> _health;
        private Modifier _maxHealth;


        public override void SetFocus(FocusReferences focusReferences)
        {
            TargetGameObject = focusReferences.currentGameObject;
            BuildCache(
                focusReferences.ActiveElements,
                (joyStickAsset, SetupJoystick, focusReferences.movingCanvasTransformPoint),
                (healthBarAsset, SetupHealthBar, focusReferences.stillCanvasTransformPoint),
                (cinemachineAsset, SetupCinemachine, null)
            );
        }

        private void SetupJoystick(GameObject obj)
        {
            _joyStickCache = obj;
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