using System;
using UnityEngine;

namespace Soul.Reactives.Runtime
{
    [Serializable]
    public class Reactive<T>
    {
        [SerializeField] protected T value;
        public event Action<T, T> OnChange;

        public Reactive()
        {
        }

        public Reactive(T value)
        {
            this.value = value;
        }

        public virtual T Value
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