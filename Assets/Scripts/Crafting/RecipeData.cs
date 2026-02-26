using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Ingredient
{
    public ItemData item;
    public int amount;
}

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Inventory/Recipe")]
public class RecipeData : ScriptableObject
{
    public ItemData resultItem;
    public List<Ingredient> ingredients;
    public bool requiresStation; // For crafting stations
}
