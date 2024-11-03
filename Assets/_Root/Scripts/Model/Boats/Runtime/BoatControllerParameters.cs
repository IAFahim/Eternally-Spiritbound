using System;
using UnityEngine;

namespace _Root.Scripts.Model.Boats.Runtime
{
    [Serializable]
    public struct BoatControllerParameters
    {
        [Header("Movement Settings")] public float maxForwardSpeed;
        public float maxReverseSpeed;
        public float turnTorque;
        public float accelerationForce;
        public float reverseAccelerationForce;

        [Header("Stability Settings")] public float stabilizationTorque;
        public float dampingFactor;

        public BoatControllerParameters(BoatControllerParameters parameters)
        {
            maxForwardSpeed = parameters.maxForwardSpeed;
            maxReverseSpeed = parameters.maxReverseSpeed;
            turnTorque = parameters.turnTorque;
            accelerationForce = parameters.accelerationForce;
            reverseAccelerationForce = parameters.reverseAccelerationForce;
            stabilizationTorque = parameters.stabilizationTorque;
            dampingFactor = parameters.dampingFactor;
        }
    }
}