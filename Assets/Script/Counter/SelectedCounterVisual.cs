using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;
   
    private void Start()
    {
        
        if(Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSlectedCounterChange += Player_OnSlectedCounterChange;
        }
        else
        {
            Player.OnPlayerSpawnet += Player_OnPlayerSpawnet;
        }
    }

    private void Player_OnPlayerSpawnet(object sender, System.EventArgs e)
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSlectedCounterChange -= Player_OnSlectedCounterChange;
            Player.LocalInstance.OnSlectedCounterChange += Player_OnSlectedCounterChange;
        }
    }

    private void Player_OnSlectedCounterChange(object sender, Player.OnSlectedCounterChangeEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
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
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
       
    }
    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
