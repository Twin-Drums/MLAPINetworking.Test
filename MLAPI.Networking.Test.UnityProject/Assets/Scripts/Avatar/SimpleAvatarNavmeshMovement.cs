using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Twindrums.TheWagaduChronicles.Playground.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SimpleAvatarNavmeshMovement : MonoBehaviour
    {        
        public LayerMask layerMask;

        private NavMeshAgent _agent;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (EventSystem.current != null &&  EventSystem.current.IsPointerOverGameObject())
            {
                // GUI Action
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                HandleSelectDestination();
            }
        }
        
        private void HandleSelectDestination()
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
            {
                var destination = NearestValidDestination(_agent, hit.point);
                _agent.destination = destination;                
            }
        }

        public static Vector3 NearestValidDestination(NavMeshAgent agent, Vector3 destination)
        {
            // can we calculate a path there? then return the closest valid point
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(destination, path))
                return path.corners[path.corners.Length - 1];

            // otherwise find nearest navmesh position first. we use a radius of
            // speed*2 which works fine. afterwards we find the closest valid point.
            if (NavMesh.SamplePosition(destination, out NavMeshHit hit, agent.speed * 2, NavMesh.AllAreas))
                if (agent.CalculatePath(hit.position, path))
                    return path.corners[path.corners.Length - 1];

            // nothing worked, don't go anywhere.
            return agent.transform.position;
        }
    }
}
