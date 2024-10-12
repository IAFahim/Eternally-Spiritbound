using _Root.Scripts.Game.Guid;
using _Root.Scripts.Game.Stats.Runtime.Controller;
using _Root.Scripts.Game.Storages.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime
{
    public class GameEntity : MonoBehaviour
    {
        private ITitleGuidReference _titleGuidReference;
        private EntityStats _entityStats;
        private IGameItemStorageReference _itemStorageReference;

        public EntityStatsScriptable entityStatsScriptable;
        public bool cloneStats = true;
        private Health _health;

        private void Awake()
        {
            _titleGuidReference = gameObject.GetComponent<ITitleGuidReference>();
            _entityStats = entityStatsScriptable.GetStats(_titleGuidReference.TitleGuid);

            _itemStorageReference = GetComponent<IGameItemStorageReference>();
        }

        private void OnEnable()
        {
            if (cloneStats) _entityStats = (EntityStats)_entityStats.Clone();
            _health = new Health(
                _entityStats.vitality.health,
                _entityStats.defensive.armor,
                _entityStats.defensive.shield,
                _entityStats.critical
            );
            _entityStats.vitality.health.current.OnChange += OnHealthChange;
        }

        private void OnDisable()
        {
            _entityStats.vitality.health.current.OnChange -= OnHealthChange;
        }

        private void OnHealthChange(float old, float current)
        {
            if (current <= 0)
            {
                var deathCallBacks = GetComponents<IDeathCallBack>();
                foreach (var deathCallBack in deathCallBacks) deathCallBack.OnDeath();
                foreach (var (gameItem, value) in _itemStorageReference.GameItemStorage)
                {
                    if (gameItem.DropOnDeath) gameItem.OnDrop(gameObject, gameObject.transform.position, value);
                }
            }
        }
    }
}