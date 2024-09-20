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

        [SerializeField] private int baseXp = 10;
        [SerializeField] private float xpMultiplier = 1.5f;
        [SerializeField] private int maxLevel = 10;

        private int _xp;
        private int _xpToNextLevel;

        public int Xp => _xp;
        public int XpToNextLevel => _xpToNextLevel;
        public float XpProgress => _xpToNextLevel > 0 ? (float)_xp / _xpToNextLevel : 1f;

        public void SetData((int, int) data)
        {
            SetData(data.Item1);
            if (CurrentLevel >= maxLevel) return;
            _xp = data.Item2;
            _xpToNextLevel = CalculateXpToNextLevel();
        }


        public void AddXp(int amount)
        {
            if (CurrentLevel >= maxLevel) return;

            int oldXp = _xp;
            _xp += amount;

            while (_xp >= _xpToNextLevel && CurrentLevel < maxLevel)
            {
                _xp -= _xpToNextLevel;
                IncreaseLevel();
                _xpToNextLevel= CalculateXpToNextLevel();
            }

            OnXpChange?.Invoke(oldXp, _xp);
        }

        private int CalculateXpToNextLevel()
        {
            if (CurrentLevel >= maxLevel) return 0;
            return (int)(baseXp * Math.Pow(xpMultiplier, CurrentLevel - 1));
        }

        public void Reset()
        {
            int oldXp = _xp;
            _xp = 0;
            OnXpChange?.Invoke(oldXp, _xp);
        }
        
        public abstract void SaveData((int, int) data);
        public abstract override void SaveData();
    }
}