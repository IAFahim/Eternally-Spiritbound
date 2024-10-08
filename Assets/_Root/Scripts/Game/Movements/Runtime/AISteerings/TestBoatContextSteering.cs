using System;
using UnityEngine;

namespace _Root.Scripts.Game.Movements.Runtime.AISteerings
{
    public class TestBoatContextSteering : MonoBehaviour
    {
        private BoatContextSteering _steering;
        public Vector2 steeringDirection;

        private void Start()
        {
            _steering = GetComponent<BoatContextSteering>();
        }

        private void Update()
        {
            steeringDirection = _steering.GetSteeringDirection();
        }


        private void OnCollisionEnter(Collision other)
        {
            gameObject.SetActive(false);
        }
    }
}