using System.Globalization;
using Sisus.Init;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.Containers.Runtime
{
    public class StatsViewController : MonoBehaviour, IInitializable<Sprite, string, float>
    {
        public Image image;
        public TMP_Text title;
        public TMP_Text value;

        public void Init(Sprite icon, string titleStr, float valueFloat)
        {
            image.sprite = icon;
            title.text = titleStr;
            this.value.text = valueFloat.ToString(CultureInfo.InvariantCulture);
        }
    }
}