using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectWrapper : MonoBehaviour
{
    private KitchenObjectFlyweight flyweight;
    private KitchenObject kitchenObject;

    public void Initialize(KitchenObject kitchenObject, KitchenObjectFlyweight flyweight)
    {
        this.kitchenObject = kitchenObject;
        this.flyweight = flyweight;
    }

    public string GetObjectName()
    {
        return flyweight.ObjectName;
    }

    public Sprite GetSprite()
    {
        return flyweight.Sprite;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
}
