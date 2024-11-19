using UnityEngine;

namespace _Root.Scripts.Model.ObjectPlacers.Runtime
{
    public interface IMeshPlanter
    {
        public void Plant(Mesh mesh);
        void MoveSingle(Vector2Int gridIndex, Vector3 position);
        void MoveSlice();
        public (Vector2Int gridIndex, Vector3 position) GetClosestPoint(Vector3 position);
        void Clear();
    }
}