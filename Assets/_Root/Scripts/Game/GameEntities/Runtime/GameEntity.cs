using _Root.Scripts.Game.GameEntities.Runtime.Attacks;
using _Root.Scripts.Game.GameEntities.Runtime.Damages;
using _Root.Scripts.Game.GameEntities.Runtime.Healths;
using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Game.Storages.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using Soul.Modifiers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityStatsComponent))]
    public class GameEntity : MonoBehaviour, IDamage, IHealth
    {
        public EntityStatsComponent entityStatsComponent;
        private IGameItemStorageReference _itemStorageReference;

        #region Plumbing

        private void Awake()
        {
            _itemStorageReference = GetComponent<IGameItemStorageReference>();
            entityStatsComponent ??= GetComponent<EntityStatsComponent>();
        }

        private void OnEnable()
        {
            entityStatsComponent.entityStats.Initialize();
            entityStatsComponent.entityStats.vitality.health.current.OnChange += OnHealthChange;
        }

        private void OnDisable()
        {
            entityStatsComponent.entityStats.vitality.health.current.OnChange -= OnHealthChange;
        }

        private void OnValidate()
        {
            entityStatsComponent ??= GetComponent<EntityStatsComponent>();
        }

        #endregion

        public EnableLimitStat<Modifier> HealthReference => entityStatsComponent.entityStats.vitality.health;

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
            return entityStatsComponent.entityStats.TryKill(damage, out damageDelt);
        }
    }
}