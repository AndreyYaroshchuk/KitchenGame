using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HostDictonectionUi : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;

    private void Start()
    {
   
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        Hide();
    }
    private void Update()
    {
        playAgainButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }
    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
       if(clientId == NetworkManager.ServerClientId) 
        {
            // сервер отключился 
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
