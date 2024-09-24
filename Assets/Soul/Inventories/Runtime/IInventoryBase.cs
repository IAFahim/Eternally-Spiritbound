using System.Collections.Generic;
using Soul.Serializers.Runtime;

namespace Soul.Inventories.Runtime
{
    public interface IInventoryBase<TItemBase> 
    {
        int MaxSlots { get; set; }
        int AvailableSlots { get; }
        public bool HasSpace(TItemBase stackable, int amount);
        public int SpaceLeft(TItemBase stackable);
        bool CanAddAll(IEnumerable<Pair<TItemBase, int>> items);
        void OnItemAdded(TItemBase item, int amount);
        void OnItemRemoved(TItemBase item, int amount);
    }
}