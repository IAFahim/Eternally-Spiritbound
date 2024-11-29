using _Root.Scripts.Game.Ai.Runtime.BreadCrumbs;
using _Root.Scripts.Game.GameEntities.Runtime;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.Interactables.Runtime.Focus;
using UnityEngine;

namespace _Root.Scripts.Game.Falls.Runtime
{
    public class FallKillOtherRestoreMain : MonoBehaviour
    {
        public FocusManagerScript focusManagerScript;

        private async void OnTriggerEnter(Collider other)
        {
            var otherGameObject = other.gameObject;

            if (otherGameObject != focusManagerScript.mainObject)
            {
                if (otherGameObject.TryGetComponent(out GameEntity gameEntity))
                {
                    gameEntity.Kill();
                }

                return;
            }

            if (otherGameObject.TryGetComponent(out BreadCrumb breadCrumb))
            {
                otherGameObject.SetActive(false);
                otherGameObject.transform.position = breadCrumb.GetFarthestBreadCrumbFromCurrent();
                otherGameObject.SetActive(true);
            }
        }
        
    }
}