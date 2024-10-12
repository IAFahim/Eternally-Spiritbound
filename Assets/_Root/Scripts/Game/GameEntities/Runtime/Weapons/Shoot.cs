using _Root.Scripts.Game.GameEntities.Runtime.Weapons;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    public class Shoot : MonoBehaviour
    {
        [FormerlySerializedAs("weaponComponenet")] [FormerlySerializedAs("Weapon")] public WeaponComponent weaponComponent;
        
        void Update()
        {
            
        }
    }
}