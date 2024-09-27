using System;
using Soul.ChainOfResponsibility.Runtime;
using UnityEngine;

namespace Soul.QuickPickup.Runtime
{
    [Serializable]
    public abstract class PickupHandlerBase<T> : IResponsibilityHandler<T>,IDisposable
    {
        [SerializeField] public int skipFrame = 0;
        public IResponsibilityHandler<T> Next { get; set; }

        public abstract void Handle(T responsibility);

        public virtual void HandleNext(T responsibility)
        {
            Next.Handle(responsibility);
        }

        public abstract void Process();
        public abstract void Dispose();

#if UNITY_EDITOR
        public abstract void OnDrawGizmos();
#endif
    }
}