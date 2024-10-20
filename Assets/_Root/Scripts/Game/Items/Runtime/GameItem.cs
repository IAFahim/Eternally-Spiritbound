using System;
using _Root.Scripts.Game.Interactables.Runtime;
using Pancake;
using Pancake.Pools;
using Soul.Items.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Items.Runtime
{
    [Serializable]
    [CreateAssetMenu(fileName = "Coin", menuName = "Scriptable/Items/New")]
    public class GameItem : ScriptableObject, IItemBase<GameObject>, IPickupStrategy, IDropStrategy
    {
        [Guid] public string guid;
        public AssetReferenceGameObject assetReferenceGameObject;
        public DropStrategyScriptable dropStrategy;

        [SerializeField] private string itemName;
        [SerializeField, TextArea(3, 10)] public string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private int maxStack;
        [SerializeField] private bool consumable;

        [SerializeField] private bool dropOnDeath;
        [SerializeField] private bool autoPickup;
        [SerializeField] private float pickupRange = 5;
        public string ItemName => itemName;
        public string Description => description;
        public Sprite Icon => icon;
        public bool Consumable => consumable;
        public bool DropOnDeath => dropOnDeath;
        public bool IsStackable => maxStack > 1;
        public bool AutoPickup => autoPickup;
        public float PickupRange => pickupRange;

        private AsyncAddressableGameObjectPool _pool;
        
        public AsyncAddressableGameObjectPool Pool => _pool;

        public int MaxStack
        {
            get => maxStack;
            set => maxStack = value;
        }


        public virtual void OnAddedToInventory(GameObject user, int amount)
        {
        }

        public virtual void OnPick(GameObject picker)
        {
        }

        public virtual void OnUse(GameObject user)
        {
        }
        public float DropRange => dropStrategy.DropRange;

        public virtual void OnDrop(GameObject user, Vector3 position, int amount, Action<GameObject> onDropped)
        {
            dropStrategy.OnDrop(_pool, position, amount, onDropped);
        }


        public static implicit operator Sprite(GameItem itemBase) => itemBase.Icon;
        public static implicit operator string(GameItem itemBase) => itemBase.guid;

        public static implicit operator AssetReferenceGameObject(GameItem itemBase) =>
            itemBase.assetReferenceGameObject;
    }
}