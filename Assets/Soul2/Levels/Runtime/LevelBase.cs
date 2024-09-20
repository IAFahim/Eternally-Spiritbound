using System;
using Soul2.Datas.Runtime.Interface;
using UnityEngine;

namespace Soul2.Levels.Runtime
{
    [Serializable]
    public abstract class LevelBase : IDataAdapter<int>, ILoadThenSave
    {
        [SerializeField] protected int currentLevel = 1;
        public event Action<int, int> OnLevelChange;
        protected string guid;

        public int CurrentLevel => currentLevel;

        public void SetLevel(int level)
        {
            if (level <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(level), "Level must be greater than 0.");
            }

            int oldLevel = currentLevel;
            currentLevel = level;
            OnLevelChange?.Invoke(oldLevel, currentLevel);
        }

        public void IncreaseLevel() => SetLevel(currentLevel + 1);

        public void DecreaseLevel()
        {
            if (currentLevel > 1)
            {
                SetLevel(currentLevel - 1);
            }
        }

        public string Guid => guid;
        public virtual string DataKey => $"{guid}_lv";
        public int DefaultData => 1;

        public abstract int LoadData();
        public abstract void SaveData(int data);
        public void FirstLoad(string guid)
        {
            this.guid = guid;
            currentLevel = LoadData();
        }

        public void Save()
        {
            SaveData(currentLevel);
        }
    }
}