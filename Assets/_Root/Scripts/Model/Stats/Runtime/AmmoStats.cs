using System;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public class AmmoStats<T> 
    {
        public T total;
        public T clipSize;
        
    }
}