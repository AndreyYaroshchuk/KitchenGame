using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public override void Interact( Player player)
    {
        if (!HasKitchenObject())
        {

            // ��� ����� �������;
            if(player.HasKitchenObject())
            {
                // � player ���� ������ � ����� 
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // player ��� ������� � ����� 

            }
        }
        else
        {
            // ���� ����� �������;

            if(player.HasKitchenObject())
            {
                // � player ���� ������ � ����� 
                
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // ����� ������ ������� 
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        
                        KitchenObject.DestroySelfKitchenObject(GetKitchenObject());
                    }

                }
                else
                {
                    // ���� �� ����� ������� � ����� , � ���� � ����� �������� ������
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // �� ���������� ����� �������
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            KitchenObject.DestroySelfKitchenObject(player.GetKitchenObject());
                        }

                    }
                }

            }
            else
            {
                // player ��� ������� � ����� 
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
