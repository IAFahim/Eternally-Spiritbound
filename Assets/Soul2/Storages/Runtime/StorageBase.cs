using System;
using System.Collections.Generic;
using Soul2.Containers.RunTime;
using Soul2.Datas.Runtime;
using Soul2.Datas.Runtime.Interface;
using UnityEngine;

namespace Soul2.Storages.Runtime
{
    /// <summary>
    /// Abstract class representing a storage for items.
    /// </summary>
    /// <typeparam name="T">Type of the items to be stored.</typeparam>
    [Serializable]
    public abstract class StorageBase<T> : IStorageBase<T>, IDataAdapter<Pair<T, int>[]>
    {
        public event Action<T, int, int> OnItemChanged;
        [SerializeField] protected Pair<T, int>[] startingElements;
        private Dictionary<T, int> _elements;


        /// <summary>
        /// Gets the starting elements of the storage.
        /// </summary>
        public Pair<T, int>[] StartingElements => startingElements;

        /// <summary>
        /// Gets the count of different items in the storage.
        /// </summary>
        public int Count => _elements.Count;


        /// <summary>
        /// Gets the core unique identifier 
        /// </summary>
        public string Guid { get; private set; }

        /// <summary>
        /// Mix with guid To get unique an identifier for the storage.
        /// </summary>
        public abstract string DataKey { get; }

        public Pair<T, int>[] DefaultData => startingElements;
        public abstract Pair<T, int>[] LoadData();

        public abstract void SaveData(Pair<T, int>[] data);

        public void FirstLoad(string guid)
        {
            Guid = guid;
            startingElements = LoadData();
            Initialize();
        }

        public void Save()
        {
            SaveData(_elements.ToPairArray());
        }

        public void Initialize() => SetElements(startingElements);

        public void SetElements(Pair<T, int>[] loadedData)
        {
            _elements = new Dictionary<T, int>();
            foreach (var loadedKeyValuePair in loadedData)
            {
                if (_elements.ContainsKey(loadedKeyValuePair.Key))
                    _elements[loadedKeyValuePair.Key] += loadedKeyValuePair.Value;
                else _elements.Add(loadedKeyValuePair.Key, loadedKeyValuePair.Value);
            }
        }


        /// <summary>
        /// Tries to add an element to the storage.
        /// </summary>
        /// <param name="element">The element to add.</param>
        /// <param name="amount">The amount to add.</param>
        /// <param name="added">The amount actually added.</param>
        /// <param name="saveOnSuccess">Whether to save the storage on success.</param>
        /// <returns>True if the element was added successfully, otherwise false.</returns>
        public bool TryAdd(T element, int amount, out int added, bool saveOnSuccess = true)
        {
            added = 0;
            if (_elements.TryGetValue(element, out int currentAmount))
            {
                _elements[element] = currentAmount + amount;
                added = amount;
                OnItemChanged?.Invoke(element, currentAmount, currentAmount + amount);
            }
            else
            {
                _elements.Add(element, amount);
                added = amount;
                OnItemChanged?.Invoke(element, 0, amount);
            }

            if (saveOnSuccess) SaveData(_elements.ToPairArray());
            return true;
        }

        /// <summary>
        /// Tries to add multiple elements to the storage.
        /// </summary>
        /// <param name="elementsToAdd">The elements to add.</param>
        /// <param name="failedToAdd">The elements that failed to add.</param>
        /// <param name="saveOnSuccess">Whether to save the storage on success.</param>
        /// <returns>True if all elements were added successfully, otherwise false.</returns>
        public bool TryAdd(IEnumerable<Pair<T, int>> elementsToAdd, out List<Pair<T, int>> failedToAdd,
            bool saveOnSuccess = true)
        {
            failedToAdd = new List<Pair<T, int>>();
            foreach (var pair in elementsToAdd)
            {
                if (!TryAdd(pair.Key, pair.Value, out _, false)) failedToAdd.Add(pair);
            }

            if (saveOnSuccess && failedToAdd.Count == 0) SaveData(_elements.ToPairArray());
            return failedToAdd.Count == 0;
        }

