using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManegerUI : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] Transform recipeTemplete;
    

    private void Awake()
    {
        recipeTemplete.gameObject.SetActive(false);
    }
    private void Start()
    {
       
        DeliveryManeger.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        DeliveryManeger.Instance.OnRecipeSpawned += DeliveryManeger_OnRecipeSpawned;
        UpdateVisual();
    }
   

    private void DeliveryManeger_OnRecipeSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    //private void UpdateVisual()
    //{
    //    foreach (Transform child in conteiner)
    //    {
    //        if (child == recipeTemplate) continue;
    //        Destroy(child.gameObject);  
           
    //    }
    //    foreach (RecipeSO recipeSO in DeliveryManeger.Instance.GetWaitingResipeSOList())
    //    {
    //        Transform recipeTranfrom = Instantiate(recipeTemplate, conteiner);
            
    //        recipeTemplate.gameObject.SetActive(true);
    //        recipeTranfrom.GetComponent<DeliveryManegerSinglUI>().SetRecipeSO(recipeSO); 
    //    }
    //}

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplete) continue;
            Destroy(child.gameObject);
        }
        foreach (RecipeSO recipeSO in DeliveryManeger.Instance.GetWaitingResipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplete, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManegerSinglUI>().SetRecipeSO(recipeSO);

        }

    }
}
