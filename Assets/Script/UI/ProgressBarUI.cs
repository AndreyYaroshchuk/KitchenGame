using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] Image barImage;
    private IHasProgress hasProgress;
    [SerializeField] private GameObject hasProgressGameObject;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0;
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNirmalized;

        if (e.progressNirmalized == 0f || e.progressNirmalized == 1f)
        {
            Hide();

        }
        else
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
