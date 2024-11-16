using System;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Farmings.Runtime;
using _Root.Scripts.Model.Links.Runtime;
using Pancake.Common;
using Pancake.Pattern;
using Sirenix.OdinInspector;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Farmings.Runtime
{
    public class CropFsm : MonoBehaviour
    {
        public AssetScript cropAsset;
        public UnityTimeSpan growthTime;
        public AssetScriptMeshesLink assetScriptMeshesLink;

        public CropData cropData;
        public FieldIdle fieldIdle;
        public PlantCrop plantCrop;
        public PlantGrow plantGrow;
        public HarvestCrop harvestCrop;

        private StateMachine _cropFsm;

        [Button]
        public void PutCrop()
        {
            assetScriptMeshesLink.TryGetValue(cropAsset, out var cropMeshes);
            cropData.Initialize(cropAsset, DateTime.UtcNow, growthTime, cropMeshes);
        }


        [Button]
        private void ActiveCrop()
        {
            PutCrop();
            SetupStateMachine();
            App.AddListener(EUpdateMode.Update, OnUpdateFSM);
        }

        private void SetupStateMachine()
        {
            _cropFsm = new StateMachine();

            fieldIdle = new FieldIdle(cropData);
            plantCrop = new PlantCrop(cropData);
            plantGrow = new PlantGrow(cropData);
            harvestCrop = new HarvestCrop(cropData);

            _cropFsm.AddTransition(fieldIdle, plantCrop, plantCrop.CanPlant);
            _cropFsm.AddTransition(plantCrop, plantGrow, plantCrop.CanPlant);
            _cropFsm.AddTransition(plantGrow, harvestCrop, plantGrow.IsCompleteGrowing);
            _cropFsm.AddTransition(harvestCrop, fieldIdle, () =>
            {
                cropData.asset = null;
                App.RemoveListener(EUpdateMode.Update, OnUpdateFSM);
                return true;
            });


            _cropFsm.ChangeState(fieldIdle);
        }

        public void OnUpdateFSM()
        {
            _cropFsm.Update();
        }
    }
}