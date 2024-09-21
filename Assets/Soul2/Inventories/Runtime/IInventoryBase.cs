using System.Collections.Generic;
using Soul2.Containers.RunTime;
using Soul2.Items.Runtime;

namespace Soul2.Inventories.Runtime
{
    public interface IInventoryBase
    {
        int MaxSlots { get; set; }
        int AvailableSlots { get; }
        public bool HasSpace(IStackAble stackable, int amount);
        public int SpaceLeft(IStackAble stackable);
        bool CanAddAll(IEnumerable<Pair<IStackAble, int>> items);
        void OnItemAdded(IStackAble item, int amount);
        void OnItemRemoved(IStackAble item, int amount);
    }
}