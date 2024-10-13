﻿using System;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Movements.Runtime.AISteerings
{
    public class TargetBoatContextSteering : MonoBehaviour
    {
        [SerializeField] private MainObjectProviderScriptable mainGameObjectProviders;
        private BoatContextSteering _steering;
        private IMove _move;

        private void Start()
        {
            _steering = GetComponent<BoatContextSteering>();
            _move = GetComponent<IMove>();
        }

        private void Update()
        {
            _move.Direction = _steering.Steer(mainGameObjectProviders.mainGameObjectInstance.transform.position);
        }

        public void FixedUpdate()
        {
            _steering.FixedUpdate();
        }

        private void OnCollisionEnter(Collision other)
        {
            gameObject.SetActive(false);
        }
    }
}