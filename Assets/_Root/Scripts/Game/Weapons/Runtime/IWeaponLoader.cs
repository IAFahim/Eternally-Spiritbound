namespace _Root.Scripts.Game.Weapons.Runtime
{
    public interface IWeaponLoader
    {
        public void Add(WeaponAsset weaponAsset);
        public void Remove(WeaponAsset weaponAsset);
        public int Count { get; }
    }
}