using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.Containers.Runtime
{
    public class ButtonSelectionController : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private UIEffectTweener effectTweener;

        [SerializeField] private Image statusImage;

        private bool _selected;
        private int _index;
        private UnityAction<int> _unityAction;


        public void Initialize(int index, bool selected, Sprite sprite, Sprite statusSprite,
            UnityAction<int> onSelected)
        {
            _selected = selected;
            _index = index;
            icon.sprite = sprite;
            SetStatus(statusSprite);
            _unityAction = onSelected;
            button.onClick.AddListener(OnClick);
        }

        public void SetStatus(Sprite statusSprite)
        {
            var statusNull = statusSprite == null;
            if (!statusNull) statusImage.sprite = statusSprite;
            statusImage.enabled = !statusNull;
        }

        private void Update()
        {
            if (_selected) effectTweener.UpdateTime(Time.deltaTime);
        }

        private void OnClick()
        {
            _selected = true;
            _unityAction?.Invoke(_index);
        }

        public void DeSelect()
        {
            if (_selected) effectTweener.Restart();
            _selected = false;
        }

        private void OnDisable()
        {
            if (_unityAction != null) button.onClick.RemoveListener(OnClick);
            DeSelect();
        }

        private void Reset()
        {
            button = GetComponent<Button>();
            var images = GetComponentsInChildren<Image>(true);

            icon = images[1];
            statusImage = images[2];
            effectTweener = GetComponent<UIEffectTweener>();
        }
    }
}