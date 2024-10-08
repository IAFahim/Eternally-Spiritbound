using System;
using _Root.Scripts.Game.Interactables;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.Items.Runtime.Storage;
using Pancake;
using Soul.Items.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Items.Runtime
{
    [Serializable]
    [CreateAssetMenu(fileName = "Coin", menuName = "Scriptable/Items/New")]
    public class GameItem : ScriptableObject, IItemBase<GameObject>, IPickupStrategy
    {
        [Guid] public string guid;
        public bool shouldPool = true;
        public AssetReferenceGameObject assetReferenceGameObject;

        [SerializeField] private string itemName;
        [SerializeField, TextArea(3, 10)] public string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private int maxStack;
        [SerializeField] private bool consumable;

        [SerializeField] private bool autoPickup;
        [SerializeField] private float pickupRange = 5;
        public string ItemName => itemName;
        public string Description => description;
        public Sprite Icon => icon;
        public bool Consumable => consumable;
        public bool IsStackable => maxStack > 1;
        public bool AutoPickup => autoPickup;
        public float PickupRange => pickupRange;
        
        public int MaxStack
        {
            get => maxStack;
            set => maxStack = value;
        }


        public virtual void Initialize(GameObject user, int amount)
        {
        }
        
        public bool CanPick<TComponent>(GameObject picker, Vector3 position, int amount,
            out TComponent pickerComponent) where TComponent : IGameItemStorageReference
        {
            return picker.TryGetComponent(out pickerComponent) &&
                   pickerComponent.GameItemStorage.CanAdd(this, amount, out _);
        }


        public virtual bool TryPick(GameObject picker, Vector3 position, int amount)
        {
            return CanPick(picker, position, amount, out IGameItemStorageReference itemStorageReference) &&
                   itemStorageReference.GameItemStorage.TryAdd(this, amount, out int added) && added == amount;
        }

        public virtual bool CanUse<TComponent>(GameObject user, Vector3 position, int amount,
            out TComponent userComponent) where TComponent : IGameItemStorageReference
        {
            return user.TryGetComponent(out userComponent) && userComponent.GameItemStorage.HasEnough(this, amount);
        }

        public virtual bool TryUse(GameObject user, Vector3 position, int amount)
        {
            if (CanUse(user, position, amount, out IGameItemStorageReference itemStorageReference))
            {
                itemStorageReference.GameItemStorage.TryRemove(this, amount, out _);
                return true;
            }

            return false;
        }


        public static implicit operator Sprite(GameItem itemBase) => itemBase.Icon;
        public static implicit operator string(GameItem itemBase) => itemBase.guid;

        public static implicit operator AssetReferenceGameObject(GameItem itemBase) =>
            itemBase.assetReferenceGameObject;
    }
}