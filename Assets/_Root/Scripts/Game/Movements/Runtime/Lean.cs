using System;
using UnityEngine;

namespace _Root.Scripts.Game.Movements
{
    [Serializable]
    public class Lean
    {
        public Vector3 leanAmount;
        public Transform model;
        public float boost = 10f;

        public void UpdateLean(Vector3 input)
        {
            var rotateAmount = Quaternion.Euler(
                leanAmount.x * input.magnitude,
                0,
                leanAmount.z * input.x
            );


            model.localRotation = Quaternion.Slerp(model.localRotation, rotateAmount, Time.deltaTime * boost);
        }
    }
}