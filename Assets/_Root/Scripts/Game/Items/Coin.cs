using Soul2.Items.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Items
{
    [CreateAssetMenu(fileName = "Coin", menuName = "Scriptable/Items/Coin")]
    public class Coin : ItemBase
    {
        public override bool TryPick(GameObject picker, Vector3 position, int amount = 1)
        {
            throw new System.NotImplementedException();       
        }

        public override bool TryUse(GameObject user, Vector3 position, int amount = 1)
        {
            throw new System.NotImplementedException();
        }

        public override bool TryDrop(GameObject dropper, Vector3 position, int amount = 1)
        {
            throw new System.NotImplementedException();
        }
    }
}