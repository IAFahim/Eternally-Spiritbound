using System.Collections.Generic;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Focus.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using _Root.Scripts.Presentation.Containers.Runtime;
using Cysharp.Threading.Tasks;
using Soul.Pools.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Presentation.FocusProcessors.Runtime
{
    public class ViewEntityStats : View, IGameObjectView
    {
        public AssetReferenceGameObject statsViewControllerAsset;
        public Sprite healthIcon;
        private List<StatsViewController> statsViewControllers;

        public void Init(FocusReferences focusReferences, GameObject gameObject)
        {
            EntityStatsComponent entityStatsComponent = gameObject.GetComponent<EntityStatsComponent>();
            statsViewControllers = new List<StatsViewController>();
            InitStats(focusReferences, entityStatsComponent).Forget();
            ActiveAll();
        }

        private async UniTaskVoid InitStats(FocusReferences focusReferences, EntityStatsComponent entityStatsComponent)
        {
            var uiSillTransform = focusReferences.UISillTransformPointPadded;
            statsViewControllers.Add(
                await ShowHealth(uiSillTransform, entityStatsComponent.entityStats.vitality.health));
        }


        private async UniTask<StatsViewController> ShowHealth(Transform spawnRect, LimitStat vitalityHealth)
        {
            var statsViewController = await SharedAssetPoolInactive.RequestAsync<StatsViewController>(
                statsViewControllerAsset,
                spawnRect
            );
            statsViewController.Init(healthIcon, "Health", vitalityHealth.current);
            return statsViewController;
        }

        public void ActiveAll()
        {
            foreach (var statsViewController in statsViewControllers)
            {
                statsViewController.gameObject.SetActive(true);
            }
        }

        public override void Return()
        {
            foreach (var statsViewController in statsViewControllers)
            {
                SharedAssetPoolInactive.Return(statsViewControllerAsset, statsViewController.gameObject);
            }
        }
    }
}