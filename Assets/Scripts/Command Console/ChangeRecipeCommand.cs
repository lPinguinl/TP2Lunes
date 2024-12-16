using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChangeRecipeCommand : IDebugCommand
{
    public string Id => "change_recipe";
    public string Description => "Replace the recipe you want to skip with a random one.";
    public string Format => "change_recipe <recipeName>";

    public void Execute(string input)
    {
        string recipeName = input.Replace($"{Id} ", "").Trim();
        DeliveryManager.Instance.ChangeRecipe(recipeName); // Cambia la receta
    }
}
