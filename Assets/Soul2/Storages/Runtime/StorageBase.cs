using System;
using System.Collections.Generic;
using System.Linq;
using Soul2.Containers.RunTime;
using UnityEngine;

namespace Soul2.Storages.Runtime
{
    [Serializable]
    public abstract class StorageBase<TElement, TValue> : IStorageBase<TElement, TValue>
    {
        public event Action<TElement, TValue, TValue> OnItemChanged;

        [SerializeField] protected Pair<TElement, TValue>[] startingElements;
        protected Dictionary<TElement, TValue> _elements;

        public Pair<TElement, TValue>[] StartingElements => startingElements;
        public int Count => _elements.Count;
        public string Guid { get; set; }

        public void Initialize() => SetElements(startingElements);
        
        private void SetElements(Pair<TElement, TValue>[] loadedData)
        {
            _elements = new Dictionary<TElement, TValue>();
            foreach (var pair in loadedData)
            {
                if (_elements.TryGetValue(pair.Key, out var currentValue))
                    _elements[pair.Key] = Add(currentValue, pair.Value);
                else
                    _elements.Add(pair.Key, pair.Value);
            }
        }
        

        public bool TryAdd(TElement element, TValue amount, out TValue added, bool saveOnSuccess = false)
        {
            added = default;
            if (_elements.TryGetValue(element, out TValue currentAmount))
            {
                var newAmount = Add(currentAmount, amount);
                _elements[element] = newAmount;
                added = amount;
                OnItemChanged?.Invoke(element, currentAmount, newAmount);
            }
            else
            {
                _elements.Add(element, amount);
                added = amount;
                OnItemChanged?.Invoke(element, default, amount);
            }

            if (saveOnSuccess) SaveData();
            return true;
        }

        public bool TryAdd(IEnumerable<Pair<TElement, TValue>> elementsToAdd,
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

        public bool TryRemove(TElement element, TValue amount, out TValue removed, bool saveOnSuccess = false)
        {
            removed = default;
            if (_elements.TryGetValue(element, out TValue currentAmount) && Compare(currentAmount, amount) >= 0)
            {
                var newAmount = Remove(currentAmount, amount);
                _elements[element] = newAmount;
                removed = amount;
                OnItemChanged?.Invoke(element, currentAmount, newAmount);

                if (Compare(newAmount, default) == 0) _elements.Remove(element);
                if (saveOnSuccess) SaveData();
                return true;
            }

            return false;
        }

        public bool TryRemove(IEnumerable<Pair<TElement, TValue>> elementsToRemove,
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

        public bool HasEnough(TElement element, TValue amount)
        {
            return _elements.TryGetValue(element, out var currentAmount) && Compare(currentAmount, amount) >= 0;
        }

        public bool HasEnough(TElement element, TValue amount, out TValue remainingAmount)
        {
            if (_elements.TryGetValue(element, out TValue currentAmount))
            {
                remainingAmount = Remove(currentAmount, amount);
                return Compare(remainingAmount, default) >= 0;
            }

            remainingAmount = default;
            return false;
        }

        public bool HasEnough(IEnumerable<Pair<TElement, TValue>> elementsToCheck,
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

        public void Clear(bool save = false)
        {
            _elements.Clear();
            if (save) SaveData();
        }

        public Dictionary<TElement, TValue> GetAllElements()
        {
            return new Dictionary<TElement, TValue>(_elements);
        }

        public abstract void LoadData(string guid);
        public void SetData(Pair<TElement, TValue>[] data) => SetElements(data);
        public abstract void SaveData(Pair<TElement, TValue>[] data);

        public void SaveData()
        {
            SaveData(_elements.Select(kvp => new Pair<TElement, TValue>(kvp.Key, kvp.Value)).ToArray());
        }

        public abstract TValue Add(TValue a, TValue b);
        public abstract TValue Remove(TValue a, TValue b);
        public abstract int Compare(TValue a, TValue b);
    }
}