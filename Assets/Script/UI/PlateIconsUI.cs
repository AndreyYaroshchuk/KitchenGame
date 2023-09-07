using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconsUI : MonoBehaviour
{

    [SerializeField] PlateKitchenObject plateKitchenObject;
    [SerializeField] Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        plateKitchenObject.OnIngredientAdd += PlateKitchenObject_OnIngredientAdd;
    }
    
    private void PlateKitchenObject_OnIngredientAdd(object sender, PlateKitchenObject.OnIngredientAddEventArgs e)
    {
        UpdateVisual();
    }
    //public void UpdateVisual()
    //{
    //    foreach(Transform child in transform)
    //    {
    //        if (child == iconTemplate) continue; // продолжает действие
    //        Destroy(child.gameObject);
    //    }
    //    foreach(KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
    //    {
    //        Transform iconTempalateSpawn = Instantiate(iconTemplate, transform);
    //        iconTemplate.gameObject.SetActive(true);
    //        iconTempalateSpawn.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
    //    }
    //}
    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
