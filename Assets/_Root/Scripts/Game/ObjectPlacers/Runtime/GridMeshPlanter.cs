using _Root.Scripts.Game.MeshRenders.Runtime;
using _Root.Scripts.Model.ObjectPlacers.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.ObjectPlacers.Runtime
{
    public class GridMeshPlanter : MonoBehaviour, IMeshPlanter
    {
        [SerializeField] private Vector2 gridSize = new(1f, 1f);
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private Material material;
        [SerializeField] private int subMeshIndex;
        [SerializeField] private Vector3 plantScale = Vector3.one;

        private Bounds _bounds;
        private Vector3[,] _gridPoints; // 2D array for better grid representation
        private Vector2 _gridCount;
        private int _instanceId = int.MinValue;
        private Mesh _mesh;
        private float _xStep;
        private float _zStep;
        private Vector3 _gridOrigin; // Store grid origin for quick calculations

        private void Awake()
        {
            MakeGrid();
        }

        [Button]
        private void MakeGrid()
        {
            _bounds = boxCollider.bounds;
            _gridCount = CalculateGridSize(gridSize, _bounds);
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            var gridCountX = (int)_gridCount.x;
            var gridCountZ = (int)_gridCount.y;
            _gridPoints = new Vector3[gridCountX, gridCountZ];

            _xStep = _bounds.size.x / _gridCount.x;
            _zStep = _bounds.size.z / _gridCount.y;

            // Calculate grid origin (bottom-left corner)
            var xStart = _bounds.min.x + _xStep / 2;
            var zStart = _bounds.min.z + _zStep / 2;
            _gridOrigin = new Vector3(xStart, _bounds.center.y, zStart);

            // Fill the 2D grid
            for (var x = 0; x < gridCountX; x++)
            {
                for (var z = 0; z < gridCountZ; z++)
                {
                    _gridPoints[x, z] = new Vector3(
                        _gridOrigin.x + x * _xStep,
                        _bounds.center.y,
                        _gridOrigin.z + z * _zStep
                    );
                }
            }
        }

        public void Plant(Mesh mesh)
        {
            if (mesh == null) throw new System.ArgumentNullException(nameof(mesh));
            if (_instanceId > -1) RemovePlant(mesh);
            if (_gridPoints == null)
                throw new System.InvalidOperationException("Grid points are not initialized.");

            // Convert 2D array to 1D for the renderer
            var flattenedPoints = FlattenGridPoints();
            _instanceId = MeshRenderDictionary.AddToRender(mesh, material, subMeshIndex, flattenedPoints, plantScale);
            _mesh = mesh;
        }

        private Vector3[] FlattenGridPoints()
        {
            var rows = _gridPoints.GetLength(0);
            var cols = _gridPoints.GetLength(1);
            var flattenedPoints = new Vector3[rows * cols];

            for (var x = 0; x < rows; x++)
            for (var z = 0; z < cols; z++)
                flattenedPoints[x * cols + z] = _gridPoints[x, z];

            return flattenedPoints;
        }

        public void MoveSingle(Vector2Int gridIndex, Vector3 position)
        {
            if (_instanceId == -1) return;

            // Convert flat index to grid coordinates
            var x = gridIndex.x;
            var z = gridIndex.y;

            var flattenedPoints = FlattenGridPoints();
            flattenedPoints[x * _gridPoints.GetLength(1) + z] = position;

            // Update the 2D grid as well
            _gridPoints[x, z] = position;

            MeshRenderDictionary.Move(_mesh, _instanceId, flattenedPoints);
        }

        public void MoveSlice()
        {
            if (_instanceId == -1) return;
            var height = transform.position.y + 2;

            // Update all points in the grid
            var rows = _gridPoints.GetLength(0);
            var cols = _gridPoints.GetLength(1);

            for (var x = 0; x < rows; x++)
            {
                for (var z = 0; z < cols; z++)
                {
                    var point = _gridPoints[x, z];
                    _gridPoints[x, z] = new Vector3(point.x, height, point.z);
                }
            }

            MeshRenderDictionary.Move(_mesh, _instanceId, FlattenGridPoints());
        }

        public (Vector2Int gridIndex, Vector3 position) GetClosestPoint(Vector3 position)
        {
            var gridIndex = GetClosestPointGridIndex(position);
            return (gridIndex, _gridPoints[gridIndex.x, gridIndex.y]);
        }

        private Vector2Int GetClosestPointGridIndex(Vector3 position)
        {
            if (_gridPoints == null) return Vector2Int.zero;

            // Calculate the theoretical grid coordinates
            var relativePos = position - _gridOrigin;
            var gridX = Mathf.Clamp(Mathf.RoundToInt(relativePos.x / _xStep), 0, (int)_gridCount.x - 1);
            var gridZ = Mathf.Clamp(Mathf.RoundToInt(relativePos.z / _zStep), 0, (int)_gridCount.y - 1);
            return new Vector2Int(gridX, gridZ);
        }

        private Vector2Int GetGridIndex(int index)
        {
            var cols = _gridPoints.GetLength(1);
            return new Vector2Int(index / cols, index % cols);
        }

        [Button]
        public void Clear()
        {
            if (_instanceId > -1) RemovePlant(_mesh);
        }

        public void RemovePlant(Mesh mesh)
        {
            if (_instanceId == -1) return;

            MeshRenderDictionary.RemoveFromRender(mesh, _instanceId);
            _instanceId = -1;
        }

        private Vector2 CalculateGridSize(Vector2 size, Bounds bounds)
        {
            return new Vector2(Mathf.Floor(bounds.size.x / size.x), Mathf.Floor(bounds.size.z / size.y));
        }

        private void OnDrawGizmosSelected()
        {
            if (!boxCollider) return;

            if (_gridPoints == null || _bounds != boxCollider.bounds)
            {
                MakeGrid();
            }

            if (_gridPoints == null) return;

            Gizmos.color = Color.red;
            var rows = _gridPoints.GetLength(0);
            var cols = _gridPoints.GetLength(1);

            for (var x = 0; x < rows; x++)
            {
                for (var z = 0; z < cols; z++)
                {
                    Gizmos.DrawWireSphere(_gridPoints[x, z], gridSize.x * 0.1f);
                }
            }
        }

        private void OnDisable()
        {
            Clear();
        }

        private void Reset()
        {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }
    }
}