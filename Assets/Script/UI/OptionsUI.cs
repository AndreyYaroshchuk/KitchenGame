using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] GameObject pressToRebind;

    [SerializeField] Button buttonMusic;
    [SerializeField] Button buttonSoundEffect;
    [SerializeField] Button buttonClose;
    [SerializeField] Button buttonMoveUP;
    [SerializeField] Button buttonMoveDown;
    [SerializeField] Button buttonMoveLeft;
    [SerializeField] Button buttonMoveRight;
    [SerializeField] Button buttonInteract;
    [SerializeField] Button buttonInteractAlt;
    [SerializeField] Button buttonPause;


    [SerializeField] TextMeshProUGUI textMoveUP;
    [SerializeField] TextMeshProUGUI textMoveDown;
    [SerializeField] TextMeshProUGUI textMoveLeft;
    [SerializeField] TextMeshProUGUI textMoveRight;
    [SerializeField] TextMeshProUGUI textInteract;
    [SerializeField] TextMeshProUGUI textInteractAlt;
    [SerializeField] TextMeshProUGUI textPause;


    [SerializeField] TextMeshProUGUI textSoundEffect;
    [SerializeField] TextMeshProUGUI textMusic;

    private void Awake()
    {
        Instance = this;
        buttonMusic.onClick.AddListener(() =>
        {
            MusicManeger.Instance.ChangeVolume();
            UpdateVisual();
        });
        buttonSoundEffect.onClick.AddListener(() =>
        {
            SaundManeger.Instance.ChangeVolume();
            UpdateVisual();

        });
        buttonClose.onClick.AddListener(() =>
        {
            Hide();
        });
        buttonMoveUP.onClick.AddListener(() =>
        {
            RebingBinnding(GameInput.Binding.Move_Up);
        });
        buttonMoveDown.onClick.AddListener(() =>
        {
            RebingBinnding(GameInput.Binding.Move_Down);
        });
        buttonMoveLeft.onClick.AddListener(() =>
        {
            RebingBinnding(GameInput.Binding.Move_Left);
        });
        buttonMoveRight.onClick.AddListener(() =>
        {
            RebingBinnding(GameInput.Binding.Move_Right);
        });
        buttonInteract.onClick.AddListener(() =>
        {
            RebingBinnding(GameInput.Binding.Interact);
        });
        buttonInteractAlt.onClick.AddListener(() =>
        {
            RebingBinnding(GameInput.Binding.InteractAlternate);
        });
        buttonPause.onClick.AddListener(() =>
        {
            RebingBinnding(GameInput.Binding.Pause);
        });
    }
    private void Start()
    {
        KicthenGameManeger.Instance.OnLocalGameUnpaused += KicthenGameManeger_OnGameUnpaused;

        UpdateVisual();
        Hide();
        HidePressToRebind();
    }

    private void KicthenGameManeger_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        textSoundEffect.text = "Sound Effects: " + Mathf.Round(SaundManeger.Instance.GetVolume() * 10f);
        textMusic.text = "Music: " + Mathf.Round(MusicManeger.Instance.GetVolume() * 10f);

        textMoveUP.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.Move_Up);
        textMoveDown.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.Move_Down);
        textMoveLeft.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.Move_Left);
        textMoveRight.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.Move_Right);
        textInteract.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.Interact);
        textInteractAlt.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.InteractAlternate);
        textPause.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.Pause);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebind()
    {
        pressToRebind.SetActive(true);
    }
    private void HidePressToRebind()
    {
        pressToRebind.SetActive(false);
    }

    private void RebingBinnding(GameInput.Binding binding)
    {
        ShowPressToRebind();
        GameInput.Instance.RebingBinding(binding, () =>  
        {

            HidePressToRebind();
            UpdateVisual();

        });

    }
}
