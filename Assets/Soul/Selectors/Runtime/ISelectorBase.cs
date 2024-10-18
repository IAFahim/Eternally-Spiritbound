using UnityEngine;

namespace Soul.Selectors.Runtime
{
    public interface ISelectorBase<in T>
    {
        void OnSelected(T info);
        void OnDeselected(RaycastHit lastHitInfo, T info);
        void OnReselected(T info);
    }
}