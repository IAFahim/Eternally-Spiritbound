using _Root.Scripts.Game.GameEntities.Runtime.Attacks;
using _Root.Scripts.Game.GameEntities.Runtime.Damages;
using Pancake.Common;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public class BulletComponent : MonoBehaviour, IInitializable<Attack>
    {
        private Attack _attackOriginReference;
        public void Init(Attack firstArgument)
        {
            _attackOriginReference = firstArgument;
            App.Delay(_attackOriginReference.Info.lifeTime, OnTimeUp);
        }
        
        private void OnTimeUp()
        {
            _attackOriginReference.ReturnToPool(gameObject);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            _attackOriginReference.OnAttackHit(new DamageInfo(other.gameObject, _attackOriginReference.Info.damage));
        }
    }
}