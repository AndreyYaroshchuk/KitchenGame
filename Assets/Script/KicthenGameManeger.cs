using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using Unity.VisualScripting;

public class KicthenGameManeger : NetworkBehaviour
{

    public static KicthenGameManeger Instance { get; private set; }

    public event EventHandler OnStateChanged;

    public event EventHandler OnLocalGamePaused;
    public event EventHandler OnLocalGameUnpaused;

    public event EventHandler OnMultipGamePaused;
    public event EventHandler OnMultipGameUnpaused;

    public event EventHandler OnLocalPlayerRaedyChanged;

    [SerializeField] private Transform playerPrefab;

    private NetworkVariable<float> countDownTimer = new NetworkVariable<float>(3f); //
    private NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(0f);// сделать сервернынми
    private float gamePlayingTimerMax = 300f;
    private bool isLocalGamePause = false;
    private NetworkVariable<bool> isGamePause = new NetworkVariable<bool>(false);
    private Dictionary<ulong, bool> playerReadyDictionary;
    private Dictionary<ulong, bool> playerPausedDictionary;
    private bool autoTestGamePause;

    private enum State
    {
        waitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,

    }
    private bool isLocalPlayRaedy;
    private NetworkVariable<State> state = new NetworkVariable<State>(State.waitingToStart);

    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
        playerPausedDictionary = new Dictionary<ulong, bool>();
    }
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractionAction += GameInput_OnInteractionAction;

        /// Trigger to Start
        //state = State.CountdownToStart;
        //OnStateChanged?.Invoke(this, EventArgs.Empty);

    }
    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
        isGamePause.OnValueChanged += IsGamePaused_OnValueChanged;
        if(IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += NetworkManager_OnLoadEventCompleted;
        }
    }

    private void NetworkManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientID)
    {
        autoTestGamePause = true;
    }

    private void IsGamePaused_OnValueChanged(bool previousValue, bool newValue)
    {
        if (isGamePause.Value)
        {
            Time.timeScale = 0f;
            OnMultipGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnMultipGameUnpaused?.Invoke(this, EventArgs.Empty);
            
        }
    }
    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);

    }

    private void GameInput_OnInteractionAction(object sender, EventArgs e)
    {
        if (state.Value == State.waitingToStart)
        {
            isLocalPlayRaedy = true;
            OnLocalPlayerRaedyChanged?.Invoke(this, EventArgs.Empty);
            SetPlayerReadyServerRpc();

        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
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
            state.Value = State.CountdownToStart;
        }
        Debug.Log("allclientsReady: " + allClientsReady);
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        PauseGame();
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        switch (state.Value)
        {
            case State.waitingToStart:
                break;
            case State.CountdownToStart:
                countDownTimer.Value -= Time.deltaTime;
                if (countDownTimer.Value < 0f)
                {
                    state.Value = State.GamePlaying;
                    gamePlayingTimer.Value = gamePlayingTimerMax;

                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                if (gamePlayingTimer.Value < 0f)
                {
                    state.Value = State.GameOver;

                }
                break;
            case State.GameOver:

                break;

        }

    }

   
    public void PauseGame()
    {

        isLocalGamePause = !isLocalGamePause;
        if (isLocalGamePause)
        {
            PauseGameServerRpc();
            OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
            
        }
        else
        {
            UnPauseGameServerRpc();
            OnLocalGameUnpaused?.Invoke(this, EventArgs.Empty);
            
        }
    }

    private void LateUpdate()
    {
        if(autoTestGamePause)
        {
            autoTestGamePause = false;
            TestGamePauseState();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;
        TestGamePauseState();
    }
    [ServerRpc(RequireOwnership = false)]
    private void UnPauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;
        TestGamePauseState();
    }
    private void TestGamePauseState()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playerPausedDictionary.ContainsKey(clientId) && playerPausedDictionary[clientId])
            {
                // любой игрок поставил на паузу 
                isGamePause.Value = true;
                return;
            }
        }
        isGamePause.Value = false;
        // все игроки сняты с паузы 
    }
    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }
    public bool IsCountdownToStart()
    {
        return state.Value == State.CountdownToStart;
    }
    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }
    public bool IsWatingToStar()
    {
        return state.Value == State.waitingToStart;
    }
    public float GetCountdownToStartTimer()
    {
        return countDownTimer.Value;
    }
    public float GetGamePlayingTimer()
    {
        return gamePlayingTimer.Value / gamePlayingTimerMax;
    }
    public bool IsLocalPlayerReady()
    {
        return isLocalPlayRaedy;
    }



}
