using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace Soul.Storages.Runtime
{
    /// <summary>
    /// Base class for storage implementations.
    /// </summary>
    /// <typeparam name="TElement">The type of elements stored.</typeparam>
    /// <typeparam name="TValue">The type of values associated with elements.</typeparam>
    [Serializable]
    public abstract class StorageBase<TElement, TValue> : IStorageBase<TElement, TValue>
        where TElement : notnull
        where TValue : IComparable<TValue>
    {
        [SerializeField, Tooltip("Starting elements when you load for the first time, useful for scriptable objects")]
        protected Pair<TElement, TValue>[] defaultData = Array.Empty<Pair<TElement, TValue>>();

        [SerializeField] protected Dictionary<TElement, TValue> elements = new();

        /// <summary>
        /// Initializes the storage with either loaded data or default data.
        /// </summary>
        /// <param name="guid">The GUID of the data to load.</param>
        /// <param name="load">If true, loads data using the GUID; otherwise, sets elements to default data.</param>
        public void InitializeStorage(string guid = "", bool load = false)
        {
            if (load) LoadData(guid);
            else SetElements(defaultData);
        }

        /// <summary>
        /// Event triggered when an item in the storage changes.
        /// </summary>
        public event Action<TElement, TValue, TValue> OnItemChanged;

        /// <summary>
        /// Gets all elements in the storage.
        /// </summary>
        /// <returns>The underlying dictionary of all elements and their amounts.</returns>
        public Dictionary<TElement, TValue> Elements => elements;

        /// <summary>
        /// Gets the number of elements in the storage.
        /// </summary>
        public int Count => elements.Count;

        /// <summary>
        /// Gets the default data for the storage.
        /// </summary>
        public Pair<TElement, TValue>[] DefaultData => defaultData;

        /// <summary>
        /// Gets or sets the GUID for the storage.
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Gets the storage key.
        /// </summary>
        public abstract string StorageKey { get; }

        /// <summary>
        /// Sets the elements in the storage.
        /// </summary>
        /// <param name="loadedData">The data to load into the storage.</param>
        protected virtual void SetElements(Pair<TElement, TValue>[] loadedData)
        {
            elements.Clear();
            foreach (var pair in loadedData)
            {
                if (pair.Key == null) return;

                if (elements.TryGetValue(pair.Key, out var currentValue))
                    elements[pair.Key] = Sum(currentValue, pair.Value);
                else
                    elements.Add(pair.Key, pair.Value);
            }
        }

        public virtual bool CanAdd(TElement element, TValue amount, out TValue currentAmount)
        {
            elements.TryGetValue(element, out currentAmount);
            return true;
        }

        /// <summary>
        /// Tries to add an amount to an element in the storage.
        /// </summary>
        /// <param name="element">The element to add to.</param>
        /// <param name="amount">The amount to add.</param>
        /// <param name="added">The amount actually added.</param>
        /// <param name="saveOnSuccess">Whether to save the data after a successful addition.</param>
        /// <returns>True if the addition was successful, false otherwise.</returns>
        public virtual bool TryAdd(TElement element, TValue amount, out TValue added, bool saveOnSuccess = false)
        {
            if (!CanAdd(element, amount, out var currentAmount))
            {
                added = default;
                return false;
            }

            var newAmount = Sum(currentAmount, amount);
            elements[element] = newAmount;
            added = amount;
            OnItemChanged?.Invoke(element, currentAmount, newAmount);

            if (saveOnSuccess)
                SaveData();
            return true;
        }


        /// <summary>
        /// Tries to add multiple elements to the storage.
        /// </summary>
        /// <param name="elementsToAdd">The elements and amounts to add.</param>
        /// <param name="failedToAdd">A list of elements that failed to be added.</param>
        /// <param name="saveOnSuccess">Whether to save the data after a successful addition.</param>
        /// <returns>True if all elements were added successfully, false otherwise.</returns>
        public virtual bool TryAdd(IEnumerable<Pair<TElement, TValue>> elementsToAdd,
            out List<Pair<TElement, TValue>> failedToAdd, bool saveOnSuccess = false)
        {
            failedToAdd = new List<Pair<TElement, TValue>>();
            foreach (var pair in elementsToAdd)
            {
                if (!TryAdd(pair.Key, pair.Value, out _, false))
                    failedToAdd.Add(pair);
            }

            if (saveOnSuccess && failedToAdd.Count == 0)
                SaveData();
            return failedToAdd.Count == 0;
        }

        public virtual bool CanRemove(TElement element, TValue amount, out TValue currentAmount)
        {
            if (elements.TryGetValue(element, out currentAmount)) return Compare(currentAmount, amount) >= 0;
            return false; // Or throw an exception, depending on your requirements
        }

        /// <summary>
        /// Tries to remove an amount from an element in the storage.
        /// </summary>
        /// <param name="element">The element to remove from.</param>
        /// <param name="amount">The amount to remove.</param>
        /// <param name="removed">The amount actually removed.</param>
        /// <param name="saveOnSuccess">Whether to save the data after a successful removal.</param>
        /// <returns>True if the removal was successful, false otherwise.</returns>
        public virtual bool TryRemove(TElement element, TValue amount, out TValue removed, bool saveOnSuccess = false)
        {
            removed = default;
            if (CanRemove(element, amount, out TValue currentAmount))
            {
                var newAmount = Sub(currentAmount, amount);
                elements[element] = newAmount;
                removed = amount;
                OnItemChanged?.Invoke(element, currentAmount, newAmount);

                if (Compare(newAmount, default) == 0)
                    elements.Remove(element);
                if (saveOnSuccess)
                    SaveData();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to remove multiple elements from the storage.
        /// </summary>
        /// <param name="elementsToRemove">The elements and amounts to remove.</param>
        /// <param name="failedToRemove">A list of elements that failed to be removed.</param>
        /// <param name="saveOnSuccess">Whether to save the data after a successful removal.</param>
        /// <returns>True if all elements were removed successfully, false otherwise.</returns>
        public virtual bool TryRemove(IEnumerable<Pair<TElement, TValue>> elementsToRemove,
            out List<Pair<TElement, TValue>> failedToRemove, bool saveOnSuccess = false)
        {
            failedToRemove = new List<Pair<TElement, TValue>>();
            foreach (var pair in elementsToRemove)
            {
                if (!TryRemove(pair.Key, pair.Value, out _, false))
                    failedToRemove.Add(pair);
            }

            if (saveOnSuccess && failedToRemove.Count == 0)
                SaveData();
            return failedToRemove.Count == 0;
        }

        /// <summary>
        /// Removes all of a specific element from the storage.
        /// </summary>
        /// <param name="elementToRemove">The element to remove.</param>
        /// <param name="removed">The amount removed.</param>
        /// <param name="saveOnSuccess">Whether to save the data after a successful removal.</param>
        /// <returns>True if the element was removed, false otherwise.</returns>
        public bool RemoveAll(TElement elementToRemove, out TValue removed, bool saveOnSuccess = false)
        {
            if (elements.TryGetValue(elementToRemove, out TValue currentAmount))
            {
                removed = currentAmount;
                elements.Remove(elementToRemove);
                OnItemChanged?.Invoke(elementToRemove, currentAmount, default);
                if (saveOnSuccess)
                    SaveData();
                return true;
            }

            removed = default;
            return false;
        }


        /// <summary>
        /// Checks if the storage has enough of a specific element.
        /// </summary>
        /// <param name="element">The element to check.</param>
        /// <param name="amount">The amount to check for.</param>
        /// <returns>True if the storage has enough of the element, false otherwise.</returns>
        public virtual bool HasEnough(TElement element, TValue amount)
        {
            return elements.TryGetValue(element, out var currentAmount) && Compare(currentAmount, amount) >= 0;
        }

        /// <summary>
        /// Checks if the storage has enough of a specific element and returns the remaining amount.
        /// </summary>
        /// <param name="element">The element to check.</param>
        /// <param name="amount">The amount to check for.</param>
        /// <param name="remainingAmount">The remaining amount after the hypothetical removal.</param>
        /// <returns>True if the storage has enough of the element, false otherwise.</returns>
        public virtual bool HasEnough(TElement element, TValue amount, out TValue remainingAmount)
        {
            if (elements.TryGetValue(element, out TValue currentAmount))
            {
                remainingAmount = Sub(currentAmount, amount);
                return Compare(remainingAmount, default) >= 0;
            }

            remainingAmount = default;
            return false;
        }

        /// <summary>
        /// Checks if the storage has enough of multiple elements.
        /// </summary>
        /// <param name="elementsToCheck">The elements and amounts to check for.</param>
        /// <param name="insufficientElements">A list of elements that are insufficient.</param>
        /// <returns>True if the storage has enough of all elements, false otherwise.</returns>
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


        /// <summary>
        /// Clears all elements from the storage.
        /// </summary>
        /// <param name="save">Whether to save the data after clearing.</param>
        public virtual void Clear(bool save = false)
        {
            elements.Clear();
            if (save)
                SaveData();
        }


        /// <summary>
        /// Loads data into the storage.
        /// </summary>
        /// <param name="guid">The GUID of the data to load.</param>
        public abstract void LoadData(string guid);


        public void GetDefaultData(out Pair<TElement, TValue>[] dataDefault) => dataDefault = this.defaultData;

        public void GetData(out Pair<TElement, TValue>[] dataCurrent) => dataCurrent = GetArray();

        /// <summary>
        /// Sets the data in the storage.
        /// </summary>
        /// <param name="dataNew">The data to set.</param>
        public virtual void SetData(Pair<TElement, TValue>[] dataNew)
        {
            SetElements(dataNew);
        }

        /// <summary>
        /// Saves the current state of the storage.
        /// </summary>
        /// <param name="data">The data to save.</param>
        public abstract void SaveData(Pair<TElement, TValue>[] data);

        /// <summary>
        /// Deletes the storage key.
        /// </summary>
        public abstract void ClearStorage();

        /// <summary>
        /// Saves the current state of the storage.
        /// </summary>
        public virtual void SaveData()
        {
            var data = GetArray();
            SaveData(data);
        }


        /// <summary>
        /// Adds two values together.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <returns>The sum of the two values.</returns>
        public abstract TValue Sum(TValue a, TValue b);

        /// <summary>
        /// Removes one value from another.
        /// </summary>
        /// <param name="a">The value to remove from.</param>
        /// <param name="b">The value to remove.</param>
        /// <returns>The result of the removal.</returns>
        public abstract TValue Sub(TValue a, TValue b);

        /// <summary>
        /// Compares two values.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <returns>A negative number if an is less than b, 0 if they are equal, and a positive number if an is greater than b.</returns>
        public abstract int Compare(TValue a, TValue b);

        /// <summary>
        /// Gets the current amount of an element.
        /// </summary>
        /// <param name="element">The element to get the amount for.</param>
        /// <returns>The current amount of the element, or the default value if the element doesn't exist.</returns>
        public virtual TValue GetAmount(TElement element)
        {
            return elements.GetValueOrDefault(element);
        }


        /// <summary>
        /// Tries to get the amount of an element.
        /// <param name="element">The element to get the amount for.</param>
        /// <param name="amount">The amount of the element.</param>
        /// <returns>True if the element exists in the storage, false otherwise.</returns>
        /// </summary>
        public virtual bool TryGetAmount(TElement element, out TValue amount)
        {
            return elements.TryGetValue(element, out amount);
        }

        /// <summary>
        /// Sets the amount of an element.
        /// </summary>
        /// <param name="element">The element to set the amount for.</param>
        /// <param name="amount">The amount to set.</param>
        /// <param name="saveOnSuccess">Whether to save the data after setting the amount.</param>
        /// <returns>True if the amount was set successfully, false otherwise.</returns>
        public virtual bool TrySetAmount(TElement element, TValue amount, bool saveOnSuccess = false)
        {
            var oldAmount = GetAmount(element);
            elements[element] = amount;
            OnItemChanged?.Invoke(element, oldAmount, amount);

            if (saveOnSuccess)
                SaveData();

            return true;
        }


        /// <summary>
        /// Checks if the storage contains a specific element.
        /// </summary>
        /// <param name="element">The element to check for.</param>
        /// <returns>True if the element exists in the storage, false otherwise.</returns>
        public virtual bool ContainsElement(TElement element)
        {
            return elements.ContainsKey(element);
        }

        public Pair<TElement, TValue>[] GetArray()
        {
            var data = new Pair<TElement, TValue>[elements.Count];
            var index = 0;
            foreach (var kvp in elements)
            {
                data[index] = new Pair<TElement, TValue>(kvp.Key, kvp.Value);
                index++;
            }

            return data;
        }

        public KeyValuePair<TElement, TValue>[] GetArrayKeyValuePair()
        {
            var data = new KeyValuePair<TElement, TValue>[elements.Count];
            var index = 0;
            foreach (var kvp in elements)
            {
                data[index] = kvp;
                index++;
            }

            return data;
        }


        /// <summary>
        /// Gets an IEnumerable of all element-value pairs in the storage.
        /// </summary>
        /// <returns>An IEnumerable of all element-value pairs.</returns>
        public virtual IEnumerable<KeyValuePair<TElement, TValue>> GetIEnumerable()
        {
            return elements;
        }

        /// <summary>
        /// Adds multiple elements to the storage without saving.
        /// </summary>
        /// <param name="elementsToAdd">The elements and amounts to add.</param>
        public virtual void AddRange(IEnumerable<KeyValuePair<TElement, TValue>> elementsToAdd)
        {
            foreach (var kvp in elementsToAdd)
            {
                TryAdd(kvp.Key, kvp.Value, out _, false);
            }
        }

        /// <summary>
        /// Removes multiple elements from the storage without saving.
        /// </summary>
        /// <param name="elementsToRemove">The elements to remove.</param>
        public virtual void RemoveRange(IEnumerable<TElement> elementsToRemove)
        {
            foreach (var element in elementsToRemove)
            {
                RemoveAll(element, out _, false);
            }
        }

        /// <summary>
        /// Clears the storage and sets it to the default data.
        /// </summary>
        /// <param name="save">Whether to save after resetting to default.</param>
        public virtual void ResetToDefault(bool save = false)
        {
            Clear(false);
            SetElements(defaultData);
            if (save)
                SaveData();
        }

        /// <summary>
        /// Gets the total sum of all values in the storage.
        /// </summary>
        /// <returns>The total sum of all values.</returns>
        public virtual TValue GetTotalSum()
        {
            return elements.Aggregate(default(TValue), (current, kvp) => Sum(current, kvp.Value));
        }

        /// <summary>
        /// Creates a deep copy of the current storage state.
        /// </summary>
        /// <returns>A new Dictionary representing the current state.</returns>
        public virtual Dictionary<TElement, TValue> CreateDeepCopy()
        {
            return new Dictionary<TElement, TValue>(elements);
        }

        /// <summary>
        /// Applies a transformation to all values in the storage.
        /// </summary>
        /// <param name="transformFunc">The function to apply to each value.</param>
        /// <param name="save">Whether to save after the transformation.</param>
        public virtual void TransformAllValues(Func<TValue, TValue> transformFunc, bool save = false)
        {
            foreach (var key in elements.Keys.ToList())
            {
                var oldValue = elements[key];
                var newValue = transformFunc(oldValue);
                elements[key] = newValue;
                OnItemChanged?.Invoke(key, oldValue, newValue);
            }

            if (save) SaveData();
        }

        /// <summary>
        /// Merges another storage into this one, combining values for matching elements.
        /// </summary>
        /// <param name="otherStorage">The other storage to merge from.</param>
        /// <param name="save">Whether to save after merging.</param>
        public virtual void MergeFrom(IStorageBase<TElement, TValue> otherStorage, bool save = false)
        {
            foreach (var kvp in otherStorage.GetIEnumerable())
            {
                TryAdd(kvp.Key, kvp.Value, out _, false);
            }

            if (save) SaveData();
        }

        public IEnumerator<KeyValuePair<TElement, TValue>> GetEnumerator() => elements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool ContainsKey(TElement key)
        {
            return elements.ContainsKey(key);
        }

        public bool TryGetValue(TElement key, out TValue value)
        {
            return elements.TryGetValue(key, out value);
        }

        public TValue this[TElement key] => throw new NotImplementedException();

        public IEnumerable<TElement> Keys => elements.Keys;
        public IEnumerable<TValue> Values => elements.Values;
    }
}