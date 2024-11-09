using System;
using UnityEngine;

namespace Soul.Reactives.Runtime
{
    [Serializable]
    public struct Reactive<T>
    {
        public T value;
        public event Action<T, T> OnChange;

        public Reactive(T value)
        {
            this.value = value;
            OnChange = null;
        }

        public T Value
        {
            get => value;
            set
            {
                T oldValue = this.value;
                this.value = value;
                OnChange?.Invoke(oldValue, this.value);
            }
        }

        public void SetValueWithoutNotify(T value)
        {
            this.value = value;
        }

        public static implicit operator T(Reactive<T> reactive)
        {
            return reactive.Value;
        }
    }
}