using UnityEngine;

namespace Soul.Selectors.Runtime
{
    public interface ISelectionCallback
    {
        void OnSelected(RaycastHit hit);
        void OnDeselected(RaycastHit hit);
        void OnReselected(RaycastHit hit);
    }
}