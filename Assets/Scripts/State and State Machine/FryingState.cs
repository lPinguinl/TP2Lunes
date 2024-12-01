using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingState : IState
{
    private readonly StoveCounter stoveCounter;
    private float fryingTimer;

    public FryingState(StoveCounter stoveCounter)
    {
        this.stoveCounter = stoveCounter;
    }

    public void Enter()
    {
        fryingTimer = 0f;
        stoveCounter.NotifyProgressChanged(0f);
    }

    public void Execute()
    {
        fryingTimer += Time.deltaTime;
        stoveCounter.NotifyProgressChanged(fryingTimer / stoveCounter.GetFryingRecipeSO().fryingTimerMax);

        if (fryingTimer > stoveCounter.GetFryingRecipeSO().fryingTimerMax)
        {
            stoveCounter.GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(stoveCounter.GetFryingRecipeSO().output, stoveCounter);
            stoveCounter.SetBurningRecipeSO(stoveCounter.GetBurningRecipeSOWithInput(stoveCounter.GetKitchenObject().GetKitchenObjectsSO()));
            stoveCounter.ChangeState(new FriedState(stoveCounter));
        }
    }

    public void Exit() { }
}
