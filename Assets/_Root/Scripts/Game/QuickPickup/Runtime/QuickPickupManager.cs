using System;
using System.Collections.Generic;
using Soul.QuickPickup.Runtime;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.QuickPickup.Runtime
{
    [Serializable]
    public abstract class QuickPickupManager<T>
    {
        private Pair<int, int>[] _skipFrameHandlers;
        public T elementReference;
        [SerializeReference] public PickupHandler<T>[] handlers;

        public virtual void Enable(T element, PickupHandler<T>[] handlers)
        {
            this.handlers = handlers;
            elementReference = element;
            for (var i = 0; i < handlers.Length - 1; i++)
            {
                handlers[i].Next = handlers[i + 1];
            }

            handlers[^1].Next = handlers[0];

            _skipFrameHandlers = new Pair<int, int>[handlers.Length];
            for (var i = 0; i < handlers.Length; i++)
            {
                var alwaysLoopingHandler = handlers[i];
                _skipFrameHandlers[i] = new(alwaysLoopingHandler.skipFrame, 0);
            }
        }

        public void Add(GameObject gameObject, int amount)
        {
            var controller = new PickupContainer<T>(gameObject.transform, elementReference, amount);
            handlers[0].Handle(controller);
        }

        public void Process()
        {
            for (var i = 0; i < handlers.Length; i++)
            {
                var handler = handlers[i];
                if (_skipFrameHandlers[i].First == _skipFrameHandlers[i].Second++)
                {
                    handler.Process();
                    _skipFrameHandlers[i].Second = 0;
                }
            }
        }

        public void Clear()
        {
            foreach (var handler in handlers)
            {
                handler.Clear();
            }
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (handlers == null) return;
            foreach (var handler in handlers)
            {
                if (handler == null) continue;
                handler.OnDrawGizmos();
            }
        }
#endif
    }
}