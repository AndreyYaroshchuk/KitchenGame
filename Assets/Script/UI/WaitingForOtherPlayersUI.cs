using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForOtherPlayersUI : MonoBehaviour
{
    private void Start()
    {
        KicthenGameManeger.Instance.OnLocalPlayerRaedyChanged += KicthenGameManeger_OnLocalPlayerRaedyChanged;
        KicthenGameManeger.Instance.OnStateChanged += KicthenGameManeger_OnStateChanged;
        Hide();
    }

    private void KicthenGameManeger_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KicthenGameManeger.Instance.IsCountdownToStart())
        {
            Hide();
        }
      
    }

    private void KicthenGameManeger_OnLocalPlayerRaedyChanged(object sender, System.EventArgs e)
    {   
        if(KicthenGameManeger.Instance.IsLocalPlayerReady())
        {
            Show();
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
