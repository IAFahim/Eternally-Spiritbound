using _Root.Scripts.Game.Inputs.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Movements.Runtime
{
    public interface IMove: IMoveInputConsumer
    {
        public Vector3 Direction { get; set; }
    }
}