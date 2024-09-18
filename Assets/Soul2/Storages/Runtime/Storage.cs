using System;
using System.Collections.Generic;
using Pancake;
using Pancake.Common;
using Soul2.Containers.RunTime;
using UnityEngine;

namespace Soul2.Storages.Runtime
{
    /// <summary>
    /// Abstract class representing a storage for items.
    /// </summary>
    /// <typeparam name="T">Type of the items to be stored.</typeparam>
    [Serializable]
    public abstract class Storage<T> : IStorage<T>
    {
        [Guid] public string guid;
        [SerializeField] private Pair<T, int>[] startingElements;
        public event Action<T, int, int> OnItemChanged;

        private Dictionary<T, int> _elements;

        /// <summary>
        /// Gets the unique identifier for the storage.
        /// </summary>
        public string Guid => guid;

        /// <summary>
        /// Gets the starting elements of the storage.
        /// </summary>
        public Pair<T, int>[] StartingElements => startingElements;

        /// <summary>
        /// Gets the count of different items in the storage.
        /// </summary>
        public int Count => _elements.Count;

        /// <summary>
        /// Loads the storage data.
        /// </summary>
        [ContextMenu("Load")]
        public void Load()
        {
            startingElements = Data.Load(guid, startingElements);
            _elements = new Dictionary<T, int>();
            foreach (var pair in startingElements)
            {
                if (pair.Key != null) _elements.Add(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Saves the storage data.
        /// </summary>
        [ContextMenu("Save")]
        public void Save()
        {
            startingElements = new Pair<T, int>[_elements.Count];
            int i = 0;
            foreach (var pair in _elements)
            {
                startingElements[i] = new Pair<T, int>(pair.Key, pair.Value);
                i++;
            }

            Data.Save(guid, startingElements);
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

            if (saveOnSuccess) Save();
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
                if (!TryAdd(pair.Key, pair.Value, out _, false))
                {
                    failedToAdd.Add(pair);
                }
            }

            if (saveOnSuccess && failedToAdd.Count == 0) Save();
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
                if (saveOnSuccess) Save();
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
                if (!TryRemove(pair.Key, pair.Value, out _, false))
                {
                    failedToRemove.Add(pair);
                }
            }

            if (saveOnSuccess && failedToRemove.Count == 0) Save();
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
            Save();
        }

        /// <summary>
        /// Gets the current amount of a specific element in the storage.
        /// </summary>
        /// <param name="element">The element to get the amount of.</param>
        /// <returns>The current amount of the element.</returns>
        public int GetAmount(T element)
        {
            return _elements.TryGetValue(element, out int amount) ? amount : 0;
        }

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