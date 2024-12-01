using System;
using UnityEngine;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnstateChanged;

    private StateMachine stateMachine;

    private float countdownToStart = 3f; // Añadir este campo

    public int expiredRecipeCount = 0; // Añadir este campo
    private int completedRecipesCount = 0; // Añadir este campo

    private void Awake()
    {
        Instance = this;
        stateMachine = new StateMachine();
        stateMachine.ChangeState(new WaitingToStartState(this));
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void ChangeState(IState newState)
    {
        stateMachine.ChangeState(newState);
        OnstateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetGameOver()
    {
        ChangeState(new GameOverState(this));
    }

    public void SetVictory()
    {
        ChangeState(new VictoryState(this));
    }

    public void ResetGame()
    {
        // Resetea contadores y estados
        expiredRecipeCount = 0;
        completedRecipesCount = 0;
        ChangeState(new WaitingToStartState(this));
        DeliveryManager.Instance.ResetDeliveryManager(); // Resetea el DeliveryManager
    }

    public bool IsGamePlaying()
    {
        return stateMachine.GetCurrentState() is GamePlayingState;
    }

    public bool IsCountdownToStartActive()
    {
        return stateMachine.GetCurrentState() is CountdownToStartState;
    }

    public bool IsStateGameOver()
    {
        return stateMachine.GetCurrentState() is GameOverState;
    }

    public bool IsVictory()
    {
        return stateMachine.GetCurrentState() is VictoryState;
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
        if (stateMachine.GetCurrentState() is CountdownToStartState countdownState)
        {
            return countdownState.Timer; // Asegúrate de que Timer es accesible
        }
        return 0f;
    }
}
