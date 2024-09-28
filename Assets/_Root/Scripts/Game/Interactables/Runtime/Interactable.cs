using _Root.Scripts.Game.Utils.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    
    public class Interactable : MonoBehaviour
    {
        public LayerMask playerLayer;
        public bool vibrateOnRange = true;

        public void OnTriggerEnter(Collider other)
        {
            if (vibrateOnRange) Interaction.VibrateOnLayer(other.gameObject, playerLayer);
        }
    }
}