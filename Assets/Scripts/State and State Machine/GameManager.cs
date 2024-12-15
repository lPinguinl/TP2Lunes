using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnstateChanged;

    private StateMachine stateMachine;

    private float countdownToStart = 3f;

    public int expiredRecipeCount = 0; 
    private int completedRecipesCount = 0; 

    [SerializeField] private AudioManager audioManagerPrefab; // Referencia al prefab de AudioManager
    [SerializeField] private AudioClip cuttingClip; // Campo asignable del clip de corte

    private void Awake()
    {
        Instance = this;
        stateMachine = new StateMachine();
        stateMachine.ChangeState(new WaitingToStartState(this));

        // Instanciar el AudioManager desde el Prefab
        AudioManager audioManager = Instantiate(audioManagerPrefab, transform);
        AudioSource audioSource = audioManager.GetComponent<AudioSource>();

        var audioClips = new Dictionary<string, AudioClip>
        {
            { "cuttingSound", cuttingClip }
        };
        audioManager.Initialize(audioSource, audioClips);

        // Registrar en el Service Locator
        ServiceLocator.Register<IAudioManager>(audioManager);
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
        expiredRecipeCount = 0;
        completedRecipesCount = 0;
        ChangeState(new WaitingToStartState(this));
        DeliveryManager.Instance.ResetDeliveryManager(); 
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
            SetVictory(); 
        }
    }

    public float GetCountdownToStartTimer()
    {
        if (stateMachine.GetCurrentState() is CountdownToStartState countdownState)
        {
            return countdownState.Timer; 
        }
        return 0f;
    }
}
