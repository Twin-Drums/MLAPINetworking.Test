using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class Initialization : MonoBehaviour
{
    [SerializeField] private bool isServer;
    [SerializeField] private string ip;
    [SerializeField] private int port;

    private void Start()
    {
        if(isServer)
        {            
            NetworkingManager.Singleton.StartServer();
        }
    }
}
