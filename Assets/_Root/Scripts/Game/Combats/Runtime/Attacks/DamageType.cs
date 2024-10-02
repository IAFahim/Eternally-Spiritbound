using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Attacks
{
    public abstract class DamageType : ScriptableObject
    {
        public StringConstant type;
        public abstract float Apply(Attack attack, Vector3 position, float strength);
    }
}