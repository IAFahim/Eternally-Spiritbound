using System;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Model.Assets.Runtime;
using Soul.Interactions.Runtime;
using Soul.Items.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Model.Items.Runtime
{
    [Serializable]
    [CreateAssetMenu(fileName = "Game Item", menuName = "Scriptable/Asset/Items/New")]
    public class GameItem : AssetScript, IItemBase<GameObject>, IDropStrategy
    {
        public DropStrategyScriptable dropStrategy;

        [SerializeField] private int maxStack;
        [SerializeField] private bool consumable;

        [SerializeField] private bool dropOnDeath;
        [SerializeField] private bool autoPickup;
        public bool Consumable => consumable;
        public bool DropOnDeath => dropOnDeath;
        public bool IsStackable => maxStack > 1;
        public bool AutoPickup => autoPickup;

        public int MaxStack
        {
            get => maxStack;
            set => maxStack = value;
        }
        

        public virtual void OnPick(GameObject picker)
        {
        }

        public virtual void OnUse(GameObject user)
        {
        }


        public float DropRange => dropStrategy.DropRange;

        public void OnDrop(GameObject user, Vector3 position, int amount)
        {
            dropStrategy.OnDrop(assetReference, position, amount);
        }


        public static implicit operator Sprite(GameItem itemBase) => itemBase.icon;
        public static implicit operator string(GameItem itemBase) => itemBase.guid;

        public static implicit operator AssetReferenceGameObject(GameItem itemBase) =>
            itemBase.assetReference;
    }
}