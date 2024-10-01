using System;
using Sisus.Init;

namespace _Root.Scripts.Game.Tests
{
    [Service(typeof(IMoney))]
    [Serializable]
    public class Money : IMoney
    {
        public int currentMoney;
        public int Value
        {
            get => currentMoney;
            set => currentMoney = value;
        }
    }
}