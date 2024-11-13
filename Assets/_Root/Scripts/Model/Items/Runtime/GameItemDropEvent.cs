using UnityEngine;

namespace _Root.Scripts.Model.Items.Runtime
{
    public struct GameItemDropEvent
    {
        public GameObject Owner;
        public GameItem Item;
        public Vector3 Position;
        public int Amount;
        
        public GameItemDropEvent(GameObject owner, GameItem item, Vector3 position, int amount)
        {
            this.Owner = owner;
            this.Item = item;
            this.Position = position;
            this.Amount = amount;
        }
    }
}