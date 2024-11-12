using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Pools.Runtime
{
    public class AddressableGameObjectPoolInactive
    {
        public AddressableGameObjectPoolInactive(object key)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public AddressableGameObjectPoolInactive(AssetReferenceGameObject reference)
        {
            if (reference == null) throw new ArgumentNullException(nameof(reference));
            _key = reference.RuntimeKey;
        }

        private readonly object _key;
        private readonly Stack<GameObject> _stack = new(32);
        private bool _isDisposed;

        public int Count => _stack.Count;
        public bool IsDisposed => _isDisposed;

        public GameObject Request()
        {
            ThrowIfDisposed();
            if (!_stack.TryPop(out var obj)) return Addressables.InstantiateAsync(_key).WaitForCompletion();
            return obj;
        }

        public GameObject Request(Transform parent, bool worldPositionStays = false)
        {
            ThrowIfDisposed();

            if (!_stack.TryPop(out var obj))
                return Addressables.InstantiateAsync(_key, parent, worldPositionStays).WaitForCompletion();
            obj.transform.SetParent(parent, worldPositionStays);
            return obj;
        }

        public GameObject Request(Vector3 position, Quaternion rotation)
        {
            ThrowIfDisposed();
            if (!_stack.TryPop(out var obj))
                return Addressables.InstantiateAsync(_key, position, rotation).WaitForCompletion();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }

        public GameObject Request(Vector3 position, Quaternion rotation, Transform parent)
        {
            ThrowIfDisposed();
            if (!_stack.TryPop(out var obj))
                return Addressables.InstantiateAsync(_key, position, rotation, parent).WaitForCompletion();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.transform.SetParent(parent);
            return obj;
        }

        public void Return(GameObject gameObject)
        {
            ThrowIfDisposed();
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            gameObject.SetActive(false);
            _stack.Push(gameObject);
        }
        
        public void Clear()
        {
            ThrowIfDisposed();
            while (_stack.TryPop(out var obj)) Addressables.ReleaseInstance(obj);
        }

        public void Prewarm(int count)
        {
            ThrowIfDisposed();
            for (var i = 0; i < count; i++)
            {
                var obj = Addressables.InstantiateAsync(_key).WaitForCompletion();
                _stack.Push(obj);
                obj.SetActive(false);
            }
        }
        
        public void Dispose()
        {
            if (_isDisposed) return;
            Clear();
            _isDisposed = true;
        }
        
        private void ThrowIfDisposed()
        {
            if (_isDisposed) throw new ObjectDisposedException(GetType().Name);
        }
    }
}