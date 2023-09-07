using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{

    public static DeliveryCounter Instance { get; private set; }
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))// ������ ��������� �������
            {
                DeliveryManeger.Instance.DelirevyRecipe(plateKitchenObject);
                KitchenObject.DestroySelfKitchenObject(player.GetKitchenObject()); 
            }
        }
    }
}
