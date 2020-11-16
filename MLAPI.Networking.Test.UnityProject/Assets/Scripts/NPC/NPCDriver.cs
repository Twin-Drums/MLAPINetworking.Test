using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;
using UnityEngine.AI;

namespace Twindrums.TheWagaduChronicles.NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCDriver : MonoBehaviour
    {
        private NPCStateMachine stateMachine;
        private NPCStateData stateData;

        private void Start()
        {
            stateData = new NPCStateData();
            stateMachine = NPCStateMachineHelper.GetIdleOnlyStateMachine(GetComponent<NavMeshAgent>());
            stateMachine.Initialize(NPCStateId.IdleStanding, stateData);
        }
        
        private void Update()
        {
            stateData.Time += Time.deltaTime;
            stateMachine.Update();
        }
    }
}
