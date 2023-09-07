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

            // нет кухон объекта;
            if(player.HasKitchenObject())
            {
                // у player есть объект в руках 
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // player нет объекта в руках 

            }
        }
        else
        {
            // есть кухон объекта;

            if(player.HasKitchenObject())
            {
                // у player есть объект в руках 
                
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // игрок держит тарелку 
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        
                        KitchenObject.DestroySelfKitchenObject(GetKitchenObject());
                    }

                }
                else
                {
                    // ирок не нисет тарелку в руках , у него в руках кухонный объект
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // на столешнице лежит тарелка
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            KitchenObject.DestroySelfKitchenObject(player.GetKitchenObject());
                        }

                    }
                }

            }
            else
            {
                // player нет объекта в руках 
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
