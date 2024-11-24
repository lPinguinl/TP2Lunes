using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultExpirationStrategy : IExpirationStrategy
{
    public void HandleExpiration(List<(RecipeSO recipe, float timeRemaining)> recipeList)
    {
        for (int i = recipeList.Count - 1; i >= 0; i--)
        {
            if (recipeList[i].timeRemaining <= 0)
            {
                Debug.Log($"La receta '{recipeList[i].recipe.recipeName}' ha expirado.");
                recipeList.RemoveAt(i);
            }
        }
    }
}

