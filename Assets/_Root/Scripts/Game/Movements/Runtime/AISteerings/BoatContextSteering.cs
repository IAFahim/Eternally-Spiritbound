using Soul.OverlapSugar.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Movements.Runtime.AISteerings
{
    public class BoatContextSteering : MonoBehaviour
    {
        [Header("Steering Settings")]
        [SerializeField] private float detectRadius = 10f;
        [SerializeField] private float avoidanceWeight = 2f;
        [SerializeField] private float seekWeight = 1f;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private Transform playerTransform;
    
        [Header("Context Settings")]
        [SerializeField] private int directions = 16; // Increased for smoother steering
        [SerializeField] private float dangerDecayDistance = 5f;
    
        [Header("Debug")]
        [SerializeField] private bool showDebugVisuals = true;
        [SerializeField] private float debugRayLength = 2f;
    
        private OverlapNonAlloc _obstacleDetector;
        private float[] _interestArray;
        private float[] _dangerArray;
        private Vector3[] _directionVectors;
    
        public Vector3 _resultDirection;
        public Vector3 _lastNonZeroDirection;
        public IMove move;

        private void Start()
        {
            InitializeArrays();
            move = GetComponent<IMove>();
            InitializeObstacleDetector();
        }

        private void InitializeArrays()
        {
            _interestArray = new float[directions];
            _dangerArray = new float[directions];
            _directionVectors = new Vector3[directions];
        
            for (int i = 0; i < directions; i++)
            {
                float angle = i * (360f / directions);
                _directionVectors[i] = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            }
        }

        private void InitializeObstacleDetector()
        {
            _obstacleDetector = new OverlapNonAlloc
            {
                overlapType = OverlapType.Sphere,
                sphereRadius = detectRadius,
                searchMask = obstacleLayer
            };
            _obstacleDetector.SetOverlapPoint(transform);
            _obstacleDetector.Init(20);
        }

        public Vector3 GetSteeringDirection()
        {
            ClearArrays();
            DetectObstacles();
            CalculateInterest();
        
            return CalculateResultDirection();
        }

        private void ClearArrays()
        {
            for (int i = 0; i < directions; i++)
            {
                _interestArray[i] = 0;
                _dangerArray[i] = 0;
            }
        }

        private void DetectObstacles()
        {
            _obstacleDetector.PerformOverlap();
        
            if (_obstacleDetector.Found())
            {
                for (int i = 0; i < _obstacleDetector.foundSize; i++)
                {
                    ProcessObstacle(_obstacleDetector.Colliders[i]);
                }
            }
        }

        private void ProcessObstacle(Collider obstacle)
        {
            Vector3 toObstacle = obstacle.ClosestPoint(transform.position) - transform.position;
            toObstacle.y = 0; // Keep navigation on XZ plane
            float distance = toObstacle.magnitude;
        
            if (distance < 0.1f) return; // Prevent division by zero
        
            Vector3 directionToObstacle = toObstacle / distance;
            float dangerWeight = Mathf.Clamp01(1 - (distance / dangerDecayDistance));
        
            for (int i = 0; i < directions; i++)
            {
                float dot = Vector3.Dot(directionToObstacle, _directionVectors[i]);
                if (dot > 0)
                {
                    float danger = dot * dangerWeight * avoidanceWeight;
                    _dangerArray[i] = Mathf.Max(_dangerArray[i], danger);
                }
            }
        }

        private void CalculateInterest()
        {
            if (playerTransform == null) return;

            Vector3 toTarget = playerTransform.position - transform.position;
            toTarget.y = 0;
        
            if (toTarget.magnitude < 0.1f) return;
        
            Vector3 directionToTarget = toTarget.normalized;
        
            for (int i = 0; i < directions; i++)
            {
                float dot = Vector3.Dot(directionToTarget, _directionVectors[i]);
                _interestArray[i] = Mathf.Max(0, dot * seekWeight);
            }
        }

        private Vector3 CalculateResultDirection()
        {
            Vector3 resultDirection = Vector3.zero;
            float maxValue = float.MinValue;
        
            for (int i = 0; i < directions; i++)
            {
                float value = _interestArray[i] - _dangerArray[i];
            
                if (value > maxValue)
                {
                    maxValue = value;
                    resultDirection = _directionVectors[i];
                }
            }
        
            _resultDirection = resultDirection;
            if (_resultDirection != Vector3.zero)
            {
                _lastNonZeroDirection = _resultDirection;
            }
            var direction= _resultDirection != Vector3.zero ? _resultDirection : _lastNonZeroDirection;
            move.Direction = direction;
            return _resultDirection;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !showDebugVisuals) return;

            // Draw direction vectors with color intensity based on interest/danger
            for (int i = 0; i < directions; i++)
            {
                float interest = _interestArray[i];
                float danger = _dangerArray[i];
            
                // Green for interest, Red for danger
                Gizmos.color = new Color(danger, interest, 0, 0.5f);
                Gizmos.DrawRay(transform.position, _directionVectors[i] * debugRayLength);
            }

            // Draw result direction
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, _resultDirection * debugRayLength * 1.2f);
        
            // Use the provided DrawGizmos from OverlapNonAlloc
            _obstacleDetector?.DrawGizmos(Color.yellow, Color.red);
        }
    }
}