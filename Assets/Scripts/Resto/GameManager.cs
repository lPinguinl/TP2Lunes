using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnstateChanged;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
        Victory,
        FinalVictory,
    }

    private State state;
    private float waitingToStartTimer = 1f;
    private float countdownToStart = 3f;

    public int expiredRecipeCount = 0;
    private int completedRecipesCount = 0;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f)
                {
                    state = State.CountdownToStart;
                    OnstateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                countdownToStart -= Time.deltaTime;
                if (countdownToStart < 0f)
                {
                    state = State.GamePlaying;
                    OnstateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                // Lógica del juego en curso
                break;
            case State.GameOver:
                // Lógica de fin de juego
                break;
            case State.Victory:
                // Lógica de victoria, si es necesario
                break;
            case State.FinalVictory:
                break;
        }
        //Debug.Log(state);
    }

    public void SetGameOver()
    {
        state = State.GameOver;
        OnstateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetVictory()
    {
        state = State.Victory;
        OnstateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ResetGame()
    {
        expiredRecipeCount = 0;
        completedRecipesCount = 0;
        state = State.WaitingToStart; // O el estado inicial deseado
        DeliveryManager.Instance.ResetDeliveryManager(); // Resetea el DeliveryManager
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public bool IsStateGameOver()
    {
        return state == State.GameOver;
    }

    public bool IsVictory()
    {
        return state == State.Victory;
    }

    public void CheckVictoryCondition(int completedCount)
    {
        if (completedCount >= 4)
        {
            SetVictory(); // Cambia a estado de victoria
        }
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStart;
    }
}
