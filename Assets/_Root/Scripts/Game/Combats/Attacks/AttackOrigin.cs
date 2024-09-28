using System;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Attacks
{
    [Serializable]
    public class AttackOrigin
    {
        public GameObject attacker;
        public GameObject weapon;
        public Vector3 position;
        public Vector3 direction;
        public LayerMask layerMask;
        public float normalizedRange;

        public AttackOrigin(GameObject attacker, GameObject weapon, Vector3 position, Vector3 direction,
            LayerMask layerMask, float normalizedRange)
        {
            this.attacker = attacker;
            this.position = position;
            this.direction = direction;
            this.layerMask = layerMask;
            this.normalizedRange = normalizedRange;
        }
    }
}