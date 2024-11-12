using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#endif

namespace Pancake.MobileInput
{
    /// <summary>
    /// Wrapper touch data that works with both legacy and new input system
    /// </summary>
    public class TouchData
    {
        public Vector3 Position { get; set; }
        public int FingerId { get; set; } = -1;

#if ENABLE_INPUT_SYSTEM
        public static TouchData From(Touch touch) =>
            new() { Position = touch.screenPosition, FingerId = touch.touchId };
#else
        public static TouchData From(UnityEngine.Touch touch) =>
            new() { Position = touch.position, FingerId = touch.fingerId };
#endif

        public static TouchData FromMouse()
        {
#if ENABLE_INPUT_SYSTEM
            var mousePosition = Mouse.current?.position.ReadValue() ?? Vector2.zero;
            return new TouchData { Position = mousePosition, FingerId = 0 };
#else
            return new TouchData { Position = Input.mousePosition, FingerId = 0 };
#endif
        }
    }
}