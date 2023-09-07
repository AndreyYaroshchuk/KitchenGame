using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuClearUp : MonoBehaviour
{
    private void Awake()
    {
        if(NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }
        if(KichenGameMultipler.Instance != null)
        {
            Destroy(KichenGameMultipler.Instance.gameObject);
        }
        if(KitchenGameLobby.Instance != null)
        {
            Destroy(KitchenGameLobby.Instance.gameObject);
        }
    }
    }

