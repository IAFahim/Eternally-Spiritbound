using UnityEngine;

namespace _Root.Scripts.Game.Items.Runtime
{
    public struct ItemDropEvent
    {
        public ItemDropEvent(GameItem itemBase,  Vector3 position, int amount = 1)
        {
            ItemBase = itemBase;
            Position = position;
            Amount = amount;
        }

        public GameItem ItemBase { get; }
        public Vector3 Position { get; }
        public int Amount { get; }
        
        public static implicit operator GameItem(ItemDropEvent itemDropEvent) => itemDropEvent.ItemBase;
        public static implicit operator Vector3(ItemDropEvent itemDropEvent) => itemDropEvent.Position;
        public static implicit operator int(ItemDropEvent itemDropEvent) => itemDropEvent.Amount;

    }
}