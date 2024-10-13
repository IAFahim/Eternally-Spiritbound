using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public struct AmmoStats<T> 
    {
        public T total;
        public T clipSize;
        
    }
}