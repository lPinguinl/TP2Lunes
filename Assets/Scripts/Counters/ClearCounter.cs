using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;
   

    
    public override void Interact(PlayerInteractions playerInteractions)
    {
        if(!HasKitchenObject())
        {
            // No hay KitchenObject aca
            if (playerInteractions.HasKitchenObject())
            {
                //El player lleva algo
                playerInteractions.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //Player no tiene una goma
            }
        }
        else
        {
            // Hay KitchenObject aca
            if (playerInteractions.HasKitchenObject())
            {
                //Player lleva algo
                if (playerInteractions.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player tiene un plato
                    if (plateKitchenObject.TrAddIngredient(GetKitchenObject().GetKitchenObjectsSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                
                }
                else
                {
                   //Player no tiene plato pero otra cosa
                   if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                   {
                       //Counter tine un plato
                       if(plateKitchenObject.TrAddIngredient(playerInteractions.GetKitchenObject().GetKitchenObjectsSO()))
                       {
                           playerInteractions.GetKitchenObject().DestroySelf();

                       }
                   }
                }
            }
            else
            {
                //Player no lleva nada
                GetKitchenObject().SetKitchenObjectParent(playerInteractions);
            }
        }
    }

}