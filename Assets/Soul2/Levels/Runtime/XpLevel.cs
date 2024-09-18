using System;
using Pancake.Common;
using Soul2.Containers.RunTime;
using UnityEngine;
using Math = System.Math;

namespace Soul2.Levels.Runtime
{
    [Serializable]
    public class XpLevel : Level
    {
        public event Action<int, int> OnXpChange;

        [SerializeField] private int baseXp = 10;
        [SerializeField] private float xpMultiplier = 1.5f;
        [SerializeField] private int xp;
        [SerializeField] private int xpToNextLevel;
        private int _maxLevel;

        public override string LocalKey => $"{guid}_xpLv";

        public int Xp => xp;
        public int XpToNextLevel => xpToNextLevel;
        public float XpProgress => xpToNextLevel > 0 ? (float)xp / xpToNextLevel : 1f;

        public Pair<int, int> LevelXpPair
        {
            get => new(CurrentLevel, xpToNextLevel);
            private set => (CurrentLevel, xpToNextLevel) = (value.Key, value.Value);
        }

        private Pair<int, int> DefaultLevelXpPair => new(1, 0);

        public void LocalLoad(string guid, int levelCap)
        {
            this.guid = guid;
            _maxLevel = levelCap;
            LevelXpPair = Data.Load(base.guid, DefaultLevelXpPair);
            SetLevel(CurrentLevel);
            xp = 0;
            CalculateXpToNextLevel();
        }

        public void Save()
        {
            Data.Save(LocalKey, this);
        }

        public void AddXp(int amount)
        {
            if (CurrentLevel >= _maxLevel) return;

            int oldXp = xp;
            xp += amount;

            while (xp >= xpToNextLevel && CurrentLevel < _maxLevel)
            {
                xp -= xpToNextLevel;
                IncreaseLevel();
                CalculateXpToNextLevel(); // Recalculate after level up
            }

            OnXpChange?.Invoke(oldXp, xp);
        }

        private void CalculateXpToNextLevel()
        {
            if (CurrentLevel >= _maxLevel)
            {
                xpToNextLevel = 0; // No more levels 
                return;
            }

            xpToNextLevel = (int)(baseXp * Math.Pow(xpMultiplier, CurrentLevel - 1));
        }

        public void ResetXp()
        {
            int oldXp = xp;
            xp = 0;
            OnXpChange?.Invoke(oldXp, xp);
        }
    }
}