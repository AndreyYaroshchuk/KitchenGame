using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;

public class CharacterSelectReady : NetworkBehaviour
{

    public event EventHandler OnReadyChanged;
    public static CharacterSelectReady Instance { get; private set; }
    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake()
    {
        playerReadyDictionary = new Dictionary<ulong, bool>();
        Instance = this;
    }
    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
        bool allClientsReady = true;
        foreach (ulong ClientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(ClientId) || !playerReadyDictionary[ClientId])
            {
                //игрок не готов 
                allClientsReady = false;
                break;
            }
        }
        if (allClientsReady)
        {
            Loader.LoadNetwork(Loader.Scene.GameScene);
        }
        Debug.Log("allclientsReady: " + allClientsReady);
        
    }
    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientId)
    {
        playerReadyDictionary[clientId] = true;
        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerReday(ulong clientId)
    {
        return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
    }

}
