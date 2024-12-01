using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private StateMachine stateMachine;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.ChangeState(new IdleState(this));
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void ChangeState(IState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public FryingRecipeSO GetFryingRecipeSO()
    {
        return fryingRecipeSO;
    }

    public BurningRecipeSO GetBurningRecipeSO()
    {
        return burningRecipeSO;
    }

    public void SetBurningRecipeSO(BurningRecipeSO recipe)
    {
        burningRecipeSO = recipe;
    }

    public void NotifyProgressChanged(float progressNormalized)
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = progressNormalized });
    }

    public override void Interact(PlayerInteractions playerInteractions)
    {
        if(!HasKitchenObject())
        {
            if (playerInteractions.HasKitchenObject())
            {
                if (HasRecipeWithInput(playerInteractions.GetKitchenObject().GetKitchenObjectsSO()))
                {
                    playerInteractions.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());
                    ChangeState(new FryingState(this));
                }
            }
        }
        else
        {
            if (playerInteractions.HasKitchenObject())
            {
                if (playerInteractions.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TrAddIngredient(GetKitchenObject().GetKitchenObjectsSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        ChangeState(new IdleState(this));
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(playerInteractions);
                ChangeState(new IdleState(this));
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectsSO inputkitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSo = GetFryingRecipeSOWithInput(inputkitchenObjectSO);
        return fryingRecipeSo != null;
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

    public BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectsSO inputkitchenObjectSO)
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
