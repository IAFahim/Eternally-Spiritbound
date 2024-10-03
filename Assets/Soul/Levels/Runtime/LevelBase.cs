using System;
using Soul.Datas.Runtime.Interface;
using UnityEngine;

namespace Soul.Levels.Runtime
{
    [Serializable]
    public abstract class LevelBase : IStorageAdapter<int>
    {
        [SerializeField] protected int currentLevel = 1;
        private string guid;

        public int CurrentLevel => currentLevel;

        public event Action<int, int> OnLevelChange;

        public string Guid
        {
            get => guid;
            set => guid = value;
        }

        public abstract string StorageKey { get; }
        
        public abstract void InitializeStorage(string guid, bool load);

        public abstract void GetDefaultData(out int dataDefault);

        public void GetData(out int dataCurrent) => dataCurrent = CurrentLevel;

        public void SetData(int dataNew)
        {
            if (dataNew <= 0) throw new ArgumentOutOfRangeException(nameof(dataNew), "Level must be greater than 0.");
            if (CurrentLevel == dataNew) return;
            
            int oldLevel = CurrentLevel;
            currentLevel = dataNew;
            OnLevelChange?.Invoke(oldLevel, CurrentLevel);
        }

        public void IncreaseLevel() => SetData(CurrentLevel + 1);

        public void DecreaseLevel()
        {
            if (CurrentLevel > 1)
            {
                SetData(CurrentLevel - 1);
            }
        }

        public abstract void LoadData(string guid);

        public abstract void SaveData(int data);
        public abstract void SaveData();
        public abstract void ClearStorage();
    }
}