using System;

namespace _Root.Scripts.Model.Parameters.Runtime
{
    [Serializable]
    public class Parameters<T> where T : struct
    {
        public Parameter<T>[] parameters;

        public bool TryGetParameter(int level, out T parameter)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].level == level)
                {
                    parameter = parameters[i].value;
                    return true;
                }
            }

            parameter = default;
            return false;
        }
    }
}