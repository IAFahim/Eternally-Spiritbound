using System;
using System.Collections.Generic;
using Soul2.Containers.RunTime;
using Soul2.Items.Runtime;
using Soul2.Storages.Runtime;
using UnityEngine;

namespace Soul2.Inventories.Runtime
{
    [Serializable]
    public abstract class InventoryBase : StorageBase<IStackAble, int>, IInventoryBase
    {
        [SerializeField] private int maxSlots = 20;

        public int MaxSlots
        {
            get => maxSlots;
            set => maxSlots = value;
        }

        public int AvailableSlots => MaxSlots - Count;

        public override bool TryAdd(IStackAble stackable, int amount, out int added, bool saveOnSuccess = false)
        {
            added = 0;

            if (amount <= 0)
                return false;

            if (!TryGetAmount(stackable, out int currentAmount))
            {
                if (AvailableSlots <= 0) return false; // Inventory is full
            }

            int maxStack = stackable.MaxStack;
            int availableSpace = maxStack - currentAmount;

            if (availableSpace <= 0) return false; // Item stack is full

            int toAdd = Mathf.Min(amount, availableSpace);
            bool result = base.TryAdd(stackable, toAdd, out added, saveOnSuccess);

            if (result && added > 0) OnItemAdded(stackable, added);

            return result;
        }

        public override bool TryAdd(IEnumerable<Pair<IStackAble, int>> elementsToAdd,
            out List<Pair<IStackAble, int>> failedToAdd, bool saveOnSuccess = false)
        {
            failedToAdd = new List<Pair<IStackAble, int>>();

            foreach (var pair in elementsToAdd)
            {
                int remainingToAdd = pair.Value;
                while (remainingToAdd > 0)
                {
                    if (TryAdd(pair.Key, remainingToAdd, out int added, false))
                    {
                        remainingToAdd -= added;
                    }
                    else
                    {
                        break;
                    }
                }

                if (remainingToAdd > 0) failedToAdd.Add(new Pair<IStackAble, int>(pair.Key, remainingToAdd));
            }

            if (saveOnSuccess && failedToAdd.Count == 0)
                SaveData();

            return failedToAdd.Count == 0;
        }

        public override bool TryRemove(IStackAble stackable, int amount, out int removed, bool saveOnSuccess = false)
        {
            removed = 0;

            if (amount <= 0)
                return false;

            if (!TryGetAmount(stackable, out int currentAmount))
                return false;

            int toRemove = Mathf.Min(amount, currentAmount);
            bool result = base.TryRemove(stackable, toRemove, out removed, saveOnSuccess);

            if (result && removed > 0) OnItemRemoved(stackable, removed);

            return result;
        }

        public override bool TryRemove(IEnumerable<Pair<IStackAble, int>> elementsToRemove,
            out List<Pair<IStackAble, int>> failedToRemove, bool saveOnSuccess = false)
        {
            failedToRemove = new List<Pair<IStackAble, int>>();

            foreach (var pair in elementsToRemove)
            {
                if (TryRemove(pair.Key, pair.Value, out int removed, false))
                {
                    if (removed < pair.Value)
                        failedToRemove.Add(new Pair<IStackAble, int>(pair.Key, pair.Value - removed));
                }
                else failedToRemove.Add(pair);
            }

            if (saveOnSuccess && failedToRemove.Count == 0) SaveData();

            return failedToRemove.Count == 0;
        }

        public bool HasSpace(IStackAble stackable, int amount)
        {
            if (!TryGetAmount(stackable, out int currentAmount))
            {
                return AvailableSlots > 0 && amount <= stackable.MaxStack;
            }

            return currentAmount + amount <= stackable.MaxStack;
        }
        
        public int SpaceLeft(IStackAble stackable)
        {
            if (!TryGetAmount(stackable, out int currentAmount))
            {
                return AvailableSlots * stackable.MaxStack;
            }

            return (stackable.MaxStack - currentAmount) + (AvailableSlots - 1) * stackable.MaxStack;
        }

        public bool CanAddAll(IEnumerable<Pair<IStackAble, int>> items)
        {
            var tempInventory = new Dictionary<IStackAble, int>(_elements);
            int availableSlots = AvailableSlots;

            foreach (var pair in items)
            {
                if (tempInventory.TryGetValue(pair.Key, out int currentAmount))
                {
                    if (currentAmount + pair.Value > pair.Key.MaxStack)
                        return false;
                    tempInventory[pair.Key] = currentAmount + pair.Value;
                }
                else
                {
                    if (availableSlots <= 0)
                        return false;
                    if (pair.Value > pair.Key.MaxStack)
                        return false;
                    tempInventory[pair.Key] = pair.Value;
                    availableSlots--;
                }
            }

            return true;
        }

        public override int Add(int a, int b) => a + b;
        public override int Remove(int a, int b) => Mathf.Max(0, a - b);
        public override int Compare(int a, int b) => a.CompareTo(b);

        public abstract void OnItemAdded(IStackAble item, int amount);
        public abstract void OnItemRemoved(IStackAble item, int amount);
    }
}