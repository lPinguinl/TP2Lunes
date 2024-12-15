using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectFactory : IGenericFactory<KitchenObject>
{
    private KitchenObjectsSO _kitchenObjectsSO;

    public KitchenObjectFactory(KitchenObjectsSO kitchenObjectsSO)
    {
        _kitchenObjectsSO = kitchenObjectsSO;
    }

    public KitchenObject Create(IKitchenObjectParent kitchenObjectParent)
    {
        return KitchenObject.SpawnKitchenObject(_kitchenObjectsSO, kitchenObjectParent);
    }
}