using System;
using _Root.Scripts.Game.QuickPickup.Runtime.Handlers;
using _Root.Scripts.Model.Assets.Runtime;
using Cysharp.Threading.Tasks;
using Pancake.Pools;
using Soul.QuickPickup.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Game.QuickPickup.Runtime
{
    [Serializable]
    [CreateAssetMenu(fileName = "QuickPickupManager", menuName = "Scriptable/QuickPickup/QuickPickupManager")]
    public class QuickItemPickupManager : QuickPickupManager<PickupContainerBase<AssetScript>>
    {
        public PickupDetectHandlerBase<AssetScript> detectHandlerBase;
        public PickupActiveTweenHandlerBase<AssetScript> activeTweenHandlerBase;
        public PickupHomingHandlerBase<AssetScript> pickupHomingHandlerBase;


        public void Setup()
        {
            base.Enable(new PickupHandlerBase<PickupContainerBase<AssetScript>>[]
            {
                detectHandlerBase,
                activeTweenHandlerBase,
                pickupHomingHandlerBase
            });
        }


        public async UniTaskVoid Add(AssetScript assetScript, Vector3 position, int amount)
        {
            var gameObject = await assetScript.AssetReference.RequestAsync(position, Random.rotation);
            var pickupContainer = new PickupContainerBase<AssetScript>(assetScript, gameObject.transform, amount);
            Add(pickupContainer);
        }


    }
}