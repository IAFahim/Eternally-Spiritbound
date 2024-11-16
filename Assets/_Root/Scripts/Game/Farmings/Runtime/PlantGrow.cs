using System;
using _Root.Scripts.Model.Farmings.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Farmings.Runtime
{
    [Serializable]
    public class PlantGrow : CropState
    {
        public float timeToGrow;


        public PlantGrow(CropData cropData) : base(cropData)
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

        public bool IsCompleteGrowing() => cropData.IsOverGrowthTime();

        public override void OnExit()
        {
            Debug.Log("Planting Finished");
        }
    }
}