using System;
using _Root.Scripts.Game.Combats.Runtime.Weapons;
using _Root.Scripts.Game.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Presentation.Shop.Runtime
{
    public class GunShop : InteractableComponent
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
    }
}