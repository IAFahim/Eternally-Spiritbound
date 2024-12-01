using _Root.Scripts.Game.Interactables.Runtime.Focus;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using _Root.Scripts.Presentation.Containers.Runtime;
using Cysharp.Threading.Tasks;
using Soul.Pools.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Presentation.FocusProcessors.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/FocusProcessor/Sub/ViewEntityStats", fileName = "ViewEntityStats")]
    public class ViewEntityStats : View, IGameObjectView
    {
        [SerializeField] private AssetReferenceGameObject statsPanelBGAsset;
        [SerializeField] private AssetReferenceGameObject statsViewAsset;
        [SerializeField] private AssetReferenceGameObject statsViewControllerAsset;
        [SerializeField] private Sprite healthIcon;
        [SerializeField] private FocusManagerScript focusManagerScript;

        private GameObject statsPanelBG;
        private GameObject statsView;
        private StatsViewController[] statsViewControllers;

        public void Init(GameObject gameObject)
        {
            EntityStatsComponent entityStatsComponent = gameObject.GetComponent<EntityStatsComponent>();
            InitStats(entityStatsComponent).Forget();
        }

        private async UniTaskVoid InitStats(EntityStatsComponent entityStatsComponent)
        {
            var uiSillTransform = focusManagerScript.FocusReferences.UISillTransformPointPadded;
            statsPanelBG = await SharedAssetPoolInactive.RequestAsync(statsPanelBGAsset, uiSillTransform);
            statsView = await SharedAssetPoolInactive.RequestAsync(statsViewAsset, uiSillTransform);
            var spawnRect = statsView.transform;
            statsViewControllers = new StatsViewController[]
            {
                await ShowHealth(spawnRect, entityStatsComponent.entityStats.vitality.health)
            };
            ActiveAll();
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
            statsPanelBG.SetActive(true);
            statsView.SetActive(true);
            foreach (var statsViewController in statsViewControllers) statsViewController.gameObject.SetActive(true);
        }

        public override void Return()
        {
            foreach (var statsViewController in statsViewControllers)
            {
                SharedAssetPoolInactive.Return(statsViewControllerAsset, statsViewController.gameObject);
            }

            SharedAssetPoolInactive.Return(statsViewAsset, statsView);
            SharedAssetPoolInactive.Return(statsPanelBGAsset, statsPanelBG);
        }
    }
}