using System;
using _Root.Scripts.Game.GameEntities.Runtime.Healths;
using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityStatsComponent))]
    public class GameEntity : AssetScriptReferenceComponent, IHealth
    {
        public EntityStatsComponent entityStatsComponent;
        

        public Action<GameEntity> OnDeath;

        #region Plumbing

        private void OnEnable()
        {
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

        public LimitStat HealthReference => entityStatsComponent.entityStats.vitality.health;
        

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
        }

        public void Kill() => OnDeath?.Invoke(this);
        
    }
}