using System.Collections.Generic;
using _Root.Scripts.Game.Stats.Runtime.Controller;
using _Root.Scripts.Game.UiLoaders.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityProgressBar;

namespace _Root.Scripts.Presentation.UIProviders.Runtime
{
    [CreateAssetMenu(fileName = "BoatNavigationUIProviderScriptable",
        menuName = "Scriptable/UIProviders/BoatNavigationUIProviderScriptable")]
    public class BoatNavigationUIProviderScriptable : UIProviderScriptable
    {
        public AssetReferenceGameObject healthBarPrefab;
        public ProgressBar healthBar;

        public override void EnableUI(HashSet<GameObject> activeUiElementHashSet, Transform uISpawnPointTransform,
            GameObject gameObject)
        {
            if (healthBar != null && activeUiElementHashSet.Contains(healthBar.gameObject)) return;
            Addressables.InstantiateAsync(healthBarPrefab, uISpawnPointTransform).Completed += handle =>
            {
                healthBar = handle.Result.GetComponent<ProgressBar>();
                var entityStats = gameObject.GetComponent<IEntityStatsReference>().EntityStats;
                entityStats.vitality.health.current.OnChange += OnCurrentHealthChange;
                healthBar.SetValueWithoutNotify(entityStats.vitality.health.current.Value);
                activeUiElementHashSet.Add(healthBar.gameObject);
            };
        }


        private void OnCurrentHealthChange(float old, float current)
        {
            healthBar.Value = current;
        }

        public override GameObject[] DisableUI(GameObject gameObject)
        {
            List<GameObject> uiElements = new List<GameObject>();
            if (gameObject.TryGetComponent<IEntityStatsReference>(out var entityStatsReference))
            {
                entityStatsReference.EntityStats.vitality.health.current.OnChange -= OnCurrentHealthChange;
                uiElements.Add(healthBar.gameObject);
            }

            return uiElements.ToArray();
        }
    }
}