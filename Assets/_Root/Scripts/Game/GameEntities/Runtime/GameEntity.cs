using _Root.Scripts.Game.Combats.Runtime.Attacks;
using _Root.Scripts.Game.Combats.Runtime.Damages;
using _Root.Scripts.Game.GameEntities.Runtime.Attacks;
using _Root.Scripts.Game.Guid;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.Stats.Runtime.Controller;
using _Root.Scripts.Game.Storages.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime
{
    [DisallowMultipleComponent, SelectionBase]
    public class GameEntity : MonoBehaviour, IEntityStatsReference, IDamage
    {
        private ITitleGuidReference _titleGuidReference;
        private EntityStats _entityStats;
        private IGameItemStorageReference _itemStorageReference;

        public EntityStatsScriptable entityStatsScriptable;
        public bool cloneStats = true;
        private Health _health;
        [ShowInInspector] public EntityStats EntityStats => _entityStats;
        
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
                AnnounceDeath();
                DropItem();
            }
        }

        private void AnnounceDeath()
        {
            var deathCallBacks = GetComponents<IDeathCallBack>();
            foreach (var deathCallBack in deathCallBacks) deathCallBack.OnDeath();
        }

        private void DropItem()
        {
            foreach (var (gameItem, value) in _itemStorageReference.GameItemStorage)
            {
                if (gameItem.DropOnDeath) gameItem.OnDrop(gameObject, gameObject.transform.position, value);
            }
        }

        public bool TryKill(AttackOrigin attackOrigin, out DamageInfo damage)
        {
            damage = new DamageInfo();
            return false;
        }

        public bool TryKill(float damage, out DamageInfo damageInfo)
        {
            bool dead = _health.TryKill(damage, out var damageTaken);
            damageInfo = new DamageInfo
            {
                damaged = gameObject,
                damageTaken = damage
            };
            return dead;
        }
    }
}