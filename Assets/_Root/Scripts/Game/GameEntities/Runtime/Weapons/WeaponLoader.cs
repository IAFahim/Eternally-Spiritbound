using System.Collections.Generic;
using _Root.Scripts.Game.Combats.Runtime.Weapons;
using _Root.Scripts.Game.Stats.Runtime.Model;
using Soul.Modifiers.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public class WeaponLoader : MonoBehaviour, IWeaponLoader
    {
        public List<Weapon> weapons;
        public List<WeaponComponent> activeWeapons;
        public OffensiveStats<Modifier> offensiveStats;
        public int Count => weapons.Count;
        public void Add(Weapon weapon)
        {
            weapons.Add(weapon);
        }

        private void Start()
        {
            foreach (var weapon in weapons)
            {
                Addressables.LoadAssetAsync<GameObject>(weapon.assetReferenceGameObject).Completed += OnWeaponLoadComplete;
            }
        }

        private void OnWeaponLoadComplete(AsyncOperationHandle<GameObject> handle)
        {
            GameObject weaponObject = Instantiate(handle.Result, transform);
            var weaponComponent = weaponObject.GetComponent<WeaponComponent>();
            weaponComponent.Init(offensiveStats);
            activeWeapons.Add(weaponComponent);
        }
    }
}