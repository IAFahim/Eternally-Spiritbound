﻿using System;
using Pancake.Common;
using Soul.Levels.Runtime;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Levels.Runtime
{
    [Serializable]
    public class XpLevel : XpLevelBase
    {
        [SerializeField] private Pair<int, int> defaultData;
        
        [SerializeField] private string appendKey = "_xp";
        public override string StorageKey => $"{Guid}{appendKey}";
        public override void InitializeStorage(string guid, bool load)
        {
            Guid = guid;
            if (load) LoadData(guid);
            else
            {
                GetDefaultData(out var data);
                SetData(data);
            }
        }

        public override void GetDefaultData(out int data)
        {
            data = defaultData.First;
        }

        public override void LoadData(string guid)
        {
            Guid = guid;
            GetDefaultData(out var dataDefault);
            SetData(Data.Load(StorageKey, dataDefault));
        }

        public override void GetDefaultData(out Pair<int, int> data) => data = defaultData;
        public override void SaveData(Pair<int, int> data) => Data.Save(StorageKey, data);
        public override void SaveData(int data) => SaveData((data, Xp));
        public override void ClearStorage() => Data.DeleteKey(StorageKey);
    }
}