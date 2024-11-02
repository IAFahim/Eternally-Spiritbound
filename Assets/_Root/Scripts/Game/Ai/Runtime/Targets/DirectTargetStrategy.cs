using System;
using _Root.Scripts.Game.Interactables.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    [CreateAssetMenu(menuName = "Scriptable/TargetingStrategy/Direct")]
    public class DirectTargetStrategy : TargetStrategy
    {
        [SerializeField] FocusManagerScript focusManager;
        [SerializeField] private bool targetFound;
        [ShowInInspector] private ITargetable _currentTargetable;

        public override void Start()
        {
            if (isActive) return;
            targetFound = false;
            isActive = true;
            focusManager.OnMainChanged += FocusManagerOnMainChanged;
            if (focusManager.mainObject != null) FocusManagerOnMainChanged(focusManager.mainObject);
        }

        public override void Register(ITargeter targeter, Action<ITargetable> onTargetFound,
            Action<ITargetable, bool> onTargetLost)
        {
            FoundCallBack += onTargetFound;
            LostCallback += onTargetLost;
            if (targetFound) onTargetFound?.Invoke(_currentTargetable);
        }

        public override void UnRegister(ITargeter targeter, Action<ITargetable> onTargetFound,
            Action<ITargetable, bool> onTargetLost)
        {
            FoundCallBack -= onTargetFound;
            LostCallback -= onTargetLost;
        }


        public override void Set(ITargetable targetable)
        {
            if (targetFound) Remove(_currentTargetable, false);
            _currentTargetable = targetable;
            targetFound = true;
            FoundCallBack?.Invoke(targetable);
        }

        public override void Remove(ITargetable targetable, bool onDisable)
        {
            if (targetFound)
            {
                LostCallback?.Invoke(targetable, onDisable);
                targetFound = false;
                _currentTargetable = null;
            }
        }

        public override void Stop()
        {
            focusManager.OnMainChanged -= FocusManagerOnMainChanged;
            Remove(_currentTargetable, false);
            FoundCallBack = null;
            isActive = false;
            targetFound = false;
            LostCallback = null;
        }

        private void FocusManagerOnMainChanged(GameObject mainGameObject)
        {
            if (mainGameObject.TryGetComponent<ITargetable>(out var targetable)) Set(targetable);
        }
    }
}