using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class Initialization : MonoBehaviour
{
    [SerializeField] private bool isServer;

    private void Start()
    {
        if(isServer)
        {            
            NetworkingManager.Singleton.StartServer();
        }
        else
        {
#if UNITY_EDITOR
            NetworkingManager.Singleton.StartHost();
#else
            NetworkingManager.Singleton.StartClient();
#endif
        }
    }
}
