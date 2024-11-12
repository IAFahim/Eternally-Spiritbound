#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#endif
using UnityEngine;
using System.Collections.Generic;

namespace Pancake.MobileInput
{
    public static class TouchWrapper
    {
#if ENABLE_INPUT_SYSTEM
        private static readonly InputAction mousePosition;
        private static readonly InputAction mouseButton;

        static TouchWrapper()
        {
            EnhancedTouchSupport.Enable();
            
            mousePosition = new InputAction(type: InputActionType.Value, binding: "<Mouse>/position");
            mouseButton = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
            
            mousePosition.Enable();
            mouseButton.Enable();
        }
#endif

        public static int TouchCount
        {
            get
            {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
    #if ENABLE_INPUT_SYSTEM
                if (Touch.activeTouches.Count > 0) return Touch.activeTouches.Count;
                return mouseButton.IsPressed() ? 1 : 0;
    #else
                if (Input.touchCount > 0) return Input.touchCount;
                return Input.GetMouseButton(0) ? 1 : 0;
    #endif
#else
    #if ENABLE_INPUT_SYSTEM
                return Touch.activeTouches.Count;
    #else
                return Input.touchCount;
    #endif
#endif
            }
        }

        public static TouchData Touch0
        {
            get
            {
                if (TouchCount > 0)
                {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
    #if ENABLE_INPUT_SYSTEM
                    if (Touch.activeTouches.Count > 0)
                        return TouchData.From(Touch.activeTouches[0]);
                    return new TouchData { Position = mousePosition.ReadValue<Vector2>() };
    #else
                    return Input.touchCount > 0 ? 
                        TouchData.From(Input.touches[0]) : 
                        new TouchData { Position = Input.mousePosition };
    #endif
#else
    #if ENABLE_INPUT_SYSTEM
                    return TouchData.From(Touch.activeTouches[0]);
    #else
                    return TouchData.From(Input.touches[0]);
    #endif
#endif
                }
                return null;
            }
        }

        public static bool IsFingerDown => TouchCount > 0;

        public static List<TouchData> Touches
        {
            get
            {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
    #if ENABLE_INPUT_SYSTEM
                return Touch.activeTouches.Count > 0 ? 
                    GetTouchesFromInput() : 
                    new List<TouchData> { Touch0 };
    #else
                return Input.touchCount > 0 ? 
                    GetTouchesFromInput() : 
                    new List<TouchData> { Touch0 };
    #endif
#else
                return GetTouchesFromInput();
#endif
            }
        }

        private static List<TouchData> GetTouchesFromInput()
        {
            var touches = new List<TouchData>();
#if ENABLE_INPUT_SYSTEM
            foreach (var touch in Touch.activeTouches)
            {
                touches.Add(TouchData.From(touch));
            }
#else
            foreach (var touch in Input.touches)
            {
                touches.Add(TouchData.From(touch));
            }
#endif
            return touches;
        }

        public static Vector2 AverageTouchPos
        {
            get
            {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
    #if ENABLE_INPUT_SYSTEM
                if (Touch.activeTouches.Count > 0)
                    return GetAverageTouchPositionFromInput();
                return mousePosition.ReadValue<Vector2>();
    #else
                if (Input.touchCount > 0)
                    return GetAverageTouchPositionFromInput();
                return Input.mousePosition;
    #endif
#else
                return GetAverageTouchPositionFromInput();
#endif
            }
        }

        private static Vector2 GetAverageTouchPositionFromInput()
        {
            var position = Vector2.zero;
#if ENABLE_INPUT_SYSTEM
            if (Touch.activeTouches.Count > 0)
            {
                foreach (var touch in Touch.activeTouches)
                {
                    position += touch.screenPosition;
                }
                position /= Touch.activeTouches.Count;
            }
#else
            if (Input.touches != null && Input.touches.Length > 0)
            {
                foreach (var touch in Input.touches)
                {
                    position += touch.position;
                }
                position /= Input.touches.Length;
            }
#endif
            return position;
        }
    }
}