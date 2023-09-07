using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlateComplateVisual;


public class PlateComplateVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObject_GameObject
    {
        public GameObject gameObject;
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObject_GameObject> KitchenObjectSOGameObjectList;

    private void Start()
    {
        foreach (KitchenObject_GameObject kitchenObjectSOGameObject in KitchenObjectSOGameObjectList)
        {
            kitchenObjectSOGameObject.gameObject.SetActive(false);                  
        }
        plateKitchenObject.OnIngredientAdd += PlateKitchenObject_OnIngredientAdd;

    }


    private void PlateKitchenObject_OnIngredientAdd(object sender, PlateKitchenObject.OnIngredientAddEventArgs e)
    {
        foreach (KitchenObject_GameObject kitchenObjectSOGameObject in KitchenObjectSOGameObjectList)
        {
            if(kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
