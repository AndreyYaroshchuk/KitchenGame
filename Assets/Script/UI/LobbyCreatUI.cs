using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreatUI : MonoBehaviour
{
    [SerializeField] private Button close;
    [SerializeField] private Button createPrivate;
    [SerializeField] private Button createPublic;
    [SerializeField] private TMP_InputField lobbyInputField;
    private void Awake()
    {
        close.onClick.AddListener(() =>
        {
            Hide();

        });
        createPrivate.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.CreateLobby(lobbyInputField.text, true);
        });
        createPublic.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.CreateLobby(lobbyInputField.text, false);
        });
        
    }
    private void Start()
    {
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
