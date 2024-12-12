using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> kitchenSprites;

    private KitchenObjectFlyweightFactory factory = new KitchenObjectFlyweightFactory();
    private List<KitchenObjectWrapper> kitchenObjects = new List<KitchenObjectWrapper>();

    private void Start()
    {
        // Verificación de límites
        if (kitchenSprites.Count > 0)
        {
            CreateKitchenObject("knife_01", "Knife", kitchenSprites[0]);
            if (kitchenSprites.Count > 1)
            {
                CreateKitchenObject("pan_01", "Pan", kitchenSprites[1]);
            }
        }
        else
        {
            Debug.LogError("La lista kitchenSprites está vacía. Asegúrate de asignar sprites en el Inspector.");
        }
    }

    private void CreateKitchenObject(string uniqueID, string objectName, Sprite sprite)
    {
        var flyweight = factory.GetFlyweight(objectName, sprite);
        var kitchenObject = new GameObject(objectName).AddComponent<KitchenObject>();
        var wrapper = new GameObject(objectName + "_Wrapper").AddComponent<KitchenObjectWrapper>();
        wrapper.Initialize(kitchenObject, flyweight);
        kitchenObjects.Add(wrapper);
    }
}   