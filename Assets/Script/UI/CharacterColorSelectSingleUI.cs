using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectSingleUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private int colorId;
    [SerializeField] private GameObject selectedGameObject;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            KichenGameMultipler.Instance.ChangePlayerColor(colorId);
        });
    }
    private void Start()
    {
        KichenGameMultipler.Instance.OnPlayerDataChanged += KichenGameMultipler_OnPlayerDataChanged;
        image.color = KichenGameMultipler.Instance.GetPlayerColor(colorId);
        UpdateIsSelected();
    }

    private void KichenGameMultipler_OnPlayerDataChanged(object sender, System.EventArgs e)
    {
        UpdateIsSelected();
    }

    private void UpdateIsSelected()
    {
        if(KichenGameMultipler.Instance.GetPlayerData().colorId == colorId)
        {
            selectedGameObject.SetActive(true);
        }
        else
        {
            selectedGameObject.SetActive(false);
        }
    }
}
