using System;
using System.Collections.Generic;
using Soul2.Containers.RunTime;
using Soul2.Datas.Runtime.Interface;

namespace Soul2.Storages.Runtime
{
    public interface IStorageBase<TElement, TValue> : IStorageAdapter<Pair<TElement, TValue>[]>
    {
        void Initialize();
        Pair<TElement, TValue>[] StartingElements { get; }
        int Count { get; }
        event Action<TElement, TValue, TValue> OnItemChanged;
        bool TryAdd(TElement element, TValue amount, out TValue added, bool saveOnSuccess = false);
        bool TryAdd(IEnumerable<Pair<TElement, TValue>> elementsToAdd, out List<Pair<TElement, TValue>> failedToAdd, bool saveOnSuccess = false);
        bool TryRemove(TElement element, TValue amount, out TValue removed, bool saveOnSuccess = false);
        bool TryRemove(IEnumerable<Pair<TElement, TValue>> elementsToRemove, out List<Pair<TElement, TValue>> failedToRemove, bool saveOnSuccess = false);
        bool HasEnough(TElement element, TValue amount);
        bool HasEnough(TElement element, TValue amount, out TValue remainingAmount);
        bool HasEnough(IEnumerable<Pair<TElement, TValue>> elementsToCheck, out List<Pair<TElement, TValue>> insufficientElements);
        void Clear(bool save = false);
        Dictionary<TElement, TValue> GetAllElements();
        TValue Add(TValue a, TValue b);
        TValue Remove(TValue a, TValue b);
        int Compare(TValue a, TValue b);
    }
}