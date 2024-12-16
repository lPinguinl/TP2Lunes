using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    private bool _showConsole;
    private string _input;
    private CommandRegistry _commandRegistry;

    private void Awake()
    {
        // Inicializar el registro de comandos
        _commandRegistry = new CommandRegistry();
        _commandRegistry.RegisterCommand(new WinLevelCommand());
        _commandRegistry.RegisterCommand(new ChangeRecipeCommand());
    }

    public void OnToggleDebug()
    {
        _showConsole = !_showConsole; // Alternar la visibilidad de la consola
    }

    public void OnReturn()
    {
        if (_showConsole)
        {
            if (!_commandRegistry.TryExecuteCommand(_input))
            {
                Debug.LogWarning("Command not recognized."); // Comando no encontrado
            }
            _input = string.Empty; // Limpiar la entrada
        }
    }

    private void OnGUI()
    {
        if (!_showConsole) return;

        float y = 0f;
        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        _input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), _input);
    }
}
