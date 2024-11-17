using System;
using _Root.Scripts.Model.Farmings.Runtime;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;

namespace _Root.Scripts.Game.Farmings.Runtime
{
    public class GridMeshPlanter : MonoBehaviour, IMeshPlanter
    {
        [SerializeField] private Vector2 gridSize;
        [SerializeField] private BoxCollider boxCollider;

        private Bounds _bounds;
        private Vector3[] _points;
        private Vector2 _gridCount;

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

        [SerializeField] private Material material;
        private Mesh _mesh;
        private NativeArray<Matrix4x4> _nativeMatrices;
        private RenderParams _rp;
        private bool _isReadyForPlant;
        [SerializeField] private int subMeshIndex;

        public void Plant(Mesh mesh)
        {
            _nativeMatrices = new NativeArray<Matrix4x4>(_points.Length, Allocator.Persistent);
            _rp = new RenderParams(material);
            _mesh = mesh;

            for (var i = 0; i < _points.Length; i++)
            {
                _nativeMatrices[i] = Matrix4x4.TRS(_points[i], Quaternion.identity, Vector3.one);
            }

            _isReadyForPlant = true;
        }

        private void Update()
        {
            if (_isReadyForPlant) Graphics.RenderMeshInstanced(_rp, _mesh, subMeshIndex, _nativeMatrices);
        }

        private Vector3[] GridPointInsideBound(BoxCollider boxColliderRef, Vector3 gridCount, Bounds bounds)
        {
            // calculate points inside box collider on xz plane
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
            return new Vector2(bounds.size.x / size.x, bounds.size.z / size.y);
        }

        private void OnDrawGizmosSelected()
        {
            MakeGrid();
            if (_points == null) return;
            Gizmos.color = Color.red;
            foreach (var point in _points)
            {
                Gizmos.DrawSphere(point, 0.1f);
            }
        }

        private void OnDisable()
        {
            _nativeMatrices.Dispose();
        }


        private void Reset()
        {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }
    }
}