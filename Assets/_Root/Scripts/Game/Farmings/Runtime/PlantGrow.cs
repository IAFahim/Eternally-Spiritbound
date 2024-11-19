using System;
using _Root.Scripts.Model.Farmings.Runtime;
using _Root.Scripts.Model.ObjectPlacers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Farmings.Runtime
{
    [Serializable]
    public class PlantGrow : CropState
    {
        public float timeToGrow;
        private IMeshPlanter _planter;

        public PlantGrow(CropData cropData, IMeshPlanter meshPlanter) : base(cropData)
        {
            _planter = meshPlanter;
        }

        public override void OnEnter()
        {
            _planter.Plant(cropData.Meshes[0]);
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
            _planter.MoveSlice();
        }
    }
}