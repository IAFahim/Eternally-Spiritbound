using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Movements.Runtime.AISteerings
{
    public class TargetBoatContextSteering : MonoBehaviour
    {
        [SerializeField] private MainObjectProviderScriptable mainGameObjectProviders;
        private BoatContextSteering _steering;

        private void Start()
        {
            _steering = GetComponent<BoatContextSteering>();
        }

        private void Update()
        {
            _steering.Steer(mainGameObjectProviders.mainGameObjectInstance.transform.position);
        }

        private void OnCollisionEnter(Collision other)
        {
            gameObject.SetActive(false);
        }
    }
}