using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(PlayerInteractions playerInteractions)
    {
        if (playerInteractions.HasKitchenObject())
        {
            if (playerInteractions.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                //Solo acepta los platos

                DeliveryManager.Instance.DeliveryRecipe(plateKitchenObject);
                
                playerInteractions.GetKitchenObject().DestroySelf();
            }
        }
    }
}
