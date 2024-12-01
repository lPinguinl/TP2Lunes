using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : IState
{
    private readonly GameManager gameManager;

    public VictoryState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter() { }

    public void Execute()
    {
        // Lógica de victoria, si es necesario
    }

    public void Exit() { }
}
