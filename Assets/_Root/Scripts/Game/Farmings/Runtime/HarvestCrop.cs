using System;
using _Root.Scripts.Model.Farmings.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Farmings.Runtime
{
    [Serializable]
    public class HarvestCrop : CropState
    {
        public bool readyToHarvest;

        public HarvestCrop(CropData cropData) : base(cropData)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Harvest Started");
            readyToHarvest = true;
        }

        public override void OnUpdate()
        {
            Debug.Log("Harvesting");
        }

        public override void OnExit()
        {
            Debug.Log("Harvest Finished");
            readyToHarvest = false;
        }
    }
}