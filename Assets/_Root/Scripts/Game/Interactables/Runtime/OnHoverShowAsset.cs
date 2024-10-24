// using Pancake;
// using UnityEngine;
// using UnityEngine.AddressableAssets;
//
// namespace _Root.Scripts.Game.Interactables.Runtime
// {
//     public class OnHoverShowAsset: MonoBehaviour, IInteractable
//     {
//         [SerializeField] private Optional<AssetReferenceGameObject> confirmAsset;
//         [SerializeField] private bool isMain;
//
//         private IInteractableConfirmHelper _interactableConfirmHelper;
//         [SerializeField] private Vector3 spawnOffset;
//
//         public GameObject GameObject => gameObject;
//         
//         public void OnHoverStarted(IInteractor initiator)
//         {
//             if(initiator.OnInteractionStarted()
//         }
//
//         public void OnHoverEnded(IInteractor initiator)
//         {
//             throw new System.NotImplementedException();
//         }
//
//         public void OnInteractionStarted(IInteractor initiator)
//         {
//             throw new System.NotImplementedException();
//         }
//
//         public void OnInteractionEnded(IInteractor initiator)
//         {
//             throw new System.NotImplementedException();
//         }
//
//
// #if UNITY_EDITOR
//         private void OnDrawGizmosSelected()
//         {
//             Gizmos.color = Color.yellow;
//             Gizmos.DrawSphere(transform.TransformPoint(spawnOffset), .5f);
//         }
//
// #endif
//     }
// }