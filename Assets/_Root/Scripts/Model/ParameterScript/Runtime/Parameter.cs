using System;

namespace _Root.Scripts.Model.ParameterScript.Runtime
{
    [Serializable]
    public struct Parameter<T> where T : struct
    {
        public int key;
        public T value;
    }
}