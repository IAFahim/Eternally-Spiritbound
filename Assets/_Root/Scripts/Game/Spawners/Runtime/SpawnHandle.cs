using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    public class SpawnHandle : MonoBehaviour, IInitializable<Spawner>
    {
        [ShowInInspector] [ReadOnly] private Spawner _spawner;
        private void OnDisable() => _spawner.Despawn(gameObject);

        public void Init(Spawner argument)
        {
            _spawner = argument;
        }
    }
}