using System;

namespace Soul2.Storages.Runtime
{
    [Serializable]
    public abstract class IntStorage<TElement> : StorageBase<TElement, int>
    { 
        public override int Add(int a, int b) => a + b;
        public override int Remove(int a, int b) => a - b;
        public override int Compare(int a, int b) => a.CompareTo(b);
    }
}