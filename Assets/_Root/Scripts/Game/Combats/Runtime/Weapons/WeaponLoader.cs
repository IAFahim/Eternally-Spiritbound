using System.Collections.Generic;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    public class WeaponLoader : MonoBehaviour, IWeaponLoader
    {
        public List<Weapon> weapons;
        public int Count => weapons.Count;
        public void Add(Weapon weapon)
        {
            weapons.Add(weapon);
        }

        private void Start()
        {
            
        }
    }
}