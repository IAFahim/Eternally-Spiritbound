using System;
using Soul2.Datas.Runtime.Interface;
using UnityEngine;
using Math = System.Math;

namespace Soul2.Levels.Runtime
{
    [Serializable]
    public abstract class XpLevelBase : LevelBase, IStorageAdapter<(int, int)>
    {
        public event Action<int, int> OnXpChange;
        [SerializeField] protected int xp;

        [SerializeField] private int baseXp = 10;
        [SerializeField] private float xpMultiplier = 1.5f;
        [SerializeField] private int maxLevel = 10;
        private int _xpToNextLevel;

        public int Xp => xp;
        public int XpToNextLevel => _xpToNextLevel;
        public float XpProgress => _xpToNextLevel > 0 ? (float)xp / _xpToNextLevel : 1f;

        public void SetData((int, int) data)
        {
            SetData(data.Item1);
            if (currentLevel >= maxLevel) return;
            xp = data.Item2;
            _xpToNextLevel = CalculateXpToNextLevel(currentLevel, maxLevel, baseXp, xpMultiplier);
        }


        public void AddXp(int amount)
        {
            if (currentLevel >= maxLevel) return;

            int oldXp = xp;
            xp += amount;

            while (xp >= _xpToNextLevel && currentLevel < maxLevel)
            {
                xp -= _xpToNextLevel;
                IncreaseLevel();
                _xpToNextLevel = CalculateXpToNextLevel(currentLevel, maxLevel, baseXp, xpMultiplier);
            }

            OnXpChange?.Invoke(oldXp, xp);
        }

        public virtual int CalculateXpToNextLevel(int currentLv, int maxLv, int xpBase, float xpMult)
        {
            if (currentLv >= maxLv) return 0;
            return (int)(xpBase * Math.Pow(xpMult, currentLv - 1));
        }

        public void Reset()
        {
            OnXpChange?.Invoke(xp, xp = 0);
        }

        public abstract void SaveData((int, int) data);

        public override void SaveData()
        {
            SaveData((currentLevel, xp));
        }
    }
}