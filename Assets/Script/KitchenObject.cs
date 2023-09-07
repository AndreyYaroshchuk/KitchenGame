using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    public KitchenObjectSO KitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;
    private FollowTransform followTransform;

    protected virtual void Awake()
    {
        followTransform = GetComponent<FollowTransform>();
    }
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return KitchenObjectSO;
    }
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());

    }
    [ServerRpc (RequireOwnership = false)]
     public void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectReference);
    }
    [ClientRpc]
    public void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetwork);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetwork.GetComponent<IKitchenObjectParent>();
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }


        this.kitchenObjectParent = kitchenObjectParent;


        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.Log("ָלוועסÿ מבתוךע");
        }
        kitchenObjectParent.SetKitchenObject(this);

        followTransform.SetTargetTransform(kitchenObjectParent.GetKitchenObjectFollowTransform());
    }
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
    public void DestroySelf()
    {
        
        Destroy(gameObject);
    }
    public void ClearKitchenObjectOnParent()
    {
        kitchenObjectParent.ClearKitchenObject();
    }
    public static void DestroySelfKitchenObject(KitchenObject kitchenObject)
    {
        KichenGameMultipler.Instance.DestroySelfKitchenObject(kitchenObject);
    }
    
    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        KichenGameMultipler.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
    }
   
    public bool TryGetPlate( out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
        
    }

}
