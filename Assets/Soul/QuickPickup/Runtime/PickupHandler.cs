using System;
using Soul.ChainOfResponsibility.Runtime;
using UnityEngine;

namespace Soul.QuickPickup.Runtime
{
    [Serializable]
    public abstract class PickupHandler<T> : IResponsibilityHandler<PickupContainer<T>>
    {
        [SerializeField] public int skipFrame = 0;

        public IResponsibilityHandler<PickupContainer<T>> Next { get; set; }

        public abstract void Handle(PickupContainer<T> responsibility);

        public virtual void HandleNext(PickupContainer<T> responsibility)
        {
            Next.Handle(responsibility);
        }

        public abstract void Process();
        public abstract void Clear();

#if UNITY_EDITOR
        public abstract void OnDrawGizmos();
#endif
    }
}