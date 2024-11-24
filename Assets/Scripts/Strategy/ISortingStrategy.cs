using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISortingStrategy
{
    void Sort(List<(RecipeSO recipe, float timeRemaining)> recipeList);
}
