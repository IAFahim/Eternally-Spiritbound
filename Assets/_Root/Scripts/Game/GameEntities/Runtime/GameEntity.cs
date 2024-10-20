using _Root.Scripts.Game.GameEntities.Runtime.Attacks;
using _Root.Scripts.Game.GameEntities.Runtime.Damages;
using _Root.Scripts.Game.MainProviders.Runtime;
using _Root.Scripts.Game.Stats.Runtime.Controller;
using _Root.Scripts.Game.Storages.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime
{
    [DisallowMultipleComponent, SelectionBase]
    public class GameEntity : FocusConsumerComponent, IDamage
    {
        private IGameItemStorageReference _itemStorageReference;
        private IEntityStatsReference _entityStatsReference;


        private void Awake()
        {
            _itemStorageReference = GetComponent<IGameItemStorageReference>();
            _entityStatsReference = GetComponent<IEntityStatsReference>();
        }

        private void OnEnable()
        {
            _entityStatsReference.EntityStats.vitality.health.current.OnChange += OnHealthChange;
        }

        private void OnDisable()
        {
            _entityStatsReference.EntityStats.vitality.health.current.OnChange -= OnHealthChange;
        }

        private void OnHealthChange(float old, float current)
        {
            if (current <= 0)
            {
                AnnounceDeath();
                DropItem();
            }

            Debug.Log($"{gameObject.name} has {current} health left.");
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
                if (gameItem.DropOnDeath)
                {
                    gameItem.OnDrop(gameObject, gameObject.transform.position, value);
                    _itemStorageReference.GameItemStorage.TryRemove(gameItem, value, out _);
                }
            }
        }

        public bool TryKill(AttackOrigin attackOrigin, out DamageInfo damage)
        {
            damage = new DamageInfo();
            return false;
        }

        public bool TryKill(float damage, out float damageDelt)
        {
            return _entityStatsReference.EntityStats.TryKill(damage, out damageDelt);
        }
    }
}