using UnityEngine;
using System.Collections;
using MLAPI;
using System;
using System.Text.RegularExpressions;

namespace Twindrums.TheWagaduChronicles.NetworkNPC
{
    public class ServerNPCSpawner : NetworkedBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int amount = 100;
        [SerializeField] private Bounds bounds;

        private Regex amountNPCsRegex = new Regex(@"AmountNPCs=(\d+)");

        public override void NetworkStart()
        {
            var args = System.Environment.GetCommandLineArgs();
            foreach (var item in args)
            {                
                if(amountNPCsRegex.IsMatch(item))
                {                    
                    amount = int.Parse(amountNPCsRegex.Match(item).Groups[1].Value);
                }
            }

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
