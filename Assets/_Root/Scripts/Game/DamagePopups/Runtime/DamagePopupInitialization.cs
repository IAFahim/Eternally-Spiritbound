using UnityEngine;

namespace _Root.Scripts.Game.DamagePopups.Runtime
{
    public class DamagePopupInitialization : MonoBehaviour
    {
        public DamagePopup[] damagePopups;
        private void OnEnable()
        {
            foreach (var damagePopup in damagePopups)
            {
                damagePopup.Enable();
            }
        }
        
        private void OnDisable()
        {
            foreach (var damagePopup in damagePopups)
            {
                damagePopup.Disable();
            }
        }
    }
}