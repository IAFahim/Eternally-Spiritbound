using System.Threading;
using _Root.Scripts.Game.Consmatics.Runtime;
using _Root.Scripts.Model.Focus.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using _Root.Scripts.Presentation.Containers.Runtime;
using _Root.Scripts.Presentation.Interactions.Runtime;
using Cysharp.Threading.Tasks;
using Sisus.Init;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Presentation.FocusProcessors.Runtime
{
    [CreateAssetMenu(fileName = "Boat Focus Processor", menuName = "Scriptable/FocusProcessor/Boat")]
    public class PlayerFocusProcessorScriptScriptable : FocusProcessorScriptCinemachineScriptable
    {
        [SerializeField] private AssetReferenceGameObject healthBarAsset;
        [SerializeField] private AssetReferenceGameObject joyStickAsset;
        [SerializeField] private FlashConfigScript flashConfigScript;
        [SerializeField] private float timeScaleStopDuration = .2f;

        private HealthAndLevelUI _healthAndLevelUi;
        private GameObject _joyStickCache;

        private Material _targetOriginalMaterial;
        private EntityStatsComponent _entityStatsComponent;
        private DamageFlash _damageFlash;


        public override void SetFocus(FocusReferences focusReferences, CancellationToken token)
        {
            TargetGameObject = focusReferences.CurrentGameObject;
            _damageFlash = TargetGameObject.AddComponent<DamageFlash, FlashConfigScript>(flashConfigScript);
            BuildCache(
                focusReferences.ActiveElements, BeforeActive, token,
                (joyStickAsset, SetupJoystick, focusReferences.MovingUITransformPoint),
                (healthBarAsset, SetupHealthBar, focusReferences.UISillTransformPointPadded),
                (cinemachineAsset, SetupCinemachine, null)
            ).Forget();
        }

        private void BeforeActive()
        {
            
        }

        private void SetupJoystick(GameObject obj)
        {
            _joyStickCache = obj;
        }


        private void SetupHealthBar(GameObject spawnedHealthBar)
        {
            _entityStatsComponent = TargetGameObject.GetComponent<EntityStatsComponent>();
            _healthAndLevelUi = spawnedHealthBar.GetComponent<HealthAndLevelUI>();
            _healthAndLevelUi.Init(_entityStatsComponent);
        }

        private void RestoreTimeScale()
        {
            Time.timeScale = 1;
        }

        public override void OnFocusLost(GameObject targetGameObject)
        {
            Destroy(_damageFlash);
        }
    }
}