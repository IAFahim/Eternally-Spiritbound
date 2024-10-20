using System.Collections.Generic;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime
{
    public class TargetingEntitiesManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> targetingEntities = new();

        public void AddTargetingEntity(GameObject entity)
        {
            if (!targetingEntities.Contains(entity))
            {
                targetingEntities.Add(entity);
            }
        }

        public void RemoveTargetingEntity(GameObject entity)
        {
            targetingEntities.Remove(entity);
        }

        public List<GameObject> GetTargetingEntities()
        {
            return new List<GameObject>(targetingEntities);
        }

        public int GetTargetingEntityCount()
        {
            return targetingEntities.Count;
        }

        public bool IsTargetedBy(GameObject entity)
        {
            return targetingEntities.Contains(entity);
        }
        
        
    }
}