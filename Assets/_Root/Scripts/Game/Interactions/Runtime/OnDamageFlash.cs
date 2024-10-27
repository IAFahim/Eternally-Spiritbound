using System;
using _Root.Scripts.Game.Consmatics.Runtime;
using _Root.Scripts.Game.GameEntities.Runtime;
using Pancake;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Game.Interactions.Runtime
{
    [RequireComponent(typeof(GameEntity))]
    public class OnDamageFlash : GameComponent
    {
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private EntityStatsComponent entityStatsReference;
        
        private Material _defaultMaterial;
        private DelayHandle _delayHandle;
        public FlashConfigScript flashConfigScript;
        
        public void OnEnable()
        {
            _defaultMaterial = targetRenderer.material;
            entityStatsReference.EntityStats.vitality.health.current.OnChange += OnHealthChange;
        }

        private void OnHealthChange(float old, float current)
        {
            flashConfigScript.Flash(targetRenderer);
            _delayHandle = App.Delay(flashConfigScript.flashDuration, Restore, useRealTime: true);
        }

        private void Restore() => flashConfigScript.Restore(targetRenderer, _defaultMaterial);

        public void OnDisable()
        {
            entityStatsReference.EntityStats.vitality.health.current.OnChange -= OnHealthChange;
            _delayHandle?.Cancel();
        }

        private void OnValidate()
        {
            entityStatsReference ??= GetComponent<EntityStatsComponent>();
            targetRenderer ??= GetComponentInChildren<Renderer>();
            
        }
    }
}