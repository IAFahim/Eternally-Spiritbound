using System;
using Soul2.Datas.Runtime.Interface;

namespace Soul2.Levels.Runtime
{
    [Serializable]
    public abstract class LevelBase : IStorageAdapter<int>
    {
        public int CurrentLevel { get; protected set; } = 1;
        public event Action<int, int> OnLevelChange;
        public string Guid { get; set; }

        public void SetData(int level)
        {
            if (level <= 0) throw new ArgumentOutOfRangeException(nameof(level), "Level must be greater than 0.");
            if (CurrentLevel == level) return;
            
            int oldLevel = CurrentLevel;
            CurrentLevel = level;
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
    }
}