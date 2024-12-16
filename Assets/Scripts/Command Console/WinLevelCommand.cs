using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinLevelCommand : IDebugCommand
{
    public string Id => "win_level";
    public string Description => "Automatically passes you to the next level of the game.";
    public string Format => "win_level";

    public void Execute(string input)
    {
        Loader.Load(Loader.Scene.GameSceneTwo); // Lógica para cargar el siguiente nivel
    }
}
