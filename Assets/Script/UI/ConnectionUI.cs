using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionUI : MonoBehaviour
{
    private void Start()
    {
        KichenGameMultipler.Instance.OnFailToJoinGame += KichenGameMultipler_OnFailToJoinGame;
        KichenGameMultipler.Instance.OnTryingToJoinGame += KichenGameMultipler_OnTryingToJoinGame;
        Hide();
    }

    private void KichenGameMultipler_OnFailToJoinGame(object sender, System.EventArgs e)
    {
        Hide();
    }
    private void KichenGameMultipler_OnTryingToJoinGame(object sender, System.EventArgs e)
    {
        Show();
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
        KichenGameMultipler.Instance.OnFailToJoinGame -= KichenGameMultipler_OnFailToJoinGame;
        KichenGameMultipler.Instance.OnTryingToJoinGame -= KichenGameMultipler_OnTryingToJoinGame;
    }
}
