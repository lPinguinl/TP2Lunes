using System;
using System.Collections.Generic;
using UnityEngine;

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
    
    private int completedRecipesCount = 0;

    private ISortingStrategy sortingStrategy;
    private IRecipeGenerationStrategy generationStrategy;
    private IExpirationStrategy expirationStrategy;

    private void Awake()
    {
        Instance = this;

        // Inicializar el árbol binario
        recipeTree = new BinarySearchTree<(RecipeSO recipe, float timeRemaining)>((a, b) =>
        {
            if (a.recipe.priority != b.recipe.priority)
                return b.recipe.priority.CompareTo(a.recipe.priority);
            return a.timeRemaining.CompareTo(b.timeRemaining);
        });

        sortedRecipeList = new List<(RecipeSO recipe, float timeRemaining)>();

        // Configurar estrategias por defecto
        SetSortingStrategy(new QuickSortStrategy());
        SetGenerationStrategy(new RandomRecipeGenerationStrategy());
        SetExpirationStrategy(new DefaultExpirationStrategy());
    }

    private void Update()
    {
        // Generación de nuevas recetas
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            GenerateNewRecipe();
        }

        // Manejo de expiración y ordenamiento
        HandleRecipeExpiration();
        UpdateSortedRecipes();
    }

    public void SetSortingStrategy(ISortingStrategy strategy)
    {
        sortingStrategy = strategy;
    }

    public void SetGenerationStrategy(IRecipeGenerationStrategy strategy)
    {
        generationStrategy = strategy;
    }

    public void SetExpirationStrategy(IExpirationStrategy strategy)
    {
        expirationStrategy = strategy;
    }

    private void GenerateNewRecipe()
    {
        if (generationStrategy != null && recipeTree.Count() < 4)
        {
            RecipeSO newRecipe = generationStrategy.GenerateRecipe(recipeListSO.recipeSOList);
            recipeTree.Insert((newRecipe, 60f));
            OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    private void UpdateSortedRecipes()
    {
        if (sortingStrategy != null)
        {
            sortedRecipeList = new List<(RecipeSO recipe, float timeRemaining)>(recipeTree.InOrderTraversal());
            sortingStrategy.Sort(sortedRecipeList);
        }
    }

    private void HandleRecipeExpiration()
    {
        if (expirationStrategy != null)
        {
            expirationStrategy.HandleExpiration(sortedRecipeList);
        }
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
    
    public void ChangeRecipe(string recipeName)
    {
        bool recipeFound = false;

        foreach (var (recipe, timeRemaining) in recipeTree.InOrderTraversal())
        {
            if (recipe.recipeName == recipeName)
            {
                // Aqui eliminamos la receta que ya existe en la UI
                recipeTree.Remove((recipe, timeRemaining));
                recipeFound = true;
                Debug.Log($"La receta '{recipeName}' ha sido reemplazada.");
                break;
            }
        }

        if (recipeFound)
        {
            // Agregamos una nueva receta aleatoria
            RecipeSO newRecipe = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
            recipeTree.Insert((newRecipe, 60f)); // Le ponemos el minuto de duracion desde 0
            OnRecipeSpawned?.Invoke(this, EventArgs.Empty); // Actualizacion en la UI
            Debug.Log($"La nueva receta es: {newRecipe.recipeName}");
        }
        else
        {
            Debug.Log($"No se encontró la receta con el nombre: {recipeName}");
        }
    }
    
}
