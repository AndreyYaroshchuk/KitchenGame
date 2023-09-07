using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMultipUI : MonoBehaviour
{
    private void Start()
    {
        KicthenGameManeger.Instance.OnMultipGamePaused += KicthenGameManeger_OnMultipGamePaused;
        KicthenGameManeger.Instance.OnMultipGameUnpaused += KicthenGameManeger_OnMultipGameUnpaused;
        Hide();
    }

    private void KicthenGameManeger_OnMultipGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void KicthenGameManeger_OnMultipGamePaused(object sender, System.EventArgs e)
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
}
