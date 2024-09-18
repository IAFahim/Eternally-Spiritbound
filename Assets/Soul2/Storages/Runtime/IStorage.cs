using System;
using System.Collections.Generic;
using Soul2.Containers.RunTime;

namespace Soul2.Storages.Runtime
{
    public interface IStorage<T>
    {
        public string Guid { get; }
        public Pair<T, int>[] StartingElements { get; }
        public int Count { get; }
        public event Action<T, int, int> OnItemChanged;
        public void Load();
        public void Save();
        public bool TryAdd(T element, int amount, out int added, bool saveOnSuccess = true);

        public bool TryAdd(IEnumerable<Pair<T, int>> elementsToAdd, out List<Pair<T, int>> failedToAdd,
            bool saveOnSuccess = true);

        public bool TryRemove(T item, int amount, out int removed, bool saveOnSuccess = true);

        public bool TryRemove(IEnumerable<Pair<T, int>> elementsToRemove, out List<Pair<T, int>> failedToRemove,
            bool saveOnSuccess = true);

        public bool HasEnough(T item, int amount);
        public bool HasEnough(T element, int amount, out int remainingAmount);
        public bool HasEnough(IEnumerable<Pair<T, int>> elementsToCheck, out List<Pair<T, int>> insufficientElements);
    }
}