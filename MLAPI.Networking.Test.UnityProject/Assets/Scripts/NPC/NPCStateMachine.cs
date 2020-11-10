using System;
using System.Collections.Generic;

namespace Twindrums.TheWagaduChronicles.NPC
{
    public class NPCStateMachine
    {
        public class NPCStateListIsNullOrEmpty : Exception { }        

        private List<NPCState> states;// todo: Use a map?
        private NPCState current;
        private NPCStateData data;

        public NPCStateMachine(List<NPCState> states)
        {
            if (states == null || states.Count == 0)
                throw new NPCStateListIsNullOrEmpty();

            this.states = states;
        }

        public void Initialize(NPCStateId stateId,
                               NPCStateData data)
        {
            if (data == null)
            {
                UnityEngine.Debug.LogErrorFormat("[NPCStateMachine::Initalize] data=null");
                return;
            }

            var state = GetState(stateId);
            if (state == null)
            {
                UnityEngine.Debug.LogErrorFormat("[NPCStateMachine::Initalize] Did not find state for id={0}", stateId);
                return;
            }

            this.data = data;
            current = state;

            state.Enter(null, data, this);
        }

        public void Update()
        {
            current?.Update();
        }

        public void Set(NPCStateId stateId)
        {
            if (current.StateId == stateId)
                return;

            var newState = GetState(stateId);

            if(newState == null)
            {
                UnityEngine.Debug.LogErrorFormat("[NPCStateMachine::Set] Did not find state for id={0}", stateId);
                return;
            }
            var oldState = current;
            
            oldState?.Exit(newState);
            current = newState;
            data.Time = 0f;
            newState.Enter(oldState, data, this);
        }

        private NPCState GetState(NPCStateId stateId)
        {
            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].StateId == stateId)
                    return states[i];
            }

            return null;
        }
    }
}