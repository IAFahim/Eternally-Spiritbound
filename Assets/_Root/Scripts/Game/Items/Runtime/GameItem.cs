using Soul.Storages.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Items.Runtime
{
    [CreateAssetMenu(fileName = "Coin", menuName = "Scriptable/Items/New")]
    public class GameItem : ItemBase
    { 
        public override bool CanPick<TComponent>(GameObject picker, Vector3 position, int amount, out TComponent pickerComponent)
        {
            return picker.TryGetComponent(out pickerComponent) &&
                   pickerComponent.CanAdd(this, amount, out _);
        }

        public override bool TryPick(GameObject picker, Vector3 position, int amount )
        {
            return CanPick(picker, position, amount, out IStorageBase<string, int> storage) && 
                storage.TryAdd(this, amount, out int added) && added == amount;
        }

        public override bool CanUse<TComponent>(GameObject user, Vector3 position, int amount, out TComponent userComponent)
        {
            return user.TryGetComponent(out userComponent) && userComponent.HasEnough(this, amount);
        }
        

        public override bool TryUse(GameObject user, Vector3 position, int amount)
        {
            if (CanUse(user, position, amount, out IStorageBase<string, int> storage))
            {
                storage.TryRemove(this.name, amount, out _);
                return true;
            }

            return false;
        }

        public override bool CanSpawn(Vector3 position, int amount )
        {
            return true;
        }

        public override bool CanDrop<TComponent>(GameObject dropper, Vector3 position, int amount, out TComponent dropperComponent)
        {
            return dropper.TryGetComponent(out dropperComponent) && dropperComponent.HasEnough(this, amount);
        }
        

        public override bool TrySpawn(Vector3 position, int amount )
        {
            Trigger(new ItemDropEvent(this, position, amount));
            return true;
        }

        public override bool TryDrop(GameObject dropper, Vector3 position, int amount )
        {
            if (dropper.TryGetComponent(out IStorageBase<string, int> storage))
            {
                bool dropHasInStock = storage.TryRemove(this.name, amount, out int removed) && removed == amount;
                if (dropHasInStock) Trigger(new ItemDropEvent(this, position, amount));
                return dropHasInStock;
            }

            return false;
        }
    }
}