using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(PlayerInteractions playerInteractions)
    {
        if (playerInteractions.HasKitchenObject())
        {
            playerInteractions.GetKitchenObject().DestroySelf();
        }
    }
}
