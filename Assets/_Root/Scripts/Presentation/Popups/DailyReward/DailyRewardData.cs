using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Popups.DailyReward
{
    [CreateAssetMenu(menuName = "Pancake/Game/Daily Reward Data", fileName = "daily_reward_data.asset")]
    [EditorIcon("icon_so_blue")]
    public class DailyRewardData : ScriptableObject
    {
        public int day;
        public DayReward[] rewards;
    }
}