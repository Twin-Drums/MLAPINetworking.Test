using UnityEngine;

namespace Twindrums.TheWagaduChronicles.Playground.Player
{
    public class SimpleObjectFollower : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public bool lookAt;

        private void Update()
        {
            this.transform.position = target.position + offset;
            if(lookAt)
            {
                this.transform.LookAt(target);
            }
        }
    }
}