using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
        
    
        [SerializeField] private KitchenObjectsSO kitchenObjectsSO;
        public override void Interact(PlayerInteractions playerInteractions)
        {
            if (!playerInteractions.HasKitchenObject())
            {
                //El player no lleva nada encima pa
                KitchenObject.SpawnKitchenObject(kitchenObjectsSO, playerInteractions);
                
                
                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            }
           
        }
                

}
