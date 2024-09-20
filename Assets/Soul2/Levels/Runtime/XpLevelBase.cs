using System;
using Soul2.Containers.RunTime;
using Soul2.Datas.Runtime.Interface;
using UnityEngine;
using Math = System.Math;

namespace Soul2.Levels.Runtime
{
    [Serializable]
    public abstract class XpLevelBase : LevelBase, IDataAdapter<int, int>, ILoadThenSave
    {
        public event Action<int, int> OnXpChange;
        [SerializeField] private int baseXp = 10;
        [SerializeField] private float xpMultiplier = 1.5f;
        [SerializeField] private int xp;
        [SerializeField] private int xpToNextLevel;
        [SerializeField] private int maxLevel = 10;

        public int Xp => xp;
        public int XpToNextLevel => xpToNextLevel;
        public float XpProgress => xpToNextLevel > 0 ? (float)xp / xpToNextLevel : 1f;
        public override string DataKey => $"{guid}_xpLv";

        public void Init(int levelCap)
        {
            SetLevel(currentLevel);
            xp = 0;
            CalculateXpToNextLevel();
        }

        public void AddXp(int amount)
        {
            if (currentLevel >= maxLevel) return;

            int oldXp = xp;
            xp += amount;

            while (xp >= xpToNextLevel && currentLevel < maxLevel)
            {
                xp -= xpToNextLevel;
                IncreaseLevel();
                CalculateXpToNextLevel();
            }

            OnXpChange?.Invoke(oldXp, xp);
        }

        private void CalculateXpToNextLevel()
        {
            if (currentLevel >= maxLevel)
            {
                xpToNextLevel = 0;
                return;
            }

            xpToNextLevel = (int)(baseXp * Math.Pow(xpMultiplier, currentLevel - 1));
        }

        public void ResetXp()
        {
            int oldXp = xp;
            xp = 0;
            OnXpChange?.Invoke(oldXp, xp);
        }
        
        
        public (int first, int second) DefaultData2 => (DefaultData, 0);
        public abstract (int first, int second) LoadData2();
        public abstract void SaveData2((int first, int second) data);

        public new void FirstLoad(string guid)
        {
            this.guid = guid;
            LoadData2();
        }

        public new void Save()
        {
            SaveData2((currentLevel, xp));
        }
    }
}