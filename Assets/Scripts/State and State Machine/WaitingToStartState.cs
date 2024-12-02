using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingToStartState : IState
{
    private readonly GameManager gameManager;
    private float timer;

    public WaitingToStartState(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.timer = 1f; 
    }

    public void Enter() { }

    public void Execute()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            gameManager.ChangeState(new CountdownToStartState(gameManager));
        }
    }

    public void Exit() { }
}
