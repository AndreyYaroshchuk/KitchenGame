using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button creatButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button joinCodeButton;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private LobbyCreatUI lobbyCreatUI;

    private void Awake()
    {
        creatButton.onClick.AddListener(() =>
        {
            lobbyCreatUI.Show();
        });
        quickJoinButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.QuickJoin();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
        joinCodeButton.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.JoinWithCode(inputField.text);
        });
    }
    private void Start()
    {
        playerName.text = KichenGameMultipler.Instance.GetPlayerName();
        playerName.onValueChanged.AddListener((string newText) =>
        {
            KichenGameMultipler.Instance.SetPlayerName(newText);
        });
    }
}
