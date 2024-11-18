using System.Collections.Generic;
using _Root.Scripts.Model.Stats.Runtime;
using Cysharp.Threading.Tasks;
using Pancake.Pools;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    [RequireComponent(typeof(EntityStatsComponent))]
    public class WeaponLoader : MonoBehaviour, IWeaponLoader
    {
        public EntityStatsComponent entityStatsComponent;
        public List<Weapon> weapons;
        public List<WeaponComponent> activeWeapons;
        public Transform weaponParent;
        public int Count => weapons.Count;
        
        private OffensiveStats _offensiveStats; 

        private void OnEnable()
        {
            entityStatsComponent.Register(OnEntityStatsChange, OnOldEntityStatsCleanUp);
        }
        

        private void OnEntityStatsChange()
        {
            
        }
        
        private void OnOldEntityStatsCleanUp()
        {
            
        }

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

        private void Reset()
        {
            entityStatsComponent = GetComponent<EntityStatsComponent>();
        }
    }
}