using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField] private TextMeshProUGUI countText;

    private Animator animator;
    private int previousCountDownNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        KicthenGameManeger.Instance.OnStateChanged += KicthenGameManeger_OnStateChanged;
        Hide();
    }
    

    private void Update()
    {
        int countDownNumber = Mathf.CeilToInt(KicthenGameManeger.Instance.GetCountdownToStartTimer());
        countText.text = countDownNumber.ToString();
        if( previousCountDownNumber != countDownNumber)
        {
            previousCountDownNumber = countDownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SaundManeger.Instance.PlayCounDownSound();
        }
    }

    private void KicthenGameManeger_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KicthenGameManeger.Instance.IsCountdownToStart())
        {
            Show();
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
