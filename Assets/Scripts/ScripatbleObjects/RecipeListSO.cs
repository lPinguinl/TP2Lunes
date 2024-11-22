using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
El CreateAssetMenu esta comentado ya que solo creamos un unico Recipe List, osea lista de recetas
 y lo implementamos una unica vez y para no crear confuciones a la hora de crear SO lo sacamos del creador asi no 
  Creamos otra lista de recetas sin querer

//[CreateAssetMenu()] (Sacar esto y de esta parte de ser necesario (PD: No creo que necesiten sacarlo))

*/
[CreateAssetMenu()]
public class RecipeListSO : ScriptableObject
{
    public List<RecipeSO> recipeSOList;
}
