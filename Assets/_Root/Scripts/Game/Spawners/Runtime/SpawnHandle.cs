using _Root.Scripts.Game.ObjectModifers.Runtime;
using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    public class SpawnHandle : MonoBehaviour, IInitializable<GameObjectModifer>
    {
        [ShowInInspector] [ReadOnly] private GameObjectModifer _gameObjectModifer;
        private void OnDisable() => _gameObjectModifer.UnModify(gameObject);

        public void Init(GameObjectModifer argument)
        {
            _gameObjectModifer = argument;
        }

        private void Start()
        {
            _gameObjectModifer.Modify(gameObject);
        }
    }
}