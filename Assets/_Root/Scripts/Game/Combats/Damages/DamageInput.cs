using System;

namespace _Root.Scripts.Game.Combats.Damages
{
    [Serializable]
    public struct DamageInput
    {
        public EDamageType damageType;
        public float damage;
    }
}