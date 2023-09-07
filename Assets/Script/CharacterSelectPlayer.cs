using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject readyGameObject;
    [SerializeField] private TextMeshPro playerNameText;
    [SerializeField] PlayerVisual playerVisual;
    
   
    private void Start()
    {
        KichenGameMultipler.Instance.OnPlayerDataChanged += KichenGameMultipler_OnPlayerDataChanged;
        CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReady_OnReadyChanged;
        UpdatePlayer();
    }

    private void CharacterSelectReady_OnReadyChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void KichenGameMultipler_OnPlayerDataChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }
    private void UpdatePlayer()
    {
        if (KichenGameMultipler.Instance.IsPlayerIndexConnected(playerIndex))
        {
            Show();
            PlayerData playerData = KichenGameMultipler.Instance.GetPlayerFromIndex(playerIndex);
            readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReday(playerData.clientId));

            playerNameText.text = playerData.playerName.ToString(); 
            playerVisual.SetPlayerColor(KichenGameMultipler.Instance.GetPlayerColor(playerData.colorId));
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
