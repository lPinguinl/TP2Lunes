using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    
    private IGenericFactory<KitchenObject> _kitchenObjectFactory;
        
    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;
    
    private void Awake()
    {
        _kitchenObjectFactory = new KitchenObjectFactory(kitchenObjectsSO);
    }

    public override void Interact(PlayerInteractions playerInteractions)
    {
        if (!playerInteractions.HasKitchenObject())
        {
            // Utiliza la fábrica para crear el objeto de cocina
            KitchenObject kitchenObject = _kitchenObjectFactory.Create(playerInteractions);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}