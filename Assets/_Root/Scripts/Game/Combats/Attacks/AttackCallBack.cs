using System;
using _Root.Scripts.Game.Combats.Damages;

namespace _Root.Scripts.Game.Combats.Attacks
{
    public struct AttackCallBack
    {
        public event Action<DamageInfo> OnHitEvent;
        public event Action OnMissEvent;

        private void OnHit(DamageInfo obj) => OnHitEvent?.Invoke(obj);

        private void OnMiss() => OnMissEvent?.Invoke();
    }
}