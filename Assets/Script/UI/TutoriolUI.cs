using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutoriolUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI keyTextMoveUP;
    [SerializeField] TextMeshProUGUI keyTextMoveDown;
    [SerializeField] TextMeshProUGUI keyTextMoveLeft;
    [SerializeField] TextMeshProUGUI keyTextMoveRight;
    [SerializeField] TextMeshProUGUI keyTextInteract;
    [SerializeField] TextMeshProUGUI keyTextInteractAlt;
    [SerializeField] TextMeshProUGUI keyTextPause;

    private void Start()
    {     
        UpdateVisualTutoriol();
        GameInput.Instance.OnBindingChange += GameInput_OnBindingChange;
        KicthenGameManeger.Instance.OnStateChanged += KicthenGameManeger_OnStateChanged;
        KicthenGameManeger.Instance.OnLocalPlayerRaedyChanged += KicthenGameManeger_OnLocalPlayerRaedyChanged;
    }

    private void KicthenGameManeger_OnLocalPlayerRaedyChanged(object sender, System.EventArgs e)
    {
        if(KicthenGameManeger.Instance.IsLocalPlayerReady())
        {
            Hide();
        }
    }

    private void KicthenGameManeger_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KicthenGameManeger.Instance.IsCountdownToStart())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingChange(object sender, System.EventArgs e)
    {
        UpdateVisualTutoriol();
    }

    private void UpdateVisualTutoriol()
    {
        keyTextMoveUP.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.Move_Up);
        keyTextMoveDown.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.Move_Down);
        keyTextMoveLeft.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.Move_Left);
        keyTextMoveRight.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.Move_Right);
        keyTextInteract.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.Interact);
        keyTextInteractAlt.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.InteractAlternate);
        keyTextPause.text = GameInput.Instance.GetBindingTExt(GameInput.Binding.Pause);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
