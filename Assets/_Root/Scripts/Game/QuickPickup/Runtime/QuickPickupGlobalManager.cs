﻿using System.Collections.Generic;
using _Root.Scripts.Model.Items.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Game.QuickPickup.Runtime
{
    public class QuickPickupGlobalManager : MonoBehaviour
    {
        public AllGameItem allGameItem;
        public QuickItemPickupManager itemPickupManager;
        public List<GameItem> autoPickList = new();
        public int randomTest = 100;
        public float range = 1000;
        public float waterLevel = 0;


        private void Start()
        {
            itemPickupManager.Setup();
            SpawnItems();
        }

        private void SpawnItems()
        {
            for (var i = 0; i < randomTest; i++)
            {
                var randomItem = autoPickList[Random.Range(0, autoPickList.Count)];
                var randomV2 = Random.insideUnitCircle * range;
                itemPickupManager.Add(
                    randomItem,
                    transform.position + new Vector3(randomV2.x, waterLevel, randomV2.y),
                    1
                ).Forget();
            }
        }

        public void Update()
        {
            itemPickupManager.Process();
        }
        
        public void OnDisable()
        {
            itemPickupManager.DisposeAll();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            itemPickupManager?.OnDrawGizmos();
        }
#endif
    }
}