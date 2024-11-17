using System;
using _Root.Scripts.Game.MeshRenders.Runtime;
using _Root.Scripts.Model.Farmings.Runtime;
using UnityEngine;
using Sirenix.OdinInspector;

namespace _Root.Scripts.Game.Farmings.Runtime
{
    public class GridMeshPlanter : MonoBehaviour, IMeshPlanter
    {
        [SerializeField] private Vector2 gridSize = new(1f, 1f);
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private Material material;
        [SerializeField] private int subMeshIndex;
        [SerializeField] private Vector3 plantScale = Vector3.one;

        private Bounds _bounds;
        private Vector3[] _points;
        private Vector2 _gridCount;
        private int _instanceId = int.MinValue;
        private Mesh _mesh;

        private void Awake()
        {
            MakeGrid();
        }

        [Button]
        private void MakeGrid()
        {
            _bounds = boxCollider.bounds;
            _gridCount = CalculateGridSize(gridSize, _bounds);
            _points = GridPointInsideBound(boxCollider, _gridCount, _bounds);
        }

        public void Plant(Mesh mesh)
        {
            if (mesh == null) throw new System.ArgumentNullException(nameof(mesh));
            if(_instanceId > -1) RemovePlant(mesh);
            if (_points == null || _points.Length == 0)
                throw new System.InvalidOperationException("Grid points are not initialized.");
            _instanceId = MeshRenderDictionary.AddToRender(mesh, material, subMeshIndex, _points, plantScale);
            _mesh = mesh;
        }

        [Button]
        public void RemovePlant()
        {
            if (_instanceId > -1) RemovePlant(_mesh);
        }

        public void RemovePlant(Mesh mesh)
        {
            if (_instanceId == -1) return;

            MeshRenderDictionary.RemoveFromRender(mesh, _instanceId);
            _instanceId = -1;
        }

        private Vector3[] GridPointInsideBound(BoxCollider boxColliderRef, Vector2 gridCount, Bounds bounds)
        {
            var points = new Vector3[(int)(gridCount.x * gridCount.y)];
            var xStep = bounds.size.x / gridCount.x;
            var zStep = bounds.size.z / gridCount.y;
            var xStart = bounds.min.x + xStep / 2;
            var zStart = bounds.min.z + zStep / 2;
            var gridCountX = (int)gridCount.x;
            var gridCountZ = (int)gridCount.y;

            var index = 0;
            for (var x = 0; x < gridCountX; x++)
            {
                for (var z = 0; z < gridCountZ; z++)
                {
                    points[index] = new Vector3(xStart + x * xStep, bounds.center.y, zStart + z * zStep);
                    index++;
                }
            }

            return points;
        }

        private Vector2 CalculateGridSize(Vector2 size, Bounds bounds)
        {
            return new Vector2(Mathf.Floor(bounds.size.x / size.x), Mathf.Floor(bounds.size.z / size.y));
        }

        private void OnDrawGizmosSelected()
        {
            if (!boxCollider) return;

            if (_points == null || _bounds != boxCollider.bounds)
            {
                MakeGrid();
            }

            if (_points == null) return;

            Gizmos.color = Color.red;
            foreach (var point in _points)
            {
                Gizmos.DrawWireSphere(point, gridSize.x * 0.1f);
            }
        }

        private void OnDisable()
        {
            RemovePlant();
        }

        private void Reset()
        {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }
    }
}