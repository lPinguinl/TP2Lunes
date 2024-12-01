using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private readonly StoveCounter stoveCounter;

    public IdleState(StoveCounter stoveCounter)
    {
        this.stoveCounter = stoveCounter;
    }

    public void Enter() { }

    public void Execute() 
    {

    }

    public void Exit() { }
}
