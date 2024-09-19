using TMPro;
using UnityEngine;

namespace _Root.Scripts.Game.Popups.DailyReward
{
    public class DayRewardComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textAmout;

        public void Setup(int amount, bool shouldHide = false)
        {
            if (shouldHide) textAmout.gameObject.SetActive(false);
            else
            {
                textAmout.gameObject.SetActive(true);
                textAmout.text = $"x{amount}";
            }
        }
    }
}