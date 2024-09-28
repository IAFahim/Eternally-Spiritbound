using UnityEngine;

namespace _Root.Scripts.Game.Items.Runtime
{
    public class PickUpDropStrategy : ScriptableObject
    {
        public float range = 5f;
        public bool autoPickup = true;

        public Vector3 Drop(Vector3 position)
        {
            var randomV2 = Random.insideUnitCircle * range;
            return position + new Vector3(randomV2.x, 0, randomV2.y);
        }
    }
}