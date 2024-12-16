using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRegistry
{
    private readonly Dictionary<string, IDebugCommand> _commands = new();

    // Registrar comandos en el diccionario
    public void RegisterCommand(IDebugCommand command)
    {
        if (!_commands.ContainsKey(command.Id))
        {
            _commands.Add(command.Id, command);
        }
    }

    // Ejecutar el comando basado en la entrada
    public bool TryExecuteCommand(string input)
    {
        foreach (var command in _commands.Values)
        {
            if (input.StartsWith(command.Id))
            {
                command.Execute(input);
                return true;
            }
        }
        return false; // Comando no reconocido
    }

    // Listar todos los comandos registrados
    public IEnumerable<IDebugCommand> GetAllCommands()
    {
        return _commands.Values;
    }
}
