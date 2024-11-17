using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    [CreateAssetMenu(fileName = "Bullet", menuName = "Scriptable/Weapon/Bullet")]
    public class Bullet : AssetScript
    {
        [Header("Weapon Strategy")] [SerializeField]
        private OffensiveStatsParameterScript offensiveStats;
    }
}