namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public interface IWeaponLoader
    {
        public void Add(Weapon weapon);
        public int Count { get; }
    }
}