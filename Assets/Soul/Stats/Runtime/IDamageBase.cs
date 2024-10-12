namespace Soul.Stats.Runtime
{
    public interface IDamageBase<in TAttack, TDamage>
    {
        public bool TryKill(TAttack attack, out TDamage damage);
        public bool TryKill(float damage, out TDamage damageInfo);
    }
}