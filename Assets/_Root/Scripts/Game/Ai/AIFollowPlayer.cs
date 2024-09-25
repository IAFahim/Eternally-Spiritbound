using _Root.Scripts.Game.Movements;
using UnityEngine;

public class AIFollowPlayer : MonoBehaviour
{
    [Header("Target and Vision")] public Transform player; // Target to follow
    public float visionRange = 20f;
    public float fieldOfView = 90f;

    [Header("Movement")] public float moveSpeed = 5f;
    public float rotationSpeed = 5f;

    [Header("Obstacle Avoidance")] public float obstacleAvoidanceDistance = 5f;
    public int numberOfRays = 3;
    public float rayAngleSpread = 30f;

    private Vector3 lastSeenPosition;
    public Vector3 desiredDirection; // The main direction vector

    private IMove _move;
    private IAcceleration _acceleration;

    private void OnEnable()
    {
        _move = GetComponent<IMove>();
        _acceleration = GetComponent<IAcceleration>();
        lastSeenPosition = transform.position;
    }

    private void Update()
    {
        // 1. Find Player (Raycast)
        if (CanSeePlayer())
        {
            lastSeenPosition = player.position;
            desiredDirection = (player.position - transform.position).normalized;
            _acceleration.Acceleration = 1f;
        }
        else
        {
            _acceleration.Acceleration = 0f;
            desiredDirection = (lastSeenPosition - transform.position).normalized;
        }

        // 2. Obstacle Avoidance (Raycast)
        desiredDirection = AvoidObstacles(desiredDirection);

        // 3. Movement 
        MoveTowardsDesiredDirection();
    }


    // --- Helper Methods ---

    private bool CanSeePlayer()
    {
        if (player == null) return false;
        Vector3 toPlayer = player.position - transform.position;
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer > visionRange) return false;

        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);
        if (angleToPlayer > fieldOfView / 2f) return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, toPlayer, out hit, visionRange))
        {
            return hit.transform == player;
        }

        return false;
    }

    private Vector3 AvoidObstacles(Vector3 direction)
    {
        Vector3 avoidanceDirection = Vector3.zero;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = -rayAngleSpread / 2f + i * (rayAngleSpread / (numberOfRays - 1));
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * direction;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, obstacleAvoidanceDistance))
            {
                float distanceToObstacle = hit.distance;
                float avoidanceStrength = Mathf.Clamp01(1f - distanceToObstacle / obstacleAvoidanceDistance);

                avoidanceDirection += -rayDirection * avoidanceStrength; // Steer away

                Debug.DrawLine(transform.position, hit.point, Color.red); // Obstacle Gizmo
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + rayDirection * obstacleAvoidanceDistance,
                    Color.green); // Clear Path Gizmo
            }
        }

        return (direction + avoidanceDirection).normalized;
    }

    private void MoveTowardsDesiredDirection()
    {
        // Quaternion targetRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
        // _move.Direction = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime)
        // .eulerAngles;

        _move.Direction = desiredDirection;
        // transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void DrawDebugGizmos()
    {
        // Draw the main desired direction vector
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + desiredDirection * 2f);
    }
}