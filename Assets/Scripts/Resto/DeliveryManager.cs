using System;
using System.Collections.Generic;
using UnityEngine;

/*

 Ahora usamos un TDA ABB para manejar las órdenes de cocina.
 
 Implementamos Quicksort sobre los datos extraídos del ABB para mantener el orden dinámico requerido.

*/

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private BinarySearchTree<(RecipeSO recipe, float timeRemaining)> recipeTree;
    private List<(RecipeSO recipe, float timeRemaining)> sortedRecipeList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;

    private int expiredRecipeCount = 0;
    private int completedRecipesCount = 0;

    private void Awake()
    {
        Instance = this;

        // Inicializamos el ABB con un comparador que prioriza prioridad y tiempo
        recipeTree = new BinarySearchTree<(RecipeSO recipe, float timeRemaining)>((a, b) =>
        {
            if (a.recipe.priority != b.recipe.priority)
                return b.recipe.priority.CompareTo(a.recipe.priority); // Orden descendente por prioridad
            return a.timeRemaining.CompareTo(b.timeRemaining); // Orden ascendente por tiempo
        });

        sortedRecipeList = new List<(RecipeSO recipe, float timeRemaining)>();
    }

    private void Update()
    {
        // Generación de nuevas recetas
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (recipeTree.Count() < 4) // Limite máximo de recetas activas
            {
                RecipeSO newRecipe = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                recipeTree.Insert((newRecipe, 60f)); // Tiempo inicial de 60 segundos
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }

        // Actualizar tiempos de las recetas
        foreach (var (recipe, timeRemaining) in recipeTree.InOrderTraversal())
        {
            if (timeRemaining > 0)
            {
                recipeTree.Remove((recipe, timeRemaining));
                recipeTree.Insert((recipe, timeRemaining - Time.deltaTime));
            }
            else
            {
                recipeTree.Remove((recipe, timeRemaining));
                Debug.Log($"La receta '{recipe.recipeName}' ha expirado.");
                expiredRecipeCount++;
                if (expiredRecipeCount >= 4)
                {
                    Debug.Log("Se han perdido 4 recetas. Fin del juego.");
                    GameManager.Instance.SetGameOver();
                }
            }
        }

        // Ordenar recetas dinámicamente
        UpdateSortedRecipes();
    }

    private void UpdateSortedRecipes()
    {
        sortedRecipeList = new List<(RecipeSO recipe, float timeRemaining)>(recipeTree.InOrderTraversal());

        Quicksort(sortedRecipeList, 0, sortedRecipeList.Count - 1);
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

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        foreach (var (recipe, timeRemaining) in  recipeTree.InOrderTraversal())
        {
            if (recipe.kitchenObjectsSOList.Count == plateKitchenObject.GetKitchenObjectsSOList().Count)
            {
                bool plateContentsMatchesRecipe = true;

                foreach (KitchenObjectsSO recipeKitchenObjectsSO in recipe.kitchenObjectsSOList)
                {
                    bool ingredientFound = false;

                    foreach (KitchenObjectsSO plateKitchenObjectsSO in plateKitchenObject.GetKitchenObjectsSOList())
                    {
                        if (plateKitchenObjectsSO == recipeKitchenObjectsSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                        break;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    Debug.Log("¡Pedido entregado correctamente!");
                    recipeTree.Remove((recipe, timeRemaining)); // Eliminamos la receta
                    completedRecipesCount++;
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    GameManager.Instance.CheckVictoryCondition(completedRecipesCount);
                    return;
                }
            }
        }

        Debug.Log("Pedido incorrecto.");
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        var recipes = new List<RecipeSO>();
        foreach (var (recipe, timeRemaining) in recipeTree.InOrderTraversal())
        {
            recipes.Add(recipe);
        }
        return recipes;
    }

    public void ResetDeliveryManager()
    {
        recipeTree = new BinarySearchTree<(RecipeSO recipe, float timeRemaining)>((a, b) =>
        {
            if (a.recipe.priority != b.recipe.priority)
                return b.recipe.priority.CompareTo(a.recipe.priority);
            return a.timeRemaining.CompareTo(b.timeRemaining);
        });
    }
}
