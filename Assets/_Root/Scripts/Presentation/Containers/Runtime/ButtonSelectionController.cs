using System;
using Coffee.UIEffects;
using Pancake;
using Pancake.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.Containers.Runtime
{
    public class ButtonSelectionController : MonoBehaviour
    {
        [SerializeField] private Button button;
        [FormerlySerializedAs("icon")] [SerializeField] private Image image;
        [SerializeField] private Optional<UIEffectTweener> effectTweener;

        [SerializeField] private Image statusImage;

        private bool _selected;
        private int _index;
        private Action<int> _onIndexClick;


        public void Initialize(int index, bool selected, Sprite icon, Sprite statusSprite,
            Action<int> onSelected)
        {
            _selected = selected;
            _index = index;
            image.sprite = icon;
            SetStatus(statusSprite);
            _onIndexClick = onSelected;
            button.onClick.AddListener(OnClick);
            if (_selected && effectTweener.Enabled) App.AddListener(EUpdateMode.Update, OnUpdate);
        }

        public void SetStatus(Sprite statusSprite)
        {
            var statusNull = statusSprite == null;
            if (!statusNull) statusImage.sprite = statusSprite;
            statusImage.enabled = !statusNull;
        }

        private void OnUpdate()
        {
            if (_selected) effectTweener.Value.UpdateTime(Time.deltaTime);
        }

        private void OnClick()
        {
            if (!_selected && effectTweener.Enabled) App.AddListener(EUpdateMode.Update, OnUpdate);
            _selected = true;
            _onIndexClick?.Invoke(_index);
        }

        public void DeSelect()
        {
            if (!_selected) return;
            if (effectTweener.Enabled)
            {
                App.RemoveListener(EUpdateMode.Update, OnUpdate);
                effectTweener.Value.Restart();
            }

            _selected = false;
        }

        private void OnDisable()
        {
            if (_onIndexClick != null) button.onClick.RemoveListener(OnClick);
            DeSelect();
        }

        private void Reset()
        {
            button = GetComponent<Button>();
            var images = GetComponentsInChildren<Image>(true);

            image = images[1];
            statusImage = images[2];
            effectTweener = GetComponent<UIEffectTweener>();
        }
    }
}