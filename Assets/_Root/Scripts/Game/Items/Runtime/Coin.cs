using JetBrains.Annotations;
using Pancake;
using Soul2.Storages.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Items.Runtime
{
    [CreateAssetMenu(fileName = "Coin", menuName = "Scriptable/Items/Coin")]
    public class Coin : GameItem
    {
        public override bool TryPick(GameObject picker, Vector3 position, int amount = 1)
        {
            return picker.TryGetComponent(out IStorageBase<string, int> storage) &&
                   storage.TryAdd(this, amount, out int added) && added == amount;
        }

        public override bool TryUse(GameObject user, Vector3 position, int amount = 1)
        {
            throw new System.NotImplementedException();
        }

        public override bool TryDrop([CanBeNull] GameObject dropper, Vector3 position, int amount = 1)
        {
            bool dropperNull = dropper == null;
            if (dropperNull)
            {
                Trigger(new GameItemDropEvent(this, position, amount));
                return true;
            }

            if (dropper.TryGetComponent(out IStorageBase<string, int> storage))
            {
                bool dropHasInStock = storage.TryRemove(this, amount, out int removed) && removed == amount;
                if (dropHasInStock) Trigger(new GameItemDropEvent(this, position, amount));
                return true;
            }

            return false;
        }
    }
}