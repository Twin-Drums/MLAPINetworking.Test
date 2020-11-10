using System;
using UnityEngine;
using UnityEngine.AI;

namespace Twindrums.TheWagaduChronicles.NPC
{
    public class NPCStateIdleDecideNext : NPCState
    {
        public NPCStateIdleDecideNext() => StateId = NPCStateId.IdleDecideNext;

        public override void Enter(NPCState from, NPCStateData data, NPCStateMachine stateMachine)
        {
            switch (from.StateId)
            {
                default:
                case NPCStateId.IdleStanding:
                    stateMachine.Set(NPCStateId.IdleMoving);
                    break;
                case NPCStateId.IdleMoving:
                    stateMachine.Set(NPCStateId.IdleStanding);
                    break;
            }
        }
    }
}