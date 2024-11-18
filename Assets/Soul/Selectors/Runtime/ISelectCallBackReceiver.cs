using UnityEngine;

namespace Soul.Selectors.Runtime
{
    public interface ISelectCallBackReceiver
    {
        void OnSelected(RaycastHit hit);

        void OnUpdateDrag(RaycastHit hitRef, bool isInside, Vector3 worldPosition,
            Vector3 worldPositionDelta); // new method

        void OnDragEnd(RaycastHit hitRef, bool isInside, Vector3 worldPosition); // new method
        void OnDeselected(RaycastHit lastHitInfo, RaycastHit hit);
        void OnReselected(RaycastHit hit);
    }
}