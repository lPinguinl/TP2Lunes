using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenericFactory<T>
{
    T Create(IKitchenObjectParent kitchenObjectParent);
}
