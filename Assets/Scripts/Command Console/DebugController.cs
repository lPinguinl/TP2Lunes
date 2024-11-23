using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    
    string input;
    
    public static DebugCommandBase.DebugCommand WIN_LEVEL;
    public static DebugCommandBase.DebugCommand CHANGE_RECIPE;

    public List<object> commandList;

    public void OnToggleDebug()
    {
        showConsole = !showConsole;
    }

    public void OnReturn()
    {
        if (showConsole)
        {
            HandleInput();
            input = "";
            
        }
    }


    private void Awake()
    {
        WIN_LEVEL = new DebugCommandBase.DebugCommand
            (
                "win_level", 
                "Automatically passes you to the next level of the game", 
                "win_level", 
                () => Loader.Load(Loader.Scene.GameSceneTwo)
            );
        
        CHANGE_RECIPE = new DebugCommandBase.DebugCommand(
            "change_recipe",
            "Replace the recipe you want to skip with a random one",
            "change_recipe <recipeName>",
            () =>
            {
                string recipeName = input.Replace("change_recipe ", "").Trim();
                DeliveryManager.Instance.ChangeRecipe(recipeName);
            }
        );


        commandList = new List<object>()
        {
            WIN_LEVEL,CHANGE_RECIPE,
        };
    }


    private void OnGUI()
    {
        if (!showConsole) {return;}

        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0 ,0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }


    private void HandleInput()
    {
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (input.Contains(commandBase.CommandId))
            {
                if (commandList[i] as DebugCommandBase.DebugCommand != null)
                {
                    //Ejecuta el comando
                    (commandList[i] as DebugCommandBase.DebugCommand)?.Invoke();
                }
            }
        }
    }
    
    
}
