using System;
using _Root.Scripts.Game.Inputs.Runtime;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.MainProviders.Runtime;
using Soul.OverlapSugar.Runtime;
using UnityEngine;

namespace _Root.Scripts.Presentation.Platforms.Runtime
{
    public class BoatEnter : MonoBehaviour, IInteractable
    {
        public GameObject boat;
        public OverlapCheckedNonAlloc overlapCheckedNonAlloc;
        public MainProviderScriptable mainProviderScriptable;
        public bool CanInteract(IInteractor initiator) => true;

        private void Awake()
        {
            overlapCheckedNonAlloc.Initialize();
        }

        public void OnInteractHoverEnter(IInteractor initiator)
        {
            if (initiator.GameObject == mainProviderScriptable.mainObject)
            {
                if (overlapCheckedNonAlloc.Perform() > 0)
                {
                    boat = overlapCheckedNonAlloc.Colliders[0].gameObject;
                    boat.GetComponent<IMove>().IsInputEnabled = true;
                    mainProviderScriptable.ProvideTo(boat, true);
                }
            }
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