using _Root.Scripts.Game.GameEntities.Runtime.Healths;
using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using Cysharp.Threading.Tasks;
using Pancake.Pools;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.GameEntities.Runtime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityStatsComponent))]
    public class GameEntity : AssetScriptReferenceComponent, IHealth
    {
        public EntityStatsComponent entityStatsComponent;
        public AssetReferenceGameObject deathEffectAsset;
        

        #region Plumbing

        private void OnEnable()
        {
            entityStatsComponent.RegisterChange(OnEntityStatsChange, OnOldEntityStatsCleanUp);
        }

        private void OnEntityStatsChange()
        {
            entityStatsComponent.entityStats.vitality.health.current.OnChange += OnHealthChange;
            OnHealthChange(0, entityStatsComponent.entityStats.vitality.health.current);
        }

        private void OnOldEntityStatsCleanUp()
        {
            entityStatsComponent.entityStats.vitality.health.current.OnChange -= OnHealthChange;
        }


        private void OnDisable()
        {
            OnOldEntityStatsCleanUp();
        }

        private void OnValidate()
        {
            entityStatsComponent ??= GetComponent<EntityStatsComponent>();
        }

        #endregion


        private void OnHealthChange(float old, float current)
        {
            if (current <= 0)
            {
                AnnounceDeath();
            }

            Debug.Log($"{gameObject.name} has {current} health left.", this);
        }

        private void AnnounceDeath()
        {
            var deathCallBacks = GetComponents<IDeathCallBack>();
            foreach (var deathCallBack in deathCallBacks) deathCallBack.OnDeath();
            deathEffectAsset.RequestAsync(transform).Forget();
        }

        public void Kill()
        {
            entityStatsComponent.entityStats.vitality.health.current.Value = 0;
        }
    }
}