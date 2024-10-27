using UnityEngine;

namespace _Root.Scripts.Game.Consmatics.Runtime
{
    public class FlashConfigScript : ScriptableObject
    {
        public float flashDuration = 0.1f;
        [SerializeField] private Material targetFlashMaterial;
        
        public void Flash(Renderer targetRenderer)
        {
            targetRenderer.material = targetFlashMaterial;
        }
        
        public void Restore(Renderer targetRenderer, Material defaultMaterial)
        {
            targetRenderer.material = defaultMaterial;
        }
    }
}