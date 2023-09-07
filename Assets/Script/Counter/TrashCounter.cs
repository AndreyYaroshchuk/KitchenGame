using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnObjectTrash;
    new public static void ResetStaticData()
    {
        OnObjectTrash = null;
    }
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObject.DestroySelfKitchenObject(player.GetKitchenObject());
            InteractServerRpc();
        }
    }
    [ServerRpc (RequireOwnership = false)] 
    private void InteractServerRpc()
    {
        InteractClientRpc();
    }
    [ClientRpc] 
    private void InteractClientRpc()
    {
        
        OnObjectTrash?.Invoke(this, EventArgs.Empty);
    }
}
