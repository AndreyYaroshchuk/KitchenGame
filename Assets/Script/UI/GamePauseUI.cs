using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] Button buttonResume;
    [SerializeField] Button buttonMainMenu;
    [SerializeField] Button buttonOption;
    [SerializeField] GameObject optionUI;

    private void Awake()
    {
        buttonResume.onClick.AddListener(() => {
            KicthenGameManeger.Instance.PauseGame();
        });

        buttonMainMenu.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
        buttonOption.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show();
        });
    }
    private void Start()
    {
        Hide();
        KicthenGameManeger.Instance.OnLocalGamePaused += KicthenGameManeger_OnLocalGamePaused;
        KicthenGameManeger.Instance.OnLocalGameUnpaused += KicthenGameManeger_OnLocalGameUnpaused;
    }

    private void KicthenGameManeger_OnLocalGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void KicthenGameManeger_OnLocalGamePaused(object sender, System.EventArgs e)
    {
        Show();
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
