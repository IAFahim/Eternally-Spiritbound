using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.Init_Tests
{
    public class Client : MonoBehaviour<Service>
    {
        protected override void Init(Service allItemSo)
        {
            allItemSo.Greet();
        }
    }


    [Service]
    public class Service
    {
        public void Greet() => Debug.Log("Hello, World!");
    }
}