using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayingState : IState
{
    private readonly GameManager gameManager;

    public GamePlayingState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter() { }

    public void Execute()
    {

    }

    public void Exit() { }
}
