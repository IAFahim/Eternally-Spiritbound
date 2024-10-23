using _Root.Scripts.Game.FocusProvider.Runtime;
using _Root.Scripts.Game.Inputs.Runtime;
using Pancake.Common;
using Soul.Tickers.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Movements.Runtime.AISteerings
{
    public class TargetBoatContextSteering : MonoBehaviour, IFocusConsumer
    {
        [FormerlySerializedAs("mainGameProviders")] [SerializeField]
        private FocusScriptable mainGameStacks;

        private BoatContextSteering _steering;
        private IMove _move;
        public IntervalTicker ticker;

        private void Start()
        {
            _steering = GetComponent<BoatContextSteering>();
            if (!IsFocused) AddListener();
        }

        private void AddListener()
        {
            App.AddListener(EUpdateMode.Update, OnUpdate);
            App.AddListener(EUpdateMode.FixedUpdate, OnFixedUpdate);
        }

        private void RemoveListener()
        {
            App.RemoveListener(EUpdateMode.Update, OnUpdate);
            App.RemoveListener(EUpdateMode.FixedUpdate, OnFixedUpdate);
        }

        private void OnUpdate()
        {
            _move.Direction = _steering.Steer(mainGameStacks.mainObject.transform.position);
        }

        private void OnFixedUpdate()
        {
            if (ticker.TryTick()) _steering.FixedUpdate();
        }

        public bool IsFocused { get; private set; }

        public void SetFocus(FocusReferences focusReferences)
        {
            IsFocused = true;
            AddListener();
        }

        public void OnFocusLost(GameObject targetGameObject)
        {
            IsFocused = false;
            RemoveListener();
        }
    }
}