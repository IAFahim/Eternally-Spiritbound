namespace _Root.Scripts.Game.Items.Runtime
{
    public struct GameItemDropEvent
    {
        public GameItemDropEvent(GameItem item,  UnityEngine.Vector3 position, int amount = 1)
        {
            Item = item;
            Position = position;
            Amount = amount;
        }

        public GameItem Item { get; }
        public UnityEngine.Vector3 Position { get; }
        public int Amount { get; }
        
        public static implicit operator GameItem(GameItemDropEvent gameItemDropEvent) => gameItemDropEvent.Item;
        public static implicit operator UnityEngine.Vector3(GameItemDropEvent gameItemDropEvent) => gameItemDropEvent.Position;
        public static implicit operator int(GameItemDropEvent gameItemDropEvent) => gameItemDropEvent.Amount;

    }
}