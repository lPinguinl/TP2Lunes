using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectFlyweight
{
    public string ObjectName { get; private set; }
    public Sprite Sprite { get; private set; }

    public KitchenObjectFlyweight(string objectName, Sprite sprite)
    {
        ObjectName = objectName;
        Sprite = sprite;
    }
}