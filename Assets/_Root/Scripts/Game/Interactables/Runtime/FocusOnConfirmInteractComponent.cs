using Soul.Interactables.Runtime;
using Soul.Pools.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class FocusOnConfirmInteractComponent : MonoBehaviour, IInteractable
    {
        public AssetReferenceGameObject interactableConfirmHelperAsset;
        [SerializeField] private Vector3 offset = new Vector3(0, 5, 0);
        private IInteractableConfirmHelper _interactableConfirmHelper;

        public Transform Transform => transform;

        public void OnInteractableDetected(IInteractor interactor)
        {
            Debug.Log("Detected");
            if (interactor.Focused == true)
            {
                _interactableConfirmHelper ??= interactableConfirmHelperAsset
                    .Request(transform.TransformPoint(offset), Quaternion.identity)
                    .GetComponent<IInteractableConfirmHelper>();

                _interactableConfirmHelper.Active(this);
            }
        }


        public void OnInteractableDetectionLost(IInteractor interactor)
        {
            Debug.Log("Lost");
            _interactableConfirmHelper?.Hide();
        }

        public void OnInteractionStarted(IInteractor interactor)
        {
        }

        public void OnInteractionEnded(IInteractor interactor)
        {
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.TransformPoint(offset), 0.1f);
        }
#endif
    }
}