namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    public interface IWeaponLoader
    {
        public void Add(Weapon weapon);
        public int Count { get; }
    }
}