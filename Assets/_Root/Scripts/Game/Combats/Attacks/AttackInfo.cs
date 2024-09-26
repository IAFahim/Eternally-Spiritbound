using System;
using _Root.Scripts.Game.Combats.Damages;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Attacks
{
    [Serializable]
    public class AttackInfo
    {
        public GameObject attacker;
        public EAttackType attackType;
        public Vector3 position;
        public EDamageType damageType;
        public float damage;
        public Action<DamageInfo> OnHit;
        public Action OnMiss;
    }
}