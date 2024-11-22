using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public virtual void Interact(PlayerInteractions playerInteractions)
    {
        Debug.Log("BaseCounter.Interact();");
    }
    public virtual void Interactuar(PlayerInteractions playerInteractions)
    {
        //Debug.Log("BaseCounter.Interactuar();");
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }
            
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }
        
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
        
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
        
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
