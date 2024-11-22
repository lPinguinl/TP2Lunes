using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    
    public event EventHandler <IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
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
                    // El player lleva algo q se puede cortar
                    playerInteractions.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingPorgressMax
                    });
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
                    }
                
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(playerInteractions);
            }
        }
        
    }
    public override void Interactuar(PlayerInteractions playerInteractions)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectsSO())) 
        {
            // Hay un KitchenObject aca Y se puede cortar
            cuttingProgress++;
            
            OnCut?.Invoke(this, EventArgs.Empty);
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());
            
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingPorgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingPorgressMax)
            {
                KitchenObjectsSO outputKitchenObjectsSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectsSO());
            
                GetKitchenObject().DestroySelf();
            
                KitchenObject.SpawnKitchenObject(outputKitchenObjectsSO, this);
            }

        }
    }


    private bool HasRecipeWithInput(KitchenObjectsSO inputkitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputkitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectsSO GetOutputForInput(KitchenObjectsSO inputkitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputkitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
           return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectsSO inputkitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputkitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
