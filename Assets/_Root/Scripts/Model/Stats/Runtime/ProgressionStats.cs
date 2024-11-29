using System;
using Soul.Reactives.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public struct ProgressionStats
    {
        public static readonly int[] XpToLevel =
        {
            0, 10, 25, 50, 100, 150, 250, 500, 750, 1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000,
            6500, 7000, 7500, 8000, 8500, 9000, 9500, 10000
        };

        public int MaxLevel => XpToLevel.Length;

        public Reactive<int> experience;
        public float experienceRate;
        public float luck;

        public int GetLevel()
        {
            int xp = experience.Value;
            int low = 0;
            int high = XpToLevel.Length - 1;

            while (low <= high)
            {
                int mid = low + (high - low) / 2;

                if (XpToLevel[mid] == xp)
                {
                    return mid; // Exact match found
                }
                else if (XpToLevel[mid] < xp)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            return high; // Return the highest level that the xp has surpassed.
        }

        public void AddExperience(int baseExperience)
        {
            int gainedExperience = Mathf.FloorToInt(baseExperience * experienceRate * luck);
            experience.Value += gainedExperience;
        }

        public (int currentLevel, int nextLevelExperence) GetCurrentLevelAndExperienceForNextLevel()
        {
            int currentLevel = GetLevel();
            if (currentLevel >= MaxLevel)
                return (currentLevel, XpToLevel[MaxLevel - 1]); // Already at max level, return current xp at max level

            return (currentLevel, XpToLevel[currentLevel + 1]); // Return XP needed for the next level
        }

        public float GetLevelProgress()
        {
            int currentLevel = GetLevel();
            if (currentLevel >= MaxLevel) return 1f; // Already at max level, return 1f;

            int currentLevelXp = XpToLevel[currentLevel];
            int nextLevelXp = XpToLevel[currentLevel + 1];
            return Mathf.Clamp01((float)(experience.Value - currentLevelXp) / (nextLevelXp - currentLevelXp));
        }
    }
}