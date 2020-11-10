using System;
using UnityEngine;
using UnityEngine.AI;

namespace Twindrums.TheWagaduChronicles.NPC
{
    public class NPCStateIdleStanding : NPCState
    {
        public NPCStateIdleStanding() => StateId = NPCStateId.IdleStanding;

        public override void Enter(NPCState from, NPCStateData data, NPCStateMachine stateMachine)
        {
            base.Enter(from, data, stateMachine);

            if (data.Duration <= 0f)
                data.Duration = UnityEngine.Random.Range(2f, 10f);
        }

        public override void Exit(NPCState to)
        {
            base.Exit(to);
            data.Duration = 0f;
        }

        public override void Update()
        {
            if(data.Time >= data.Duration)
            {
                stateMachine.Set(NPCStateId.IdleDecideNext);
            }
        }
    }
}