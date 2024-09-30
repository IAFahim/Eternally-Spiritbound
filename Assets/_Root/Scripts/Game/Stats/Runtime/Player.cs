using _Root.Scripts.Game.Guid;
using Pancake.StatModifier;
using Soul.Modifiers.Runtime;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private bool saved;
        private GuidProvider _guidProvider;

        [SerializeField] private StatsConstant<float> statsBase;
        public Stats<Modifier> stats;
        
        public UnityDictionary<BaseStat, Modifier> statsDictionary;

        public void Awake() => _guidProvider = GetComponent<GuidProvider>();

        public void Start()
        {
            foreach (var (baseStat, modifier) in statsDictionary)
            {
                modifier.SetBaseWithoutNotify(baseStat.baseValue);
            }
        }
    }
}