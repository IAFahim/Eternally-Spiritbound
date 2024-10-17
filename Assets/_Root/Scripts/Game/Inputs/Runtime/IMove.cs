using UnityEngine;

namespace _Root.Scripts.Game.Inputs.Runtime
{
    public interface IMove: IMoveInputConsumer
    {
        public Vector3 Direction { get; set; }
    }
}