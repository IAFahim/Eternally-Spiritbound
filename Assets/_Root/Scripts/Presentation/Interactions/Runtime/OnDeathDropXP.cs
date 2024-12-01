using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Model.Assets.Runtime;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Presentation.Interactions.Runtime
{
    public class OnDeathDropXP : MonoBehaviour, IInitializable<int>, IDeathCallBack
    {
        public AssetScript xpAsset;
        private float _xp;
        public void Init(int xp)
        {
            
        }

        public void OnDeath()
        {
            
        }
    }
}