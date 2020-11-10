using UnityEngine;
using UnityEngine.AI;

namespace Twindrums.TheWagaduChronicles.Avatar
{
    [RequireComponent(typeof(Animator))]
    public class AvatarAnimation : MonoBehaviour
    {
        public bool hasWalk;

        public NavMeshAgent agent;
        private Animator _animator;
        private float _speed;
        private Vector3 _lastPosition;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        private void LateUpdate()
        {
            if(agent != null)
            {
                _speed = Vector3.Distance(_lastPosition, agent.transform.localPosition) / Time.deltaTime;            
                _lastPosition = agent.transform.localPosition;
            }
            else
            {
                _speed = 0f;
            }

            _animator.SetBool("MOVING", IsMoving(hasWalk ? 3f : 0.2f));

            if(hasWalk)
                _animator.SetBool("Walking", IsMoving(0.2f, 3f));
        }

        private bool IsMoving(float minSpeed, float maxSpeed = 50f)
        {
            return (agent != null && agent.velocity.magnitude > minSpeed && agent.velocity.magnitude < maxSpeed) || (_speed > minSpeed && _speed < maxSpeed);
            // return (agent != null && agent.velocity.magnitude > 0.1f) || (_speed > 0.1f && _speed < 50f);
        }
    }
}