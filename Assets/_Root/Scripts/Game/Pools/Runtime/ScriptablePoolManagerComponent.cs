﻿using Soul.Pools.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Pools.Runtime
{
    public class ScriptablePoolManagerComponent : MonoBehaviour
    {
        [SerializeField] private ScriptablePool scriptablePool;
        [SerializeField] private AsyncScriptablePool asyncScriptablePool;

        private void OnDestroy()
        {
            scriptablePool.ClearAll();
            asyncScriptablePool.ClearAll();
        }
    }
}