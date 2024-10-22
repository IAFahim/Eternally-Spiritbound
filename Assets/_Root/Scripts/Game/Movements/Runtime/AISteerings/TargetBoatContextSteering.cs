using System.Collections.Generic;
using _Root.Scripts.Game.FocusProvider.Runtime;
using _Root.Scripts.Game.Inputs.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Movements.Runtime.AISteerings
{
    public class TargetBoatContextSteering : MonoBehaviour, IFocusConsumer
    {
        [FormerlySerializedAs("mainGameProviders")] [SerializeField] private FocusScriptable mainGameStacks;
        private BoatContextSteering _steering;
        private IMove _move;
        public IntervalTicker ticker;

        private void Start()
        {
            _steering = GetComponent<BoatContextSteering>();
            _move = GetComponent<IMove>();
        }

        private void Update()
        {
            if (IsFocused) return;
            _move.Direction = _steering.Steer(mainGameStacks.mainObject.transform.position);
        }

        public void FixedUpdate()
        {
            if (ticker.TryTick()) _steering.FixedUpdate();
        }

        public bool IsFocused { get; private set; }

        public void SetFocus(Dictionary<AssetReferenceGameObject, GameObject> activeElements,
            TransformReferences transformReferences, GameObject targetGameObject)
        {
            IsFocused = true;
        }

        public void OnFocusLost(GameObject targetGameObject)
        {
            IsFocused = false;
        }
    }
}