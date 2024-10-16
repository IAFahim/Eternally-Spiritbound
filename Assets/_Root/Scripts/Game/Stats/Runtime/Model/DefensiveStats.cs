﻿using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public struct DefensiveStats<T> 
    {
        public T immunity;
        public T armor;
        public LimitStat<T> shield;
        public T dodgeChance;
    }
}