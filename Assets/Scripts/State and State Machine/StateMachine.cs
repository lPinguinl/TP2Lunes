using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState currentState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Execute();
    }
    public IState GetCurrentState() => currentState;
}
