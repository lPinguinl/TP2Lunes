using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriedState : IState
{
    private readonly StoveCounter stoveCounter;
    private float burningTimer;

    public FriedState(StoveCounter stoveCounter)
    {
        this.stoveCounter = stoveCounter;
    }

    public void Enter()
    {
        burningTimer = 0f;
        stoveCounter.NotifyProgressChanged(0f);
    }

    public void Execute()
    {
        burningTimer += Time.deltaTime;
        BurningRecipeSO burningRecipeSO = stoveCounter.GetBurningRecipeSO();

        if (burningRecipeSO != null)
        {
            stoveCounter.NotifyProgressChanged(burningTimer / burningRecipeSO.burningTimerMax);

            if (burningTimer > burningRecipeSO.burningTimerMax)
            {
                stoveCounter.GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(burningRecipeSO.output, stoveCounter);
                stoveCounter.ChangeState(new BurnedState(stoveCounter));
            }
        }
        else
        {
            stoveCounter.ChangeState(new BurnedState(stoveCounter));
        }
    }

    public void Exit() { }
}

