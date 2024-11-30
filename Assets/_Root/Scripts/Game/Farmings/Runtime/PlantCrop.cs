using System;
using _Root.Scripts.Model.Farmings.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Farmings.Runtime
{
    [Serializable]
    public class PlantCrop : CropState
    {
        public PlantCrop(CropData cropData) : base(cropData)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Planting Started");
        }

        public override void OnUpdate()
        {
            Debug.Log("Planting");
        }

        public override void OnExit()
        {
            Debug.Log("Planting Finished");
        }

        public bool CanPlant() => cropData.seedAsset != null;
    }
}