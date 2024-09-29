using System;
using _Root.Scripts.Game.Combats.Attacks;
using _Root.Scripts.Game.Combats.Damages;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Weapons
{
    public class Bullet : MonoBehaviour, IWeaponSubComponent
    {
        [SerializeField] private Rigidbody rb;
        private Attack attackReference;

        public void Attack(Attack attack)
        {
            attackReference = attack;
            rb.linearVelocity = attack.Origin.direction * attack.Info.speed;
            App.Delay(attack.Info.lifeTime, OnTimeUp);
        }

        private void OnTimeUp()
        {
            attackReference.ReturnToPool(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                attackReference.OnAttackHit(new DamageInfo(other.gameObject, attackReference.Info.damage));
            }
            else
            {
                attackReference.OnAttackMiss(other.GetContact(0).point);
            }
        }

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }
    }
}