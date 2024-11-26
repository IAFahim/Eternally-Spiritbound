using UnityEngine;

namespace _Root.Scripts.Game.Utils.Runtime
{
    public static class GameMath
    {
        /// <summary>
        /// Applies a chance-based rate to the current Value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chance">The probability of applying the bonus rate. If >= 1, it's treated as a guaranteed rate.</param>
        /// <param name="rate">The rate to apply if the chance check succeeds.</param>
        /// <param name="afterMultiplier">The result of Value multiplied by the determined rate.</param>
        /// <returns>True if the rate was applied, false otherwise.</returns>
        public static bool ApplyChanceMultiplier(
            float value,     // Base value (like damage)
            float chance,    // Probability of critical hit (0-1)
            float rate,      // Additional multiplier (like 0.5 for 150% damage)
            out float afterMultiplier)
        {
            float chanceMultiplier;

            // If chance is 100% or greater, use chance as multiplier
            if (chance >= 1f) 
                chanceMultiplier = chance;
            // If random roll succeeds, apply the bonus rate + 1
            else if (Random.value < chance) 
                chanceMultiplier = rate + 1;
            // No crit, no multiplier
            else 
                chanceMultiplier = 1f;

            afterMultiplier = value * chanceMultiplier;
            return chanceMultiplier > 1f;
        }
    }
}