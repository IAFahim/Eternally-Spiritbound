using System;

namespace _Root.Scripts.Game.Combats.Attacks
{
    [Flags]
    public enum EAttackType
    {
        Melee = 0,
        Projectile = 2,
        Magic = 2,
        Area = 4
    }
}