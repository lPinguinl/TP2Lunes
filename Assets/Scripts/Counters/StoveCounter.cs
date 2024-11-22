using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    
    public event EventHandler <IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    } ;
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;



    private void Start()
    {
        state = State.Idle;
    }
    
    private void Update()
    {
        if (HasKitchenObject())
        {

            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs 
                        { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });
                    
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        //Se cocino
                        
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        
                        
                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());
                    }

                    break;
                case State.Fried:
                    
                    burningTimer += Time.deltaTime;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs 
                        { progressNormalized = burningTimer / burningRecipeSO.burningTimerMax });


                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        //Se cocino
                        
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);


                        state = State.Burned;
                        
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs 
                            { progressNormalized = 0f});
                    }
                    break;
                case State.Burned:
                    break;
            }
        }

    }
    
    

    public override void Interact(PlayerInteractions playerInteractions)
    {
        if(!HasKitchenObject())
        {
            // No hay KitchenObject aca
            if (playerInteractions.HasKitchenObject())
            {
                //El player lleva algo
                if (HasRecipeWithInput(playerInteractions.GetKitchenObject().GetKitchenObjectsSO()))
                {
                    // El player lleva algo q se puede freir
                    playerInteractions.GetKitchenObject().SetKitchenObjectParent(this);
                    
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());
                    state = State.Frying;
                    fryingTimer = 0f;
                    
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs 
                        { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });
                    
                    
                }
                
            }
            else
            {
                //Player no tiene una goma
            }
        }
        else
        {
            // Hay KitchenObject aca
            if (playerInteractions.HasKitchenObject())
            {
                //Player lleva algo
                if (playerInteractions.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player tiene un plato
                    if (plateKitchenObject.TrAddIngredient(GetKitchenObject().GetKitchenObjectsSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        state = State.Idle;
                
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs 
                            { progressNormalized = 0f});
                    }
                    
                
                }
            }
            else
            {
                // El player no lleva nada
                GetKitchenObject().SetKitchenObjectParent(playerInteractions);
                
                state = State.Idle;
                
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs 
                    { progressNormalized = 0f});
            }
        }
    }
    
    private bool HasRecipeWithInput(KitchenObjectsSO inputkitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSo = GetFryingRecipeSOWithInput(inputkitchenObjectSO);
        return fryingRecipeSo != null;
    }

    private KitchenObjectsSO GetOutputForInput(KitchenObjectsSO inputkitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSo = GetFryingRecipeSOWithInput(inputkitchenObjectSO);
        if (fryingRecipeSo != null)
        {
            return fryingRecipeSo.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectsSO inputkitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputkitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectsSO inputkitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputkitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}

