using _Root.Scripts.Game.Consmatics.Runtime;
using _Root.Scripts.Game.GameEntities.Runtime;
using _Root.Scripts.Game.GameEntities.Runtime.Healths;
using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using Pancake.Common;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Presentation.Interactions.Runtime
{
    public class DamageFlash : MonoBehaviour<FlashConfigScript>
    {
        private FlashConfigScript _flashConfigScript;
        private Renderer _targetRenderer;
        private EntityStatsComponent _entityStatsComponent;
        private Material _defaultMaterial;
        private DelayHandle _delayHandle;

        protected override void Init(FlashConfigScript argument)
        {
            _flashConfigScript = argument;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _entityStatsComponent = GetComponent<EntityStatsComponent>();
            _targetRenderer = GetComponentInChildren<Renderer>();
            _defaultMaterial = _targetRenderer.material;
        }
        
        private void OnEnable()
        {
            _defaultMaterial = _targetRenderer.material;
            _entityStatsComponent.RegisterChange(OnEntityStatsChange, OnEntityStatsDisable);
        }
        
        private void OnEntityStatsChange()
        {
            _entityStatsComponent.entityStats.vitality.health.current.OnChange += OnHealthChange;
        }
        
        private void OnEntityStatsDisable()
        {
            _entityStatsComponent.entityStats.vitality.health.current.OnChange -= OnHealthChange;
            _delayHandle?.Cancel();
            Restore();
        }
        

        private void OnHealthChange(float old, float current)
        {
            _flashConfigScript.Flash(_targetRenderer);
            _delayHandle = App.Delay(_flashConfigScript.flashDuration, Restore, useRealTime: true);
        }

        private void Restore() => _flashConfigScript.Restore(_targetRenderer, _defaultMaterial);
    }
}