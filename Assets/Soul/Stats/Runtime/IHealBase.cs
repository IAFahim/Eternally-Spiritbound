namespace Soul.Stats.Runtime
{
    public interface IHealBase<in T>
    {
        bool TryHeal(T type, float amount, out float healAmount);
    }
}