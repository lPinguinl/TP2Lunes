using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject, IPrototype<PlateKitchenObject>
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectsSO kitchenObjectsSO;
    }
    
    [SerializeField] private List<KitchenObjectsSO> validKitchenObjectsSOList;
    private List<KitchenObjectsSO> kitchenObjectSOList;


    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectsSO>();
    }
    
    public bool TrAddIngredient(KitchenObjectsSO kitchenObjectSO)
    {
        if (!validKitchenObjectsSOList.Contains(kitchenObjectSO))
        {
            //Ingrediente no valido
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //Ya tiene este tipo
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs 
                { kitchenObjectsSO = kitchenObjectSO });
            
            return true;
        }

    }

    public List<KitchenObjectsSO> GetKitchenObjectsSOList()
    {
        return kitchenObjectSOList;
    }
    
    public PlateKitchenObject Clone()
    {
        return Instantiate(this); // Clona el objeto usando Instantiate
    }

    public void Reset()
    {
        // Reinicia el estado del plato
        kitchenObjectSOList.Clear();
    }

    public void DestroySelf()
    {
        // Notifica al pool (si corresponde) antes de destruir
        if (GetKitchenObjectParent() is PlatesCounter counter)
        {
            counter.ReturnPlateToPool(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
