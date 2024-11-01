using _Root.Scripts.Game.Interactables.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    [CreateAssetMenu(menuName = "Scriptable/TargetingStrategy/Direct")]
    public class DirectTargetStrategy : TargetingStrategy
    {
        [SerializeField] FocusManagerScript focusManager;
        [ShowInInspector] private ITargetable _currentTargetable;

        public override void StartTargetLookup()
        {
            if (isActive || focusManager.mainObject != null &&
                !focusManager.mainObject.TryGetComponent<ITargetable>(out _currentTargetable)) return;
            focusManager.OnMainChanged += FocusManagerOnMainChanged;
            isActive = true;
            TargetFound(_currentTargetable);
        }

        private void FocusManagerOnMainChanged(GameObject mainGameobject)
        {
            if (!mainGameobject.TryGetComponent<ITargetable>(out var targetable)) return;
            TargetFound(targetable);
        }

        public override void TargetFound(ITargetable targetable)
        {
            if (_currentTargetable != null) TargetLost(_currentTargetable, false);
            _currentTargetable = targetable;
            InvokeTargetFound(targetable);
        }

        public override void TargetLost(ITargetable targetable, bool onDisable)
        {
            InvokeTargetLost(targetable, onDisable);
        }

        public override bool TryGetTarget(ITargeter _, out ITargetable targetable)
        {
            targetable = _currentTargetable;
            return targetable != null;
        }

        public override void StopTargetLookup()
        {
            if (_currentTargetable == null) return;
            TargetLost(_currentTargetable, false);
            focusManager.OnMainChanged -= FocusManagerOnMainChanged;
            _currentTargetable = null;
            isActive = false;
        }
    }
}