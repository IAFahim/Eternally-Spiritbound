using _Root.Scripts.Game.Consmatics.Runtime;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.Interactions.Runtime;
using _Root.Scripts.Game.Stats.Runtime.Controller;
using Pancake.Common;
using Sisus.Init;
using Soul.Modifiers.Runtime;
using Soul.Reactives.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityProgressBar;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    [CreateAssetMenu(fileName = "Boat Focus Processor", menuName = "Scriptable/FocusProcessor/Boat")]
    public class BoatFocusProcessorScriptScriptable : FocusProcessorScriptCinemachineScriptable
    {
        [SerializeField] private AssetReferenceGameObject healthBarAsset;
        [SerializeField] private AssetReferenceGameObject joyStickAsset;
        [SerializeField] private FlashConfigScript flashConfigScript;
        [SerializeField] private float timeScaleStopDuration = .2f;

        private ProgressBar _healthBarCache;
        private GameObject _joyStickCache;

        private Material _targetOriginalMaterial;
        private EntityStats _entityStats;
        private Reactive<float> _health;
        private Modifier _maxHealth;
        private DamageFlash _damageFlash;


        public override void SetFocus(FocusReferences focusReferences)
        {
            TargetGameObject = focusReferences.currentGameObject;
            _damageFlash = TargetGameObject.AddComponent<DamageFlash, FlashConfigScript>(flashConfigScript);
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

            _healthBarCache.Value = current / _maxHealth.Value;
            App.Delay(timeScaleStopDuration, RestoreTimeScale, useRealTime: true);
        }

        private void RestoreTimeScale()
        {
            Time.timeScale = 1;
        }

        public override void OnFocusLost(GameObject targetGameObject)
        {
            CleanHealthBar();
            Destroy(_damageFlash);
        }
    }
}