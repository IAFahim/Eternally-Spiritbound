using System.Collections.Generic;
using _Root.Scripts.Game.Stats.Runtime.Controller;
using _Root.Scripts.Game.UiLoaders.Runtime;
using Soul.Modifiers.Runtime;
using Soul.Reactives.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using UnityProgressBar;

namespace _Root.Scripts.Presentation.UIProviders.Runtime
{
    [CreateAssetMenu(fileName = "BoatNavigationUIProviderScriptable",
        menuName = "Scriptable/UIProviders/BoatNavigationUIProviderScriptable")]
    public class BoatNavigationUIProviderScriptable : UIProviderScriptable
    {
        [FormerlySerializedAs("healthBarAssetReferenceGameObject")] [FormerlySerializedAs("healthBarPrefab")]
        public AssetReferenceGameObject healthBarAsset;

        private ProgressBar _healthBarCache;
        private EntityStats _entityStats;
        private Reactive<float> _health;
        private Modifier _maxHealth;

        public override void EnableUI(Dictionary<AssetReferenceGameObject, GameObject> activeUiElementDictionary,
            Transform uISpawnPointTransform,
            GameObject targetGameObject)
        {
            base.EnableUI(activeUiElementDictionary, uISpawnPointTransform, targetGameObject);
            SetCache(activeUiElementDictionary, uISpawnPointTransform,
                new CacheRequest(healthBarAsset, SetupHealthBar, SpawnHealthBar)
            );
        }

        private void SpawnHealthBar(AsyncOperationHandle<GameObject> operationHandle)
        {
            if (operationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                ActiveUiElementDictionary.Add(healthBarAsset, operationHandle.Result);
                SetupHealthBar(operationHandle.Result);
            }
        }

        private void SetupHealthBar(GameObject spawnedHealthBar)
        {
            _healthBarCache = spawnedHealthBar.GetComponent<ProgressBar>();
            _entityStats = TargetGameObject.GetComponent<IEntityStatsReference>().EntityStats;
            _health = _entityStats.vitality.health.current;
            _maxHealth = _entityStats.vitality.health.max;
            _health.OnChange += OnCurrentHealthChange;
            _healthBarCache.SetValueWithoutNotify(_health.Value);
            ActiveUiElementDictionary.Add(healthBarAsset, spawnedHealthBar);
        }

        private void CleanHealthBar(GameObject uiElement)
        {
            _health.OnChange -= OnCurrentHealthChange;
        }


        private void OnCurrentHealthChange(float old, float current)
        {
            _healthBarCache.Value = current / _maxHealth.Value;
        }

        public override void DisableUI(Dictionary<AssetReferenceGameObject, GameObject> activeUiElementDictionary,
            GameObject targetGameObject)
        {
            var x = InvalidateCacheEntry(healthBarAsset, _healthBarCache.gameObject, CleanHealthBar);
            activeUiElementDictionary.Add(x.Key, x.Value);
        }
    }
}