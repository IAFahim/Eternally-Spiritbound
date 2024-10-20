using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime
{
    public class TargetManager : MonoBehaviour
    {
        [SerializeField] private TargetingEntityScriptable targetingEntityScriptable;
        
        private void OnDisable()
        {
            targetingEntityScriptable.ClearTargets();
        }
    }
}