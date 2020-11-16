using UnityEngine;
using System.Collections;
using MLAPI;

namespace Twindrums.TheWagaduChronicles.NetworkNPC
{
    public class ServerNPCSpawner : NetworkedBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int amount = 100;
        [SerializeField] private Bounds bounds;

        public override void NetworkStart()
        {
            Debug.Log("[ServerNPCSpawner::NetworkStart]");
            if(IsServer)
            {
                Debug.Log("[ServerNPCSpawner::NetworkStart] Spawning " + amount + " NPCs");
                for (int i = 0; i < amount; i++)
                {
                    var pos = new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
                    GameObject go = Instantiate(prefab, pos, Quaternion.identity);
                    var no = go.GetComponent<NetworkedObject>();
                    no.Spawn();                    
                }
            }
        }
    }
}
