using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class Initialization : MonoBehaviour
{
    [SerializeField] private bool isServer;

    private void Start()
    {
        Application.targetFrameRate = 20;

        if(isServer)
        {            
            NetworkingManager.Singleton.StartServer();
        }
        else
        {
#if UNITY_EDITOR            
#else
            NetworkingManager.Singleton.StartClient();
#endif
        }
    }
}
