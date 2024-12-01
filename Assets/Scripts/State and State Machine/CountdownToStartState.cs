using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownToStartState : IState
{
    private readonly GameManager gameManager;
    public float Timer { get; private set; } // Añadir esta propiedad

    public CountdownToStartState(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.Timer = 3f; // Inicializa el temporizador aquí
    }

    public void Enter() { }

    public void Execute()
    {
        Timer -= Time.deltaTime;
        if (Timer < 0f)
        {
            gameManager.ChangeState(new GamePlayingState(gameManager));
        }
    }

    public void Exit() { }
}

