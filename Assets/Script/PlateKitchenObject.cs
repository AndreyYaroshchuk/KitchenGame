using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddEventArgs> OnIngredientAdd;

    public class OnIngredientAddEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;

    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;

    

    protected override void Awake()
    {
        base.Awake();
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {

        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // не подходящий ингридиент 
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO) )
        {
            return false; // на тарелке есть объект такого типа 
            
        }
        else
        {
            
            AddIngredientServerRpc(KichenGameMultipler.Instance.GetKitchenObjectSOIndex(kitchenObjectSO));

            return true;
        }
  
    }
    [ServerRpc (RequireOwnership = false)]
    private void AddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredientClientRpc(kitchenObjectSOIndex);
    }
    [ClientRpc]
    private void AddIngredientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KichenGameMultipler.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdd?.Invoke(this, new OnIngredientAddEventArgs
        {
            kitchenObjectSO = kitchenObjectSO
        });

    }
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
    
}
