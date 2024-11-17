using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Pancake.Pools;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public class WeaponLoader : MonoBehaviour, IWeaponLoader
    {
        public List<Weapon> weapons;
        public List<WeaponComponent> activeWeapons;
        public Transform weaponParent;
        public int Count => weapons.Count;


        private void Start()
        {
            SpawnMainWeapons().Forget();
        }

        private async UniTaskVoid SpawnMainWeapons()
        {
            foreach (var weapon in weapons)
            {
                var weaponGameObject = await SharedAssetReferencePoolAsync.RequestAsync(weapon, weaponParent);
                weapon.PlaceWeapon(weaponParent, weaponGameObject.transform);
                activeWeapons.Add(weaponGameObject.GetComponent<WeaponComponent>());
            }
        }

        public void Add(Weapon weapon)
        {
            weapons.Add(weapon);
        }

        public void Remove(Weapon weapon)
        {
            weapons.Remove(weapon);
        }


        private void OnWeaponLoadComplete(AsyncOperationHandle<GameObject> handle)
        {
            GameObject weaponObject = Instantiate(handle.Result, transform);
            var weaponComponent = weaponObject.GetComponent<WeaponComponent>();
            activeWeapons.Add(weaponComponent);
        }
    }
}