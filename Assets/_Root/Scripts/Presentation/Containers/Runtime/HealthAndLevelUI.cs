using _Root.Scripts.Game.Stats.Runtime;
using Sisus.Init;
using TMPro;
using UnityEngine;
using UnityProgressBar;

namespace _Root.Scripts.Presentation.Containers.Runtime
{
    public class HealthAndLevelUI : MonoBehaviour, IInitializable<EntityStatsComponent>
    {
        public ProgressBar healthBar;
        public TMPTextFormat healthText;

        public ProgressBar levelBar;
        public TMPTextFormat levelText;

        private EntityStatsComponent _entityStatsComponent;

        public void Init(EntityStatsComponent entityStatsComponent)
        {
            if (_entityStatsComponent != null)
                _entityStatsComponent.UnregisterChange(OnEntityStatsChange, OnOldEntityStatsCleanUp);
            _entityStatsComponent = entityStatsComponent;
            _entityStatsComponent.RegisterChange(OnEntityStatsChange, OnOldEntityStatsCleanUp);
        }

        private void OnOldEntityStatsCleanUp()
        {
            _entityStatsComponent.entityStats.vitality.health.current.OnChange -= OnCurrentHealthChange;
            _entityStatsComponent.entityStats.progression.experience.OnChange -= OnCurrentExperienceChange;
        }

        private void OnEntityStatsChange()
        {
            _entityStatsComponent.entityStats.vitality.health.current.OnChange += OnCurrentHealthChange;
            _entityStatsComponent.entityStats.progression.experience.OnChange += OnCurrentExperienceChange;
            OnCurrentHealthChange(
                _entityStatsComponent.entityStats.vitality.health.current,
                _entityStatsComponent.entityStats.vitality.health.current
            );
            OnCurrentExperienceChange(
                _entityStatsComponent.entityStats.progression.experience,
                _entityStatsComponent.entityStats.progression.experience
            );
        }

        private void OnCurrentExperienceChange(int old, int current)
        {
            (int currentLevel, float nextLevelExperence) = _entityStatsComponent.entityStats.progression
                .GetCurrentLevelAndExperienceForNextLevel();
            levelBar.SetValueWithoutNotify(current / nextLevelExperence);
            levelText.TMP.text = string.Format(levelText.format, TMPTextFormat.FormatF1(currentLevel + 1));
        }

        private void OnCurrentHealthChange(float old, float current)
        {
            var max = _entityStatsComponent.entityStats.vitality.health.max;
            healthBar.SetValueWithoutNotify(current / max);
            healthText.TMP.text = string.Format(healthText.format, TMPTextFormat.FormatF1(current));
        }

        private void Reset()
        {
            var bars = GetComponentsInChildren<ProgressBar>();
            foreach (var bar in bars)
            {
                if (bar.name.ToLower().Contains("heal")) healthBar = bar;
                else if (bar.name.ToLower().Contains("lev")) levelBar = bar;
            }

            var texts = GetComponentsInChildren<TMP_Text>();
            foreach (var text in texts)
            {
                if (text.name.ToLower().Contains("heal")) healthText.TMP = text;
                else if (text.name.ToLower().Contains("lev")) levelText.TMP = text;
            }
        }
    }
}