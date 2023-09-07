using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionReasonTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button closeButton;


    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }
    private void Start()
    {
        KichenGameMultipler.Instance.OnTryingToJoinGame += KichenGameMultipler_OnTryingToJoinGame;
        Hide();
    }

    private void KichenGameMultipler_OnTryingToJoinGame(object sender, System.EventArgs e)
    {
        Show();
        text.text = NetworkManager.Singleton.DisconnectReason;

        if(text.text == "")
        {
            text.text = "Ошибка подключения";
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
    private void OnDestroy()
    {
        KichenGameMultipler.Instance.OnTryingToJoinGame -= KichenGameMultipler_OnTryingToJoinGame;
    }
}
