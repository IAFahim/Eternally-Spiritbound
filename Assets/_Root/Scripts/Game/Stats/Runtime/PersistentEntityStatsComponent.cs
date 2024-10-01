using _Root.Scripts.Game.Guid;
using _Root.Scripts.Game.Stats.Runtime.Model;
using Soul.Modifiers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime
{
    public class PersistentEntityStatsComponent : MonoBehaviour, IEntityStats
    {
        [SerializeField] EntityStatsScriptable entityStatsScriptable;
        public EntityStatsBase<Modifier> EntityStats { get; private set; }

        private void Awake()
        {
            EntityStats = entityStatsScriptable.GetStats(gameObject.GetComponent<ITitleGuidReference>().TitleGuid);
        }
    }
}