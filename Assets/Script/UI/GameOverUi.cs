using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Button playGameButton;
    private void Start()
    {
        Hide();
        KicthenGameManeger.Instance.OnStateChanged += KicthenGameManeger_OnStateChanged;
    }
    private void Update()
    {
        playGameButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
    }
    private void OnDestroy()
    {
        KicthenGameManeger.Instance.OnStateChanged -= KicthenGameManeger_OnStateChanged;
    }
    private void KicthenGameManeger_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KicthenGameManeger.Instance.IsGameOver())
        {
            Show();

            countText.text = DeliveryManeger.Instance.GetSuccessRecipesAmount().ToString();
        }
        else
        {
            Hide();
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
