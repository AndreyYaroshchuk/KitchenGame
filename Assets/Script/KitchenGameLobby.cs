using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class KitchenGameLobby : MonoBehaviour
{
    public static KitchenGameLobby Instance { get; private set; }
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private void Awake()
    {
        Instance = this;
      
        DontDestroyOnLoad(gameObject);
        initializeUnityAuthentication();
    }
    private void Update()
    {
        HandleHeartbeat();
    }
    private void HandleHeartbeat()
    {
        if (IsLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if(heartbeatTimer < 0) {

                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }
    private bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }
    private async void initializeUnityAuthentication()
    {
        if(UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions options = new InitializationOptions();
            options.SetProfile(Random.Range(0, 1000).ToString());
            await UnityServices.InitializeAsync(options);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
      
    }
    public async void CreateLobby(string lobbyName,bool isPrivate)
    {
        try
        {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, KichenGameMultipler.MAX_PLAYER_AMOUNT, new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
            });
            KichenGameMultipler.Instance.StartHost();
            Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);

        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
      
     
    }

    public async void QuickJoin()
    {
        try {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            KichenGameMultipler.Instance.StartClient(); 
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }

    }
    public Lobby GetLobby()
    {
        return joinedLobby;
    }
    public async void JoinWithCode(string lobbyCode)
    {
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            KichenGameMultipler.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

}
