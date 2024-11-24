using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExpirationStrategy
{
    void HandleExpiration(List<(RecipeSO recipe, float timeRemaining)> recipeList);
}

