using _Root.Scripts.Game.Weapons.Runtime.Attacks;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime.Damages
{
    public abstract class DamageType : ScriptableObject
    {
        public StringConstant type;
        public abstract float Apply(AttackOrigin attackOrigin, Vector3 position, float strength);
    }
}