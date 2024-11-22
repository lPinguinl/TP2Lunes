using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectsSO> kitchenObjectsSOList;
    
    public string recipeName;
    
    public int priority; // Prioridad de la receta
}

