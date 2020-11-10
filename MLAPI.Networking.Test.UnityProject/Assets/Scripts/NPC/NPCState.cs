using System;

namespace Twindrums.TheWagaduChronicles.NPC
{
    public class NPCState
    {
        public NPCStateId StateId { get; protected set; }

        protected NPCStateMachine stateMachine;
        protected NPCStateData data;

        public virtual void Enter(NPCState from, NPCStateData data, NPCStateMachine stateMachine)
        {
           this.data = data;
           this.stateMachine = stateMachine;
        }

        public virtual void Exit(NPCState to)
        {
        }

        public virtual void Update()
        {
        }
    }
}