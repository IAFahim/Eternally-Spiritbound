using System;

namespace Soul2.Levels.Runtime.Interface
{
    public interface IXpLevelSystem
    {
        int Xp { get; }
        int XpToNextLevel { get; }
        float XpProgress { get; }
        event Action<int, int> OnXpChange;
        void AddXp(int amount);
        void ResetXp();
    }
}