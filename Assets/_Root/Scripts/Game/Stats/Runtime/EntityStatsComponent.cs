using _Root.Scripts.Game.Guid;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime
{
    public class EntityStatsComponent : MonoBehaviour
    {
        public EntityStats entityStats;
        [SerializeField] EntityStatsScriptable entityStatsScriptable;

        private void Awake()
        {
            if (gameObject.TryGetComponent<ITitleGuidReference>(out var titleGuid))
            {
                entityStats = entityStatsScriptable.GetStats(titleGuid.TitleGuid);
            }
        }
    }
}