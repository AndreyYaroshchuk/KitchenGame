using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class CuttingCounter : BaseCounter, IHasProgress
{

    public static event EventHandler OnAnyCut; //
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler OnCutt;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;


    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSoArray;
    [SerializeField] int cuttingProgress;

    public override void Interact(Player player)
    {

        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // игрок не может положить объект на стол тот который нельзя нарезать 
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);
                    InteractCuttingCounterServerRpc();
                }

            }
            else
            {

            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // игрок держит тарелку 


                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                     KitchenObject.DestroySelfKitchenObject(GetKitchenObject());
                    }

                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    [ServerRpc (RequireOwnership = false)]
    private void InteractCuttingCounterServerRpc()
    {
        InteractCuttingCounterClientRpc();
    }
    [ClientRpc]
    private void InteractCuttingCounterClientRpc()
    {
        cuttingProgress = 0;


        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNirmalized = 0f
        }) ; 
    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // лежит кух объект который можно нарезать 
            CutObjectServerRpc();
            CuttingPtogresDoneServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        CutObjectClientRpc();
    }
    [ClientRpc]
    private void CutObjectClientRpc()
    {
        cuttingProgress++;
        OnCutt?.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNirmalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
        });
    }

    [ServerRpc (RequireOwnership = false)]
    private void CuttingPtogresDoneServerRpc()
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
        if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
        {
            KitchenObjectSO outputKitchenObkectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            KitchenObject.DestroySelfKitchenObject(GetKitchenObject());
            KitchenObject.SpawnKitchenObject(outputKitchenObkectSO, this);

        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSo)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSo);
        return cuttingRecipeSO != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSo)
    {
        CuttingRecipeSO cuttingRecipeSO =GetCuttingRecipeSOWithInput(inputKitchenObjectSo);
        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSo)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSoArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSo)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }


}
