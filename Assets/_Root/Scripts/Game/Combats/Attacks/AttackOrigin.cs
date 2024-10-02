using System;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Attacks
{
    [Serializable]
    public class AttackOrigin
    {
        public Optional<GameObject> attacker;
        public GameObject weapon;
        public Vector3 position;
        public Vector3 direction;
        public float normalizedRange;

        public AttackOrigin(GameObject attacker, GameObject weapon, Vector3 position, Vector3 direction, float normalizedRange)
        {
            this.attacker = attacker;
            this.position = position;
            this.direction = direction;
            this.normalizedRange = normalizedRange;
        }
    }
}