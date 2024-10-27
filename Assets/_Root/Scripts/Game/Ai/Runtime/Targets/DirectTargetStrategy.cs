using _Root.Scripts.Game.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    [CreateAssetMenu(menuName = "Scriptable/TargetingStrategy/Direct")]
    public class DirectTargetStrategy : TargetingStrategy
    {
        [SerializeField] FocusManagerScript focusManager;
        private ITargetable _currentTargetable;

        public override void Initialize()
        {
            _currentTargetable = focusManager.mainObject.GetComponent<ITargetable>();
            TargetFound(_currentTargetable);
        }

        public override bool TryGetTarget(out ITargetable targetable)
        {
            targetable = _currentTargetable;
            return true;
        }

        public override void CleanUp()
        {
            TargetLost(_currentTargetable);
            _currentTargetable = null;
        }
    }
}