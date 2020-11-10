using System;
using System.Collections.Generic;
using UnityEngine.AI;

namespace Twindrums.TheWagaduChronicles.NPC
{
    public static class NPCStateMachineHelper
    {
        public static NPCStateMachine GetIdleOnlyStateMachine(NavMeshAgent agent)
        {
            var states = new List<NPCState>
            {
                new NPCStateIdleStanding(),
                new NPCStateIdleMoving(agent),
                new NPCStateIdleDecideNext(),
            };
            var stateMachine = new NPCStateMachine(states);
            return stateMachine;
        }
    }
}