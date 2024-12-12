using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectFlyweightFactory
{
    private Dictionary<string, KitchenObjectFlyweight> flyweights = new Dictionary<string, KitchenObjectFlyweight>();

    public KitchenObjectFlyweight GetFlyweight(string objectName, Sprite sprite)
    {
        if (!flyweights.ContainsKey(objectName))
        {
            flyweights[objectName] = new KitchenObjectFlyweight(objectName, sprite);
        }

        return flyweights[objectName];
    }
}