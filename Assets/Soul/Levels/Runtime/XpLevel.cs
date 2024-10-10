using System;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace Soul.Levels.Runtime
{
    [Serializable]
    public class XpLevel : Level
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
        
        public void SetXpLevel(Pair<int, int> dataNew)
        {
            if (currentLevel >= maxLevel) return;
            xp = dataNew.Second;
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
    }
}