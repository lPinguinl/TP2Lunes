using UnityEngine;
using System;
using System.Collections.Generic;

public class PlateKitchenObject : KitchenObject, IPrototype<PlateKitchenObject>
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectsSO kitchenObjectsSO;
    }

    [SerializeField] private List<KitchenObjectsSO> validKitchenObjectsSOList;
    private List<KitchenObjectsSO> kitchenObjectSOList;

    private static ObjectPool<PlateKitchenObject> pool; // Pool genérica

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectsSO>();
    }

    // Inicializar el pool genérico
    public static void InitializePool(PlateKitchenObject prefab, Transform parent)
    {
        pool = new ObjectPool<PlateKitchenObject>(prefab, parent);
    }

    // Obtener un plato del pool
    public static PlateKitchenObject GetFromPool()
    {
        PlateKitchenObject plate = pool.Get();
        plate.kitchenObjectSOList.Clear(); // Limpia ingredientes al obtener el plato
        return plate;
    }

    // Devolver un plato al pool
    public static void ReturnToPool(PlateKitchenObject plate)
    {
        // Limpia la lista de ingredientes antes de devolver el plato al pool
        plate.kitchenObjectSOList.Clear();

        // Devuelve el plato al pool
        pool.Return(plate);
    }

    // Método para agregar un ingrediente al plato
    public bool TrAddIngredient(KitchenObjectsSO kitchenObjectSO)
    {
        if (!validKitchenObjectsSOList.Contains(kitchenObjectSO))
        {
            // Ingrediente no válido
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Ya tiene este tipo
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);

            // Notificar que se agregó un ingrediente
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectsSO = kitchenObjectSO
            });

            return true;
        }
    }

    // Obtener la lista de ingredientes
    public List<KitchenObjectsSO> GetKitchenObjectsSOList()
    {
        return kitchenObjectSOList;
    }

    // Implementación del patrón Prototype
    public PlateKitchenObject Clone()
    {
        PlateKitchenObject clone = Instantiate(this);
        clone.kitchenObjectSOList = new List<KitchenObjectsSO>(this.kitchenObjectSOList); // Copiar ingredientes
        return clone;
    }
}
