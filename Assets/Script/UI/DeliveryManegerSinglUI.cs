using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManegerSinglUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] Transform iconContainer;
    [SerializeField] Transform ingridientImage;

    private void Awake()
    {
        ingridientImage.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        recipeNameText.text = recipeSO.recipeName;

        foreach (Transform child in iconContainer)
        {
            if (child == iconContainer) continue;
            child.gameObject.SetActive(false);
            //Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectsSOList)
        {
            Transform iconTransform = Instantiate(ingridientImage, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().enabled = true;
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
           

        }
    }

}
