using System;
using Alchemy.Inspector;
using Pancake.StatModifier;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.Init_Tests
{
    [InitInEditMode(From.GameObject)]
    public class PlayerController : MonoBehaviour<Rigidbody, Camera, IStatModifierFactory>
    {
        public Rigidbody Rb { get; private set; }
        public Camera Cam { get; private set; }
        public Stat stat;
        public BaseStat baseStat;
        public IStatModifierFactory StatModifierFactory { get; private set; }

        protected override void Init(Rigidbody firstArgument, Camera secondArgument, IStatModifierFactory thirdArgument)
        {
            Rb = firstArgument;
            Cam = secondArgument;
            StatModifierFactory = thirdArgument;
        }

        protected override void OnAwake()
        {
            stat = new Stat(new StatMediator(), baseStat);
            stat.Mediator.AddModifier(StatModifierFactory.Create(EModifierType.Add, baseStat.statType, 5f, 1f));
        }

        [Button]
        public void PrintStats()
        {
            Debug.Log(stat.Value);
        }

        private void Update()
        {
            stat.Mediator.Update(Time.deltaTime);
        }
    }

    [Serializable]
    public class PlayerData
    {
        public int health;
    }
}