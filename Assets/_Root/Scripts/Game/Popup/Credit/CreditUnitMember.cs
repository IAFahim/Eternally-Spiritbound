using TMPro;
using UnityEngine;

namespace _Root.Scripts.Game.Popup.Credit
{
    public class CreditUnitMember : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI unitMemberPrefab;

        public void Setup(string title, string[] members)
        {
            textTitle.text = title;
            foreach (string member in members)
            {
                var instance = Instantiate(unitMemberPrefab, transform);
                instance.text = member;
            }
        }
    }
}