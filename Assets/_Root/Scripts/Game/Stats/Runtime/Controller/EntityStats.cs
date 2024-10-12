using System;
using _Root.Scripts.Game.Stats.Runtime.Model;
using Soul.Modifiers.Runtime;

namespace _Root.Scripts.Game.Stats.Runtime.Controller
{
    [Serializable]
    public class EntityStats : EntityStatsBase<Modifier>
    {
        public Modifier contactDamage;
    }
}