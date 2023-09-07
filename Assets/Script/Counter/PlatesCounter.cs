using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateDestroyed;
    [SerializeField] KitchenObjectSO plateKitchenObjectSO;



    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            if (KicthenGameManeger.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                PlateSpawnServerRpc();
            }
        }
    }
    [ServerRpc]
    private void PlateSpawnServerRpc()
    {
        PlateSpawnClientRpc();
    }
    [ClientRpc]
    private void PlateSpawnClientRpc()
    {
        platesSpawnedAmount++;
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
    }
    
    public override void Interact(Player player)
    {

        if (!player.HasKitchenObject())
        {
            if(platesSpawnedAmount > 0)
            {
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                InteractLogeServerRpc();
            }
        }

    }
    [ServerRpc (RequireOwnership =false)]
    private void InteractLogeServerRpc()
    {
        InteractLogeClientRpc();
    }
    [ClientRpc]
    private void InteractLogeClientRpc()
    {
        platesSpawnedAmount--;
     
        OnPlateDestroyed?.Invoke(this, EventArgs.Empty);
    }


}

