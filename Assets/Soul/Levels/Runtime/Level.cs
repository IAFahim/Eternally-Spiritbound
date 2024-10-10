using System;
using UnityEngine;

namespace Soul.Levels.Runtime
{
    [Serializable]
    public class Level
    {
        [SerializeField] protected int currentLevel = 1;
        public int CurrentLevel => currentLevel;
        
        public event Action<int, int> OnLevelChange;
        
        public void SetLevel(int dataNew)
        {
            if (dataNew <= 0) throw new ArgumentOutOfRangeException(nameof(dataNew), "Level must be greater than 0.");
            if (CurrentLevel == dataNew) return;
            
            int oldLevel = CurrentLevel;
            currentLevel = dataNew;
            OnLevelChange?.Invoke(oldLevel, CurrentLevel);
        }
        
        public void IncreaseLevel() => SetLevel(CurrentLevel + 1);
        
        public void DecreaseLevel()
        {
            if (CurrentLevel > 1)
            {
                SetLevel(CurrentLevel - 1);
            }
        }
    }
}