using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecipeGenerationStrategy
{
    RecipeSO GenerateRecipe(List<RecipeSO> availableRecipes);
}

