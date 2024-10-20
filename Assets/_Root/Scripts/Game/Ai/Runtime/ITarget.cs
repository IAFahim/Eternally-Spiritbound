using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime
{
    public interface ITarget
    {
        Optional<GameObject> Target { get; }
    }
}