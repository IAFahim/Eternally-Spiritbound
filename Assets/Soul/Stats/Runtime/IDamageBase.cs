namespace Soul.Stats.Runtime
{
    public interface IDamageBase<in TAttackInfo, TDamageInfo>
    {
        public bool TryDamage(TAttackInfo info, out TDamageInfo damageInfo);
    }
}