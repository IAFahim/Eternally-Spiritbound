using UnityEngine;

namespace _Root.Scripts.Game.Movements
{
    [RequireComponent(typeof(Rigidbody))]
    public class BoatBuoyancy : MonoBehaviour
    {
        public float density = 1000f; // Density of water (kg/m³)
        public int buoyancyPointsCount = 5; // Number of points to calculate buoyancy from
        public float waveHeight = 0.5f; // Height of simulated waves
        public float waveFrequency = 1f; // Frequency of simulated waves

        private Rigidbody rb;
        private Vector3[] buoyancyPoints;

        void Start()
        {
            rb = GetComponent<Rigidbody>();

            // Calculate buoyancy points based on boat's bounds
            buoyancyPoints = new Vector3[buoyancyPointsCount];
            CalculateBuoyancyPoints();
        }

        void FixedUpdate()
        {
            ApplyBuoyancy();
        }

        void ApplyBuoyancy()
        {
            for (int i = 0; i < buoyancyPoints.Length; i++)
            {
                Vector3 point = buoyancyPoints[i] + transform.position;

                // Simulate simple wave motion
                float waterLevel = transform.position.y + waveHeight * Mathf.Sin(Time.time * waveFrequency + i);

                if (point.y < waterLevel)
                {
                    float submergedDepth = waterLevel - point.y;
                    float buoyancyForceMagnitude = submergedDepth * Physics.gravity.magnitude * density;

                    Vector3 buoyancyForce = buoyancyForceMagnitude * Vector3.up;
                    rb.AddForceAtPosition(buoyancyForce, point);
                }
            }
        }

        void CalculateBuoyancyPoints()
        {
            Bounds bounds = GetComponent<Collider>().bounds;

            // Distribute buoyancy points evenly along the bottom of the boat
            for (int i = 0; i < buoyancyPointsCount; i++)
            {
                float x = Mathf.Lerp(bounds.min.x, bounds.max.x, (float)i / (buoyancyPointsCount - 1));
                buoyancyPoints[i] = new Vector3(x, bounds.min.y, 0f);
            }
        }
    }
}