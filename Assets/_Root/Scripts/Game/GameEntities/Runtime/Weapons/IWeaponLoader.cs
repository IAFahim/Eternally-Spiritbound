namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public interface IWeaponLoader
    {
        public void Add(WeaponAsset weaponAsset);
        public void Remove(WeaponAsset weaponAsset);
        public int Count { get; }
    }
}