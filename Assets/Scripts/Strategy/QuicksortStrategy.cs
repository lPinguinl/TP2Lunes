using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSortStrategy : ISortingStrategy
{
    public void Sort(List<(RecipeSO recipe, float timeRemaining)> recipeList)
    {
        Quicksort(recipeList, 0, recipeList.Count - 1);
    }

    private void Quicksort(List<(RecipeSO recipe, float timeRemaining)> list, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(list, left, right);
            Quicksort(list, left, pivotIndex - 1);
            Quicksort(list, pivotIndex + 1, right);
        }
    }

    private int Partition(List<(RecipeSO recipe, float timeRemaining)> list, int left, int right)
    {
        var pivot = list[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (list[j].recipe.priority > pivot.recipe.priority ||
                (list[j].recipe.priority == pivot.recipe.priority && list[j].timeRemaining < pivot.timeRemaining))
            {
                i++;
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        (list[i + 1], list[right]) = (list[right], list[i + 1]);
        return i + 1;
    }
}
