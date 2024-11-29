using System;
using TMPro;
using UnityEngine;

namespace _Root.Scripts.Presentation.Containers.Runtime
{
    [Serializable]
    public class TMPTextFormat
    {
        [SerializeField] private TMP_Text tmp;
        public string format;

        public TMP_Text TMP
        {
            get => tmp;
            set
            {
                tmp = value;
                format = tmp.text;
            }
        }

        public static string FormatF1(float number)
        {
            var formatted = number.ToString("F1");
            return formatted.EndsWith(".0") ? number.ToString("0") : formatted;
        }
    }
}