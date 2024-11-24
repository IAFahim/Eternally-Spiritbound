using System.Collections.Generic;
using _Root.Scripts.Model.Stats.Runtime;
using Cysharp.Threading.Tasks;
using Pancake.Pools;
using Sirenix.OdinInspector;
using Soul.Interactables.Runtime;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    [RequireComponent(typeof(EntityStatsComponent))]
    public class WeaponLoader : MonoBehaviour, IWeaponLoader
    {
        public Transform weaponParent;
        [DisableInEditorMode] public List<Pair<int, IWeapon>> activeWeapons;

        [SerializeField] private EntityStatsComponent entityStatsComponent;
        [SerializeField] private int startingLevel = 0;
        [SerializeField] private List<WeaponAsset> weapons;

        private OffensiveStats _offensiveStats;
        private IFocus _focusReference;

        public int Count => weapons.Count;

        private void Awake()
        {
            entityStatsComponent ??= GetComponent<EntityStatsComponent>();
            _focusReference = GetComponent<IFocus>();
        }

        private async void OnEnable()
        {
            await SpawnMainWeapons();
            entityStatsComponent.Register(OnEntityStatsChange, OnOldEntityStatsCleanUp);
        }


        private void OnEntityStatsChange()
        {
            _offensiveStats = entityStatsComponent.entityStats.offensive;
            foreach (var activeWeapon in activeWeapons)
            {
                activeWeapon.Value.Init(entityStatsComponent, _focusReference, activeWeapon.Key, _offensiveStats);
            }
        }

        private void OnOldEntityStatsCleanUp()
        {
        }

        private async UniTask SpawnMainWeapons()
        {
            foreach (var weapon in weapons)
            {
                var weaponGameObject = await SharedAssetReferencePoolAsync.RequestAsync(weapon, weaponParent);
                weapon.PlaceWeapon(weaponParent, weaponGameObject.transform);
                activeWeapons.Add(new Pair<int, IWeapon>(startingLevel, weaponGameObject.GetComponent<IWeapon>()));
            }
        }

        public void Add(WeaponAsset weaponAsset)
        {
            weapons.Add(weaponAsset);
        }

        public void Remove(WeaponAsset weaponAsset)
        {
            weapons.Remove(weaponAsset);
        }

        private void OnDisable()
        {
            foreach (var activeWeapon in activeWeapons)
            {
                SharedAssetReferencePoolAsync.Return(activeWeapon.Value.WeaponAsset, activeWeapon.Value.GameObject);
            }
        }

        private void Reset()
        {
            entityStatsComponent = GetComponent<EntityStatsComponent>();
        }
    }
}