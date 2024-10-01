using Alchemy.Inspector;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.Tests
{
    public class InjectTest : MonoBehaviour<IMoney>
    {
        private IMoney _money;
        protected override void Init(IMoney firstArgument)
        {
            _money = firstArgument;    
        }
        
        [Button]
        public void SetRandomMoneyValue()
        {
            _money.Value = Random.Range(0, 100);
        }
        
        [Button]
        public void PrintMoneyValue()
        {
            Debug.Log($"Money value: {_money.Value}");
        }
    }
}
