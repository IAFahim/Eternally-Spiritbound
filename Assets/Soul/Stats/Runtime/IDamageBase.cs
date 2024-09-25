namespace Soul.Stats.Runtime
{
    public interface IDamageBase<in TAttackInfo, in TDamageInput, TDamageInfo>
    {
        public bool TryDamage(TAttackInfo info, TDamageInput damageInfo, out TDamageInfo damageTaken);
    }
}