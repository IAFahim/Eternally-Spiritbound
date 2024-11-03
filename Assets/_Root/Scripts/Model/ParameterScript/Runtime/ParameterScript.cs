using UnityEngine;

namespace _Root.Scripts.Model.ParameterScript.Runtime
{
    public abstract class ParameterScript<T> : ScriptableObject where T : struct
    {
        public T value;
    }
}