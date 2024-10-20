using System;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public interface IDropStrategy
    {
        public float DropRange { get; }
        public void OnDrop(GameObject user, Vector3 position, int amount, Action<GameObject> onDropped);
    }
}