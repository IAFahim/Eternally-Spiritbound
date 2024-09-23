using System;
using System.Collections.Generic;
using Soul2.Datas.Runtime.Interface;
using Soul2.Serializers.Runtime;

namespace Soul2.Storages.Runtime
{
    /// <summary>
    /// Defines the contract for a storage system.
    /// </summary>
    /// <typeparam name="TElement">The type of elements stored.</typeparam>
    /// <typeparam name="TValue">The type of values associated with elements.</typeparam>
    public interface IStorageBase<TElement, TValue>: IStorageAdapter<Pair<TElement, TValue>[]>
        where TElement : notnull
        where TValue : IComparable<TValue>
    {
        /// <summary>
        /// Event triggered when an item in the storage changes.
        /// </summary>
        event Action<TElement, TValue, TValue> OnItemChanged;

        /// <summary>
        /// Gets the number of elements in the storage.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the default data for the storage.
        /// </summary>
        Pair<TElement, TValue>[] DefaultData { get; }
        

        /// <summary>
        /// Tries to add an amount to an element in the storage.
        /// </summary>
        bool TryAdd(TElement element, TValue amount, out TValue added, bool saveOnSuccess = false);

        /// <summary>
        /// Tries to add multiple elements to the storage.
        /// </summary>
        bool TryAdd(IEnumerable<Pair<TElement, TValue>> elementsToAdd, out List<Pair<TElement, TValue>> failedToAdd, bool saveOnSuccess = false);

        /// <summary>
        /// Tries to remove an amount from an element in the storage.
        /// </summary>
        bool TryRemove(TElement element, TValue amount, out TValue removed, bool saveOnSuccess = false);

        /// <summary>
        /// Tries to remove multiple elements from the storage.
        /// </summary>
        bool TryRemove(IEnumerable<Pair<TElement, TValue>> elementsToRemove, out List<Pair<TElement, TValue>> failedToRemove, bool saveOnSuccess = false);

        /// <summary>
        /// Removes all of a specific element from the storage.
        /// </summary>
        bool RemoveAll(TElement elementToRemove, out TValue removed, bool saveOnSuccess = false);

        /// <summary>
        /// Checks if the storage has enough of a specific element.
        /// </summary>
        bool HasEnough(TElement element, TValue amount);

        /// <summary>
        /// Checks if the storage has enough of a specific element and returns the remaining amount.
        /// </summary>
        bool HasEnough(TElement element, TValue amount, out TValue remainingAmount);

        /// <summary>
        /// Checks if the storage has enough of multiple elements.
        /// </summary>
        bool HasEnough(IEnumerable<Pair<TElement, TValue>> elementsToCheck, out List<Pair<TElement, TValue>> insufficientElements);

        /// <summary>
        /// Clears all elements from the storage.
        /// </summary>
        void Clear(bool save = false);

        /// <summary>
        /// Adds two values together.
        /// </summary>
        public TValue Sum(TValue a, TValue b);

        /// <summary>
        /// Removes one value from another.
        /// </summary>
        public TValue Sub(TValue a, TValue b);

        /// <summary>
        /// Compares two values.
        /// </summary>
        int Compare(TValue a, TValue b);

        /// <summary>
        /// Gets the current amount of an element.
        /// </summary>
        TValue GetAmount(TElement element);

        /// <summary>
        /// Sets the amount of an element.
        /// </summary>
        bool TrySetAmount(TElement element, TValue amount, bool saveOnSuccess = false);

        /// <summary>
        /// Tries to get the amount of an element.
        /// </summary>
        bool TryGetAmount(TElement element, out TValue amount);

        /// <summary>
        /// Checks if the storage contains a specific element.
        /// </summary>
        bool ContainsElement(TElement element);

        /// <summary>
        /// Gets an IEnumerable of all elements in the storage.
        /// </summary>
        Dictionary<TElement, TValue> GetAllElements();

        /// <summary>
        /// Gets an IEnumerable of all element-value pairs in the storage.
        /// </summary>
        IEnumerable<KeyValuePair<TElement, TValue>> GetAllElementValuePairs();

        /// <summary>
        /// Adds multiple elements to the storage without saving.
        /// </summary>
        void AddRange(IEnumerable<KeyValuePair<TElement, TValue>> elementsToAdd);

        /// <summary>
        /// Removes multiple elements from the storage without saving.
        /// </summary>
        void RemoveRange(IEnumerable<TElement> elementsToRemove);

        /// <summary>
        /// Clears the storage and sets it to the default data.
        /// </summary>
        void ResetToDefault(bool save = false);

        /// <summary>
        /// Gets the total sum of all values in the storage.
        /// </summary>
        TValue GetTotalSum();

        /// <summary>
        /// Creates a deep copy of the current storage state.
        /// </summary>
        Dictionary<TElement, TValue> CreateDeepCopy();

        /// <summary>
        /// Applies a transformation to all values in the storage.
        /// </summary>
        void TransformAllValues(Func<TValue, TValue> transformFunc, bool save = false);

        /// <summary>
        /// Merges another storage into this one, combining values for matching elements.
        /// </summary>
        void MergeFrom(IStorageBase<TElement, TValue> otherStorage, bool save = false);
    }
}