using _Root.Scripts.Game.Inputs.Runtime;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.MainProviders.Runtime;
using _Root.Scripts.Presentation.Tweens.Runtime;
using Soul.OverlapSugar.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Presentation.Platforms.Runtime
{
    public class BoatEnter : MonoBehaviour, IInteractable
    {
        public GameObject boat;
        public OverlapCheckedNonAlloc overlapCheckedNonAlloc;

        [FormerlySerializedAs("mainProviderScriptable")]
        public MainStackScriptable mainStackScriptable;

        public bool CanInteract(IInteractor initiator) => true;


        private void Awake()
        {
            overlapCheckedNonAlloc.Initialize();
        }

        public void OnInteractHoverEnter(IInteractor initiator)
        {
            if (initiator.GameObject == mainStackScriptable.mainObject)
            {
                if (overlapCheckedNonAlloc.Perform() > 0)
                {
                    boat = overlapCheckedNonAlloc.Colliders[0].gameObject;
                    boat.GetComponent<IMove>().IsInputEnabled = true;
                    CustomTween.Jump(initiator.GameObject.transform, 1, boat.transform.position, 1, ProvideToMainObject).Forget();
                }
            }
        }

        private void ProvideToMainObject()
        {
            mainStackScriptable.Push(boat, true);
        }

        public void OnInteractStart(IInteractor initiator)
        {
        }

        public void OnInteractEnd(IInteractor initiator)
        {
        }

        public void OnHoverExit(IInteractor initiator)
        {
        }

        private void OnDrawGizmosSelected()
        {
            overlapCheckedNonAlloc.DrawGizmos(Color.red, Color.green);
        }
    }
}