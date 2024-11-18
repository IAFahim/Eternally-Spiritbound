using System;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Farmings.Runtime;
using _Root.Scripts.Model.Links.Runtime;
using Pancake.Common;
using Pancake.Pattern;
using Sirenix.OdinInspector;
using Soul.Selectors.Runtime;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Farmings.Runtime
{
    public class CropFsm : MonoBehaviour, ISelectCallBackReceiver
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
        private IMeshPlanter _meshPlanter;
        public GameObject testPos;

        private void Awake() => _meshPlanter = GetComponent<IMeshPlanter>();

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
            plantGrow = new PlantGrow(cropData, _meshPlanter);
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

        public void Plant(Mesh mesh)
        {
            Debug.Log($"Planting {mesh.name}");
        }

        public void OnSelected(RaycastHit hit)
        {
            Debug.Log("Selected");
        }

        public void OnUpdateDrag(RaycastHit hitRef, bool isInside, Vector3 worldPosition, Vector3 delta)
        {
            Debug.DrawLine(testPos.transform.position, worldPosition, Color.red);
            testPos.transform.position = worldPosition;
            Debug.Log($"UpdateDrag: {worldPosition} {delta} {isInside}");
        }

        public void OnDragEnd(RaycastHit hitRef, bool isInside, Vector3 worldPosition)
        {
            Debug.Log($"DragEnd: {worldPosition} {isInside}");
        }


        public void OnDeselected(RaycastHit lastHitInfo, RaycastHit hit)
        {
            Debug.Log("Deselected");
        }

        public void OnReselected(RaycastHit hit)
        {
            Debug.Log("Reselected");
        }
    }
}