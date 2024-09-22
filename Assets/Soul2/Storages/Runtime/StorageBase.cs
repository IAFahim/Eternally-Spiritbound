using System;
using System.Collections.Generic;
using Soul2.Containers.RunTime;
using UnityEngine;

namespace Soul2.Storages.Runtime
{
    [Serializable]
    public abstract class StorageBase<TElement, TValue> : IStorageBase<TElement, TValue>
    {
        [SerializeField, Tooltip("Starting element when you load first time")]
        protected Pair<TElement, TValue>[] startingElementReference;

        protected Dictionary<TElement, TValue> Elements = new();
        
        public int Count => Elements.Count;

        public event Action<TElement, TValue, TValue> OnItemChanged;
        public Pair<TElement, TValue>[] StartingElementReference => startingElementReference;
        public string Guid { get; set; }

        protected virtual void SetElements(Pair<TElement, TValue>[] loadedData)
        {
            Elements.Clear();
            foreach (var pair in loadedData)
            {
                if (Elements.TryGetValue(pair.Key, out var currentValue))
                    Elements[pair.Key] = Add(currentValue, pair.Value);
                else
                    Elements.Add(pair.Key, pair.Value);
            }
        }

        public virtual bool TryAdd(TElement element, TValue amount, out TValue added, bool saveOnSuccess = false)
        {
            added = default;
            if (Elements.TryGetValue(element, out TValue currentAmount))
            {
                var newAmount = Add(currentAmount, amount);
                Elements[element] = newAmount;
                added = amount;
                OnItemChanged?.Invoke(element, currentAmount, newAmount);
            }
            else
            {
                Elements.Add(element, amount);
                added = amount;
                OnItemChanged?.Invoke(element, default, amount);
            }

            if (saveOnSuccess) SaveData();
            return true;
        }

        public virtual bool TryAdd(IEnumerable<Pair<TElement, TValue>> elementsToAdd,
            out List<Pair<TElement, TValue>> failedToAdd, bool saveOnSuccess = false)
        {
            failedToAdd = new List<Pair<TElement, TValue>>();
            foreach (var pair in elementsToAdd)
            {
                if (!TryAdd(pair.Key, pair.Value, out _, false))
                    failedToAdd.Add(pair);
            }

            if (saveOnSuccess && failedToAdd.Count == 0) SaveData();
            return failedToAdd.Count == 0;
        }

        public virtual bool TryRemove(TElement element, TValue amount, out TValue removed, bool saveOnSuccess = false)
        {
            removed = default;
            if (Elements.TryGetValue(element, out TValue currentAmount) && Compare(currentAmount, amount) >= 0)
            {
                var newAmount = Remove(currentAmount, amount);
                Elements[element] = newAmount;
                removed = amount;
                OnItemChanged?.Invoke(element, currentAmount, newAmount);

                if (Compare(newAmount, default) == 0) Elements.Remove(element);
                if (saveOnSuccess) SaveData();
                return true;
            }

            return false;
        }

        public virtual bool TryRemove(IEnumerable<Pair<TElement, TValue>> elementsToRemove,
            out List<Pair<TElement, TValue>> failedToRemove, bool saveOnSuccess = false)
        {
            failedToRemove = new List<Pair<TElement, TValue>>();
            foreach (var pair in elementsToRemove)
            {
                if (!TryRemove(pair.Key, pair.Value, out _, false))
                    failedToRemove.Add(pair);
            }

            if (saveOnSuccess && failedToRemove.Count == 0) SaveData();
            return failedToRemove.Count == 0;
        }

        public bool RemoveAll(TElement elementsToRemove, out TValue removed, bool saveOnSuccess = false)
        {
            if (Elements.TryGetValue(elementsToRemove, out TValue currentAmount))
            {
                removed = currentAmount;
                Elements.Remove(elementsToRemove);
                if (saveOnSuccess) SaveData();
                return true;
            }

            removed = default;
            return false;
        }

        public virtual bool HasEnough(TElement element, TValue amount)
        {
            return Elements.TryGetValue(element, out var currentAmount) && Compare(currentAmount, amount) >= 0;
        }

        public virtual bool HasEnough(TElement element, TValue amount, out TValue remainingAmount)
        {
            if (Elements.TryGetValue(element, out TValue currentAmount))
            {
                remainingAmount = Remove(currentAmount, amount);
                return Compare(remainingAmount, default) >= 0;
            }

            remainingAmount = default;
            return false;
        }

        public virtual bool HasEnough(IEnumerable<Pair<TElement, TValue>> elementsToCheck,
            out List<Pair<TElement, TValue>> insufficientElements)
        {
            insufficientElements = new List<Pair<TElement, TValue>>();
            foreach (var pair in elementsToCheck)
            {
                if (!HasEnough(pair.Key, pair.Value))
                {
                    insufficientElements.Add(pair);
                }
            }

            return insufficientElements.Count == 0;
        }

        public virtual void Clear(bool save = false)
        {
            Elements.Clear();
            if (save) SaveData();
        }

        public virtual Dictionary<TElement, TValue> GetAllElements()
        {
            return Elements;
        }

        public abstract void LoadData(string guid);
        public virtual void SetData(Pair<TElement, TValue>[] data) => SetElements(data);
        public abstract void SaveData(Pair<TElement, TValue>[] data);

        public virtual void SaveData()
        {
            // SaveData(Elements.Select(kvp => new Pair<TElement, TValue>(kvp.Key, kvp.Value)).ToArray());
            var data = new Pair<TElement, TValue>[Elements.Count];
            var index = 0;
            foreach (var kvp in Elements)
            {
                data[index] = new Pair<TElement, TValue>(kvp.Key, kvp.Value);
                index++;
            }

            SaveData(data);
        }

        public abstract TValue Add(TValue a, TValue b);
        public abstract TValue Remove(TValue a, TValue b);
        public abstract int Compare(TValue a, TValue b);

        // New method to get the current amount of an element
        public virtual bool TryGetAmount(TElement element, out TValue amount)
        {
            return Elements.TryGetValue(element, out amount);
        }
    }
}