using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.Containers.Runtime
{
    [Serializable]
    public class StatusSprite
    {
        public Sprite sprite;
        public Vector2 anchorMin;

        public void Apply(Image image)
        {
            image.rectTransform.anchorMin = anchorMin;
            image.sprite = sprite;
            image.gameObject.SetActive(true);
        }
    }
}