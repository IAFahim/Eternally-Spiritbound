using System;
using _Root.Scripts.Game.GameEntities.Runtime.Weapons;
using _Root.Scripts.Game.Interactables.Runtime;
using Soul.Selectors.Runtime;
using UnityEngine;

namespace _Root.Scripts.Presentation.Shop.Runtime
{
    public class GunShop : InteractableComponent, ISelectionCallback
    {
        public Weapon[] weapons;
        public override bool CanInteract(GameObject initiator) => true;

        public override void OnInteractHover(GameObject initiator)
        {
            
        }


        public override void OnInteractStart(GameObject initiator, Action onComplete)
        {
            
        }

        public override void OnInteractExit(GameObject initiator)
        {
            
        }

        public void OnSelected(RaycastHit hit)
        {
            Debug.Log("Selected");
        }

        public void OnDeselected(RaycastHit hit)
        {
            Debug.Log("Deselected");
        }

        public void OnReselected(RaycastHit hit)
        {
            Debug.Log("Reselected");
        }
    }
}