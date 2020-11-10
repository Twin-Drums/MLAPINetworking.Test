using System;
using UnityEngine;
using UnityEngine.AI;

namespace Twindrums.TheWagaduChronicles.NPC
{
    public class NPCStateIdleMoving : NPCState
    {
        private NavMeshAgent agent;

        public NPCStateIdleMoving(NavMeshAgent agent)
        {
            StateId = NPCStateId.IdleMoving;
            this.agent = agent;
        }

        public override void Enter(NPCState from, NPCStateData data, NPCStateMachine stateMachine)
        {
            base.Enter(from, data, stateMachine);
            SetNewTarget();
        }

        public override void Update()
        {            
            if(!this.agent.hasPath || agent.isStopped)
            {
                stateMachine.Set(NPCStateId.IdleDecideNext);                
            }
        }

        private void SetNewTarget()
        {
            var offset = UnityEngine.Random.insideUnitCircle * 10f;
            var newPosition = NearestValidDestination(agent, agent.transform.position +  new Vector3(offset.x, 0, offset.y));
            //Debug.LogFormat("[NPCStateIdleMoving::SetNewTarget] offset={0} newPosition={1}", offset, newPosition);
            agent.SetDestination(newPosition);
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