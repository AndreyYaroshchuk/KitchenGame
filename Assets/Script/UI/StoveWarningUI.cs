using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveWarningUI : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        Hide();
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burningShowProgress = 0.5f;
        bool show = stoveCounter.IsFried() && e.progressNirmalized >= burningShowProgress;
        
        if(show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
