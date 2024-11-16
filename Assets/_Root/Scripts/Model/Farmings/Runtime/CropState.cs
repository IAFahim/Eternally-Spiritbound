using System;
using Pancake.Pattern;

namespace _Root.Scripts.Model.Farmings.Runtime
{
    [Serializable]
    public abstract class CropState : IState
    {
        public CropData cropData;
        public CropState(CropData cropData) => this.cropData = cropData;

        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
    }
}