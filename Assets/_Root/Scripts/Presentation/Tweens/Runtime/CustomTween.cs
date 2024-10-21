using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace _Root.Scripts.Presentation.Tweens.Runtime
{
    [Serializable]
    public static class CustomTween
    {
        public static async UniTaskVoid Jump(Transform transform, float jumpHeight, Vector3 dropPosition, float duration,
            Action onComplete)
        {
            var jumpHalfPosition = (transform.position + dropPosition) / 2;
            var jumpPeekPosition = jumpHalfPosition + Vector3.up * jumpHeight;
            LMotion.Create(transform.position, jumpPeekPosition, duration / 2).BindToLocalPosition(transform);

            await LMotion.Create(jumpPeekPosition, dropPosition, duration / 2).BindToLocalPosition(transform)
                .ToAwaitable();

            onComplete?.Invoke();
        }
    }
}