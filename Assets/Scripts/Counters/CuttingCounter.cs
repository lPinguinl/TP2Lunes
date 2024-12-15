using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(PlayerInteractions playerInteractions)
    {
        if (!HasKitchenObject())
        {
            if (playerInteractions.HasKitchenObject())
            {
                if (HasRecipeWithInput(playerInteractions.GetKitchenObject().GetKitchenObjectsSO()))
                {
                    playerInteractions.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingPorgressMax
                    });
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
            cuttingProgress++;

            // Invocar el evento de corte
            OnCut?.Invoke(this, EventArgs.Empty);

            // Pedir al AudioManager que reproduzca el sonido
            ServiceLocator.Get<IAudioManager>().PlaySound("cuttingSound");

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

                // Detener el sonido cuando se completa el corte
                ServiceLocator.Get<IAudioManager>().StopSound("cuttingSound");
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
