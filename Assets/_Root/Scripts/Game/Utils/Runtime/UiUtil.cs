using UnityEngine;

namespace _Root.Scripts.Game.Utils.Runtime
{
    public static class UiUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scroll"></param>
        /// <param name="target"></param>
        /// <param name="isVertical"></param>
        public static Vector2 ScrollNormalizedPosition(this UnityEngine.UI.ScrollRect scroll, Transform target,
            bool isVertical = true)
        {
            return isVertical
                ? new Vector2(
                    0, 1 - (scroll.content.rect.height / 2 - target.localPosition.y) / scroll.content.rect.height
                )
                : new Vector2(
                    1 - (scroll.content.rect.width / 2 - target.localPosition.x) / scroll.content.rect.width,
                    0
                );
        }
    }
}