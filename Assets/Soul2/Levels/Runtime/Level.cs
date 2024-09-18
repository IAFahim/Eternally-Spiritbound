using System;
using Pancake.Common;
using Soul2.LocalDatas.Runtime;
using UnityEngine;

namespace Soul2.Levels.Runtime
{
    [Serializable]
    public class Level : ILocalData
    {
        [SerializeField] private int currentLevel = 1;

        public event Action<int, int> OnLevelChange;
        protected string guid;
        
        public virtual string LocalKey => $"{guid}_lv";

        public int CurrentLevel
        {
            get => currentLevel;
            protected set => currentLevel = value;
        }

        public void LocalLoad(string guid)
        {
            this.guid = guid;
            currentLevel = Data.Load(LocalKey, 1);
        }

        public void LocalSave()
        {
            Data.Save(LocalKey, currentLevel);
        }

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

        public void IncreaseLevel()
        {
            SetLevel(currentLevel + 1);
        }

        public void DecreaseLevel()
        {
            if (currentLevel > 1)
            {
                SetLevel(currentLevel - 1);
            }
        }

        

        
    }
}