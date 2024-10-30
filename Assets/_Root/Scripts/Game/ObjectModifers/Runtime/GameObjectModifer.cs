using UnityEngine;

namespace _Root.Scripts.Game.ObjectModifers.Runtime
{
    public abstract class GameObjectModifer : ScriptableObject
    {
        public abstract void Modify(GameObject gameObject);

        public abstract void UnModify(GameObject gameObject);
    }
}