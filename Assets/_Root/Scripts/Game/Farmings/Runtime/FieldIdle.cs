using System;
using _Root.Scripts.Model.Farmings.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Farmings.Runtime
{
    [Serializable]
    public class FieldIdle : CropState
    {
        public FieldIdle(CropData cropData) : base(cropData)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Field is idle");
        }

        public override void OnUpdate()
        {
            Debug.Log("Field idleing");
        }

        public override void OnExit()
        {
            Debug.Log("Field not Idle");
        }
    }
}