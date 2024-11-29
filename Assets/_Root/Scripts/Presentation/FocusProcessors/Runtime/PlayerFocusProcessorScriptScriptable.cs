using System.Threading;
using _Root.Scripts.Game.Consmatics.Runtime;
using _Root.Scripts.Game.Interactables.Runtime.Focus;
using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Presentation.Interactions.Runtime;
using Cysharp.Threading.Tasks;
using Pancake.Common;
using Sisus.Init;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityProgressBar;

namespace _Root.Scripts.Presentation.FocusProcessors.Runtime
{
    [CreateAssetMenu(fileName = "Boat Focus Processor", menuName = "Scriptable/FocusProcessor/Boat")]
    public class PlayerFocusProcessorScriptScriptable : FocusProcessorScriptCinemachineScriptable
    {
        [SerializeField] private AssetReferenceGameObject healthBarAsset;
        [SerializeField] private AssetReferenceGameObject joyStickAsset;
        [SerializeField] private FlashConfigScript flashConfigScript;
        [SerializeField] private float timeScaleStopDuration = .2f;

        private ProgressBar _healthBarCache;
        private GameObject _joyStickCache;

        private Material _targetOriginalMaterial;
        private EntityStatsComponent _entityStatsComponent;
        private DamageFlash _damageFlash;


        public override void SetFocus(FocusReferences focusReferences, CancellationToken token)
        {
            TargetGameObject = focusReferences.CurrentGameObject;
            _damageFlash = TargetGameObject.AddComponent<DamageFlash, FlashConfigScript>(flashConfigScript);
            BuildCache(
                focusReferences.ActiveElements, null, token,
                (joyStickAsset, SetupJoystick, focusReferences.MovingUITransformPoint),
                (healthBarAsset, SetupHealthBar, focusReferences.UISillTransformPointPadded),
                (cinemachineAsset, SetupCinemachine, null)
            ).Forget();
        }

        private void SetupJoystick(GameObject obj)
        {
            _joyStickCache = obj;
        }


        private void SetupHealthBar(GameObject spawnedHealthBar)
        {
            _healthBarCache = spawnedHealthBar.GetComponent<ProgressBar>();
            _entityStatsComponent = TargetGameObject.GetComponent<EntityStatsComponent>();

            _entityStatsComponent.entityStats.vitality.health.current.OnChange += OnCurrentHealthChange;
            _healthBarCache.SetValueWithoutNotify(_entityStatsComponent.entityStats.vitality.health.current.Value);
        }

        private void CleanHealthBar()
        {
            _entityStatsComponent.entityStats.vitality.health.current.OnChange -= OnCurrentHealthChange;
        }


        private void OnCurrentHealthChange(float old, float current)
        {
            var difference = old - current;
            if (difference > 0)
            {
                Time.timeScale = 0f;
            }

            _healthBarCache.Value = current / _entityStatsComponent.entityStats.vitality.health.max;
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