using System;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button backMainMenuButton;

    private void Awake()
    {
        nextLevelButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ResetGame(); // Resetea el estado del juego
            
            Loader.Load(Loader.Scene.GameSceneTwo); // Cargar el siguiente nivel
            
        });

        backMainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnstateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsVictory())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}