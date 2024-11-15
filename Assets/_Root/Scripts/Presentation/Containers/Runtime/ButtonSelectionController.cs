using System;
using Coffee.UIEffects;
using JetBrains.Annotations;
using Pancake;
using Pancake.Common;
using UnityEngine;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.Containers.Runtime
{
    public class ButtonSelectionController : MonoBehaviour
    {
        [SerializeField] private Button button;

        [SerializeField] private Image image;

        [SerializeField] private Optional<UIEffectTweener> effectTweener;
        [SerializeField] private Image statusImage;


        private bool _selected;
        private int _index;
        private Action<int> _onIndexClick;


        public void Initialize(int index, bool selected, Sprite icon, [CanBeNull] StatusSprite statusSprite,
            Action<int> onSelected)
        {
            _selected = selected;
            _index = index;
            image.sprite = icon;
            SetStatusImage(statusSprite);
            _onIndexClick = onSelected;
            button.onClick.AddListener(OnClick);
            if (_selected && effectTweener.Enabled) App.AddListener(EUpdateMode.Update, OnUpdate);
        }


        private void OnUpdate()
        {
            if (_selected) effectTweener.Value.UpdateTime(Time.deltaTime);
        }

        public void SetStatusImage([CanBeNull] StatusSprite statusSprite)
        {
            if (statusSprite == null)
            {
                statusImage.enabled = false;
                return;
            }

            statusSprite.Apply(statusImage);
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