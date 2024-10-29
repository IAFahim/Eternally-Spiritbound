using _Root.Scripts.Game.Ai.Runtime.Targets;
using _Root.Scripts.Game.Inputs.Runtime;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.Movements.Runtime.AISteerings;
using Pancake.Common;
using Soul.Tickers.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Presentation.Vehicles.Runtime
{
    public class TargetBoatContextSteering : MonoBehaviour
    {
        [FormerlySerializedAs("mainGameProviders")] [SerializeField]
        private FocusManagerScript mainGameStacks;

        private BoatContextSteering _steering;
        private IMove _move;
        public IntervalTicker ticker;

        private ITargeter _targeter;

        private void Awake()
        {
            _move = GetComponent<IMove>();
            _steering = GetComponent<BoatContextSteering>();
            _targeter = GetComponent<ITargeter>();
        }

        private void OnEnable()
        {
            _targeter.OnTargetFound += OnTargetFound;
            _targeter.OnTargetLost += OnTargetLost;
        }

        private void OnDisable()
        {
            RemoveUpdateListener();
            _targeter.OnTargetFound -= OnTargetFound;
            _targeter.OnTargetLost -= OnTargetLost;
        }


        private void OnUpdate()
        {
            _move.Direction = _steering.Steer(mainGameStacks.mainObject.transform.position);
        }

        private void OnFixedUpdate()
        {
            if (ticker.TryTick()) _steering.FixedUpdate();
        }
        
        public void OnTargetFound(ITargetable targetable) => AddUpdateListener();
        public void OnTargetLost(ITargetable targetable, bool onDisable) => RemoveUpdateListener();

        private void AddUpdateListener()
        {
            App.AddListener(EUpdateMode.Update, OnUpdate);
            App.AddListener(EUpdateMode.FixedUpdate, OnFixedUpdate);
        }

        private void RemoveUpdateListener()
        {
            App.RemoveListener(EUpdateMode.Update, OnUpdate);
            App.RemoveListener(EUpdateMode.FixedUpdate, OnFixedUpdate);
        }
    }
}