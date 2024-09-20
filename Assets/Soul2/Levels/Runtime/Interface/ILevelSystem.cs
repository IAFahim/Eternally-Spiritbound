using System;

namespace Soul2.Levels.Runtime.Interface
{
    public interface ILevelSystem
    {
        int CurrentLevel { get; }
        event Action<int, int> OnLevelChange;
        void SetLevel(int level);
        void IncreaseLevel();
        void DecreaseLevel();
    }
}