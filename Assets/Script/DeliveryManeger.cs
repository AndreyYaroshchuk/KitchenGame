using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class DeliveryManeger : NetworkBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeFailed;
    public event EventHandler OnRecipeSuccess;
    public static DeliveryManeger Instance { get; private set; } // ����������  �������� ��� ���������������� 
    [SerializeField] RecipeListSO recipeListSO;

    private float spawnRecipeTimer = 4f;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successRecipesAmount;
    private List<RecipeSO> waitingRecipeSOList;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer < 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (KicthenGameManeger.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipeMax)
            {
                int waitingRecipeSOIndex = UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count);
                SpawnNewRecipeClientRpc(waitingRecipeSOIndex);        
            }
        }
    }

    [ClientRpc]
    private void SpawnNewRecipeClientRpc( int waitingRecipeSOIndex)
    {
        RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[waitingRecipeSOIndex];
        waitingRecipeSOList.Add(waitingRecipeSO);
        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }
   
    public void DelirevyRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenObjectsSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) // ��������� � ��������
            {
                bool plateIngridientsMatchRecipe = true;

                foreach (KitchenObjectSO recipekitchenObjectSO in waitingRecipeSO.kitchenObjectsSOList)// �������� ��������� � ������� 
                {
                    bool ingridientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) // �������� ����������� �� �������
                    {
                        if (plateKitchenObjectSO == recipekitchenObjectSO) // ����������� ��������� 
                        {
                           
                            ingridientFound = true;
                            break;
                        }
                    }
                    if (!ingridientFound) // ������� �� ������� ������ ������� 
                    {
                        plateIngridientsMatchRecipe = false;

                    }
                }
                if (plateIngridientsMatchRecipe) // ����� ���� ���������� ������ 
                {
                    DelirevyCorrectRecipeServerRpc(i);
                    return;
                }
            }
        }

        // ��� ����������� ������� . ����� ���� �� ���������� �����
        DelirevyIncorrectRecipeServerRpc();

    }
    [ServerRpc(RequireOwnership = false)]
       private void DelirevyCorrectRecipeServerRpc(int waitingRecipiSOList)
    {
        DelirevyCorrectRecipeClientRpc(waitingRecipiSOList);
    }
    [ClientRpc]
    private void DelirevyCorrectRecipeClientRpc(int waitingRecipiSOList)
    {
        successRecipesAmount++;
        waitingRecipeSOList.RemoveAt(waitingRecipiSOList);

        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
    }
    [ServerRpc(RequireOwnership = false)]
    private void DelirevyIncorrectRecipeServerRpc()
    {
        DelirevyIncorrectRecipeClientRpc();
    }
    [ClientRpc]
    private void DelirevyIncorrectRecipeClientRpc()
    {
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
    public int GetSuccessRecipesAmount()
    {
        return successRecipesAmount;
    }
    public List<RecipeSO> GetWaitingResipeSOList()
    {
        return waitingRecipeSOList;
    }
}
