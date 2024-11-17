using System;

namespace _Root.Scripts.Model.Parameters.Runtime
{
    [Serializable]
    public struct Parameter<T> where T : struct
    {
        public int level;
        public T value;
    }
}