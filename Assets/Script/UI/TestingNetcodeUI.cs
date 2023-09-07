using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] Button buttonHost;
    [SerializeField] Button buttonClient;

    private void Awake()
    {
        buttonHost.onClick.AddListener(() => 
        {
            Debug.Log("HOST");
            KichenGameMultipler.Instance.StartHost();
            Hide();
        });
        buttonClient.onClick.AddListener(() =>
        {
            Debug.Log("CLIENT");
            KichenGameMultipler.Instance.StartClient(); 
            Hide();
        });
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
