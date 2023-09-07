using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayngClockUi : MonoBehaviour
{
    [SerializeField] Image TimerImage;

    private void Update()
    {
        TimerImage.fillAmount = KicthenGameManeger.Instance.GetGamePlayingTimer();
    }
}
