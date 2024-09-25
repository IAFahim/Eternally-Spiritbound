using System;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Attacks
{
    [Serializable]
    public struct AttackInfo
    {
        public GameObject attacker;
        public EAttackType attackType;
        public Vector3 position;
        public float damage;
    }
}