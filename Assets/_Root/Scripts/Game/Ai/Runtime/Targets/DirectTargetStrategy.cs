using System;
using _Root.Scripts.Game.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    [CreateAssetMenu(menuName = "Scriptable/TargetingStrategy/Direct")]
    public class DirectTargetStrategy : TargetStrategy
    {
        [SerializeField] FocusManagerScript focusManager;
        private bool _targetFound;
        private ITargetable _currentTargetable;

        public override void Start()
        {
            if (IsActive) return;
            _targetFound = false;
            IsActive = true;
            focusManager.OnMainChanged += FocusManagerOnMainChanged;
            if (focusManager.mainObject != null) FocusManagerOnMainChanged(focusManager.mainObject);
        }

        public override void Register(ITargeter targeter, Action<ITargetable> onTargetFound,
            Action<ITargetable, bool> onTargetLost)
        {
            FoundCallBack += onTargetFound;
            LostCallback += onTargetLost;
            if (_targetFound) onTargetFound?.Invoke(_currentTargetable);
        }

        public override void UnRegister(ITargeter targeter, Action<ITargetable> onTargetFound,
            Action<ITargetable, bool> onTargetLost)
        {
            FoundCallBack -= onTargetFound;
            LostCallback -= onTargetLost;
        }


        public override void Set(ITargetable targetable)
        {
            if (_targetFound) Remove(_currentTargetable, false);
            _currentTargetable = targetable;
            _targetFound = true;
            FoundCallBack?.Invoke(targetable);
        }

        public override void Remove(ITargetable targetable, bool onDisable)
        {
            if (_targetFound)
            {
                LostCallback?.Invoke(targetable, onDisable);
                _targetFound = false;
                _currentTargetable = null;
            }
        }
        
        public override void Stop()
        {
            focusManager.OnMainChanged -= FocusManagerOnMainChanged;
            Remove(_currentTargetable, false);
            FoundCallBack = null;
            IsActive = false;
            _targetFound = false;
            LostCallback = null;
        }

        private void FocusManagerOnMainChanged(GameObject mainGameObject)
        {
            if (mainGameObject.TryGetComponent<ITargetable>(out var targetable)) Set(targetable);
        }
    }
}