﻿using System.Collections.Generic;
using Pancake.Pools;
using UnityEngine;

namespace Soul.Pools.Runtime
{
    internal static class PoolCallbackHelper
    {
        private static readonly List<IPoolCallbackReceiver> ComponentsBuffer = new();

        public static void InvokeOnRequest(GameObject obj)
        {
            obj.GetComponentsInChildren(ComponentsBuffer);
            foreach (var receiver in ComponentsBuffer)
            {
                receiver.OnRequest();
            }
        }

        public static void InvokeOnReturn(GameObject obj)
        {
            obj.GetComponentsInChildren(ComponentsBuffer);
            foreach (var receiver in ComponentsBuffer)
            {
                receiver.OnReturn();
            }
        }
    }
}