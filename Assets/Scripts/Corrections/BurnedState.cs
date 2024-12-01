using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnedState : IState
{
    private readonly StoveCounter stoveCounter;

    public BurnedState(StoveCounter stoveCounter)
    {
        this.stoveCounter = stoveCounter;
    }

    public void Enter()
    {
        stoveCounter.NotifyProgressChanged(0f);
    }

    public void Execute()
    {
        // Aquí puedes agregar cualquier lógica adicional para cuando el objeto esté quemado.
    }

    public void Exit() { }
}

