using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : IState
{
    private readonly GameManager gameManager;

    public GameOverState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter() { }

    public void Execute()
    {
        // Lógica de fin de juego
    }

    public void Exit() { }
}