        /// <summary>
        /// Tries to remove an element from the storage.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        /// <param name="amount">The amount to remove.</param>
        /// <param name="removed">The amount actually removed.</param>
        /// <param name="saveOnSuccess">Whether to save the storage on success.</param>
        /// <returns>True if the element was removed successfully, otherwise false.</returns>
        public bool TryRemove(T element, int amount, out int removed, bool saveOnSuccess = true)
        {
            removed = 0;
            if (_elements.TryGetValue(element, out int currentAmount) && currentAmount >= amount)
            {
                _elements[element] = currentAmount - amount;
                removed = amount;
                OnItemChanged?.Invoke(element, currentAmount, currentAmount - amount);

                if (_elements[element] == 0) _elements.Remove(element);
                if (saveOnSuccess) SaveData(_elements.ToPairArray());
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to remove multiple elements from the storage.
        /// </summary>
        /// <param name="elementsToRemove">The elements to remove.</param>
        /// <param name="failedToRemove">The elements that failed to remove.</param>
        /// <param name="saveOnSuccess">Whether to save the storage on success.</param>
        /// <returns>True if all elements were removed successfully, otherwise false.</returns>
        public bool TryRemove(IEnumerable<Pair<T, int>> elementsToRemove, out List<Pair<T, int>> failedToRemove,
            bool saveOnSuccess = true)
        {
            failedToRemove = new List<Pair<T, int>>();
            foreach (var pair in elementsToRemove)
            {
                if (!TryRemove(pair.Key, pair.Value, out _, false)) failedToRemove.Add(pair);
            }

            if (saveOnSuccess && failedToRemove.Count == 0) SaveData(_elements.ToPairArray());
            return failedToRemove.Count == 0;
        }

        /// <summary>
        /// Checks if the storage has enough of a specific element.
        /// </summary>
        /// <param name="element">The element to check.</param>
        /// <param name="amount">The amount to check.</param>
        /// <returns>True if the storage has enough of the element, otherwise false.</returns>
        public bool HasEnough(T element, int amount)
        {
            return _elements.ContainsKey(element) && _elements[element] >= amount;
        }

        /// <summary>
        /// Checks if the storage has enough of a specific element and returns the remaining amount.
        /// </summary>
        /// <param name="element">The element to check.</param>
        /// <param name="amount">The amount to check.</param>
        /// <param name="remainingAmount">The remaining amount after the check.</param>
        /// <returns>True if the storage has enough of the element, otherwise false.</returns>
        public bool HasEnough(T element, int amount, out int remainingAmount)
        {
            if (_elements.TryGetValue(element, out int currentAmount))
            {
                remainingAmount = currentAmount - amount;
                return remainingAmount >= 0;
            }

            remainingAmount = -amount; // If the element doesn't exist, remaining is negative
            return false;
        }

        /// <summary>
        /// Checks if the storage has enough of multiple elements.
        /// </summary>
        /// <param name="elementsToCheck">The elements to check.</param>
        /// <param name="insufficientElements">The elements that are insufficient.</param>
        /// <returns>True if the storage has enough of all elements, otherwise false.</returns>
        public bool HasEnough(IEnumerable<Pair<T, int>> elementsToCheck, out List<Pair<T, int>> insufficientElements)
        {
            insufficientElements = new List<Pair<T, int>>();
            foreach (var pair in elementsToCheck)
            {
                if (!HasEnough(pair.Key, pair.Value))
                {
                    insufficientElements.Add(pair);
                }
            }

            return insufficientElements.Count == 0;
        }

        /// <summary>
        /// Clears all elements from the storage.
        /// </summary>
        public void Clear()
        {
            _elements.Clear();
            SaveData(_elements.ToPairArray());
        }

        /// <summary>
        /// Gets the current amount of a specific element in the storage.
        /// </summary>
        /// <param name="element">The element to get the amount of.</param>
        /// <returns>The current amount of the element.</returns>
        public int GetAmount(T element) => _elements.GetValueOrDefault(element, 0);

        /// <summary>
        /// Gets all elements in the storage.
        /// </summary>
        /// <returns>A dictionary of all elements and their amounts.</returns>
        public Dictionary<T, int> GetAllElements()
        {
            return new Dictionary<T, int>(_elements);
        }
    }
}