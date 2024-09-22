using System;
using Soul2.Datas.Runtime.Interface;
using UnityEngine;

namespace Soul2.Levels.Runtime
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

        public void SetData(int level)
        {
            if (level <= 0) throw new ArgumentOutOfRangeException(nameof(level), "Level must be greater than 0.");
            if (CurrentLevel == level) return;
            
            int oldLevel = CurrentLevel;
            currentLevel = level;
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