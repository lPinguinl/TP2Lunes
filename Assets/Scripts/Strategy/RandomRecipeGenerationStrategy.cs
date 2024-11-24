using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRecipeGenerationStrategy : IRecipeGenerationStrategy
{
    public RecipeSO GenerateRecipe(List<RecipeSO> availableRecipes)
    {
        return availableRecipes[Random.Range(0, availableRecipes.Count)];
    }
}
