using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.BreadCrumbs
{
    public class BreadCrumb : MonoBehaviour, IInitializable<Transform, int, int, int>
    {
        [SerializeField] private int breadCrumbInterval = 60;
        [SerializeField] private int currentInterval = 0;
        [SerializeField] private int moveDistance = 2;
        [SerializeField] private int breadCrumbCount = 5;
        [SerializeField] private Transform breadCrumbTarget;
        [ShowInInspector] private Vector3[] breadcrumbs;

        private int currentIndex;


        private void OnEnable()
        {
            currentInterval = 0;
            currentIndex = 0;
            currentInterval = breadCrumbCount - 1;

            breadcrumbs = new Vector3[breadCrumbCount];
            var position = breadCrumbTarget.position;
            for (int i = 0; i < breadcrumbs.Length; i++)
            {
                breadcrumbs[i] = position;
            }
        }

        // here also check if he has moved a certain distance
        public void SetBreadCrumb(Vector3 position)
        {
            if (currentIndex >= breadcrumbs.Length) currentIndex = 0;
            breadcrumbs[currentIndex++] = position;
        }

        public Vector3 GetBreadCrumbStart()
        {
            return breadcrumbs[(currentIndex + 1) % breadcrumbs.Length];
        }

        public Vector3 GetFarthestBreadCrumbFromCurrent()
        {
            float maxDistance = 0;
            int index = 0;
            Vector3 currentPosition = breadCrumbTarget.position;

            for (int i = 0; i < breadcrumbs.Length; i++)
            {
                var distance = Vector3.Distance(currentPosition, breadcrumbs[i]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    index = i;
                }
            }

            return breadcrumbs[index];
        }


        private void Update()
        {
            currentInterval++;
            if (currentInterval >= breadCrumbInterval)
            {
                var currentPosition = breadCrumbTarget.position;
                var lastPosition = GetBreadCrumbStart();
                var xzDistance = Mathf.Abs(currentPosition.x - lastPosition.x) +
                                 Mathf.Abs(currentPosition.z - lastPosition.z);
                if (xzDistance > moveDistance) SetBreadCrumb(currentPosition);
                currentInterval = 0;
            }
        }

        public void Init(Transform target, int interval, int historyCount, int distance)
        {
            breadCrumbTarget = target;
            breadCrumbInterval = interval;
            breadCrumbCount = historyCount;
        }
    }
}