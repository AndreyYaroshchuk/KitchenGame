using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryUI : MonoBehaviour
{
    private const string POPUP = "Popup";
    [SerializeField] private Animator animator;
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Hide();
        DeliveryManeger.Instance.OnRecipeSuccess += DeliveryManeger_OnRecipeSuccess;
        DeliveryManeger.Instance.OnRecipeFailed += DeliveryManeger_OnRecipeFailed;
    }

    private void DeliveryManeger_OnRecipeFailed(object sender, System.EventArgs e)
    {
        Show();
        background.color = failedColor;
        icon.sprite = failedSprite;
        text.text = "DELIVERY\nFAILED";
        animator.SetTrigger(POPUP);
    }

    private void DeliveryManeger_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        Show();
        background.color = successColor;
        icon.sprite = successSprite;
        text.text = "DELIVERY\nSUCCESS";
        animator.SetTrigger(POPUP); 
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
