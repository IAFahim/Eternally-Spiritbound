using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Items.Runtime.Storage;
using _Root.Scripts.Game.Stats.Runtime.Controller;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    public class WeaponHandler : MonoBehaviour
    {
        public List<Weapon> weapon;
        private IEntityStats _entityStats;
        private IGameItemStorageReference gameItemStorageReference;
        private void Awake()
        {
            _entityStats = GetComponent<IEntityStats>();
            gameItemStorageReference = GetComponent<IGameItemStorageReference>();
        }

        private void OnEnable()
        {
        }
    }
}