using _Root.Scripts.Game.Interactables.Runtime;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    [CreateAssetMenu(menuName = "Scriptable/TargetingStrategy/Direct")]
    public class DirectTargetStrategy : TargetingStrategy
    {
        [SerializeField] FocusManagerScript focusManager;
        private Optional<ITargetable> _currentTargetable;

        public override void Activate()
        {
            if (!focusManager.mainObject.TryGetComponent<ITargetable>(out var targetable)) return;
            _currentTargetable = new Optional<ITargetable>(targetable);
            TargetFound(targetable);
        }

        public override bool TryGetTarget(out ITargetable targetable)
        {
            targetable = _currentTargetable.Value;
            return _currentTargetable.Enabled;
        }

        public override void Deactivate()
        {
            if (_currentTargetable.Enabled)
            {
                TargetLost(_currentTargetable.Value, false);
                _currentTargetable = new Optional<ITargetable>(false, null);
            }
        }
    }
}