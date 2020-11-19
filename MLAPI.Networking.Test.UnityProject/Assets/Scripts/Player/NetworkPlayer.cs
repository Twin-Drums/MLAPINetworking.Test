using UnityEngine;
using System.Collections;
using MLAPI;
using Twindrums.TheWagaduChronicles.Visibility;
using System;
using UnityEngine.AI;
using Twindrums.TheWagaduChronicles.NPC;
using Twindrums.TheWagaduChronicles.Playground.Player;
using Twindrums.TheWagaduChronicles.Client.Avatar;

namespace Twindrums.TheWagaduChronicles.Player
{
    public class NetworkPlayer : NetworkedBehaviour
    {
        [SerializeField] private Bounds bounds;

        public override void NetworkStart()
        {
            if (Application.isBatchMode)
                GetComponent<NPCDriver>().enabled = true;

            if (IsLocalPlayer)
            {
                WarpToRandonStartPosition();
                if(!Application.isBatchMode)
                {
                    GetComponent<NPCDriver>().enabled = false;
                    GetComponent<SimpleAvatarNavmeshMovement>().enabled = true;
                    GetComponent<AvatarCameraConnector>().enabled = true;
                }
            }
        }

        private void WarpToRandonStartPosition()
        {
            var navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.enabled = false;
            var pos = new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
            this.transform.position = pos;
            navMeshAgent.enabled = true;
        }
    }
}