using UnityEngine;

namespace _Root.Scripts.Game.FocusProvider.Runtime
{
    public interface IFocusConsumer
    {
        public void SetFocus(FocusReferences focusReferences);
        public void OnFocusLost(GameObject targetGameObject);
    }
}