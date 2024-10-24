using UnityEngine;

namespace Soul.Selectors.Runtime
{
    public interface ISelector
    {
        void OnSelected(RaycastHit hit);
        void OnDeselected(RaycastHit lastHitInfo, RaycastHit hit);
        void OnReselected(RaycastHit hit);
    }
}