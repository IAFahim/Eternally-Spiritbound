using System;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using LitMotion;
using UnityEngine;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    public class LevelConfig : MonoBehaviour
    {
        public MainObjectProviderScriptable mainObjectProvider;
        public SpawnStrategy[] spawnStrategy;
        
        private CompositeMotionHandle _motionHandle;

        public void Start()
        {
            _motionHandle = new CompositeMotionHandle();
            // foreach (var strategy in spawnStrategy)
            // {
            //     _motionHandle.AddMotion(strategy);
            // }
        }
    }
}