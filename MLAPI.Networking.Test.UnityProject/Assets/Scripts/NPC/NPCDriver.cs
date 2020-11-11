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
        private NetworkedObject networkObject;

        private void Awake()
        {
            networkObject = GetComponent<NetworkedObject>();
            networkObject.CheckObjectVisibility += HandleCheckObjectVisibility;
        }

        private void OnDestroy()
        {
            networkObject.CheckObjectVisibility -= HandleCheckObjectVisibility;
        }

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

        private bool HandleCheckObjectVisibility(ulong clientId)
        {
            if (Vector3.Distance(NetworkingManager.Singleton.ConnectedClients[clientId].PlayerObject.transform.position, transform.position) > 5)
            {
                // Only show the object to players that are within 5 meters. Note that this has to be rechecked by your own code
                // If you want it to update as the client and objects distance change.
                // This callback is usually only called once per client
                return true;
            }
            else
            {
                // Dont show this object
                return false;
            }
        }
    }
}
