using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button buttonPlay;
    [SerializeField] Button buttonExit;


    private void Awake()
    {
        Time.timeScale = 1f;
        buttonPlay.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.LobbyScene);
        });
        buttonExit.onClick.AddListener(() =>
        {
           Application.Quit();
        });
    }
}
