using UnityEngine;

namespace _Root.Scripts.Game.Uitls
{
    public static class Interaction
    {
        public static void VibrateOnLayer(GameObject otherGameObject, LayerMask layerMask)
        {
            if (layerMask == (layerMask | (1 << otherGameObject.layer))) Vibration.Vibrate();
        }
    }
}