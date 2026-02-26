using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    // List of all recipes available in the game
    public List<RecipeData> allRecipes;
    
    // Check if we are near a crafting station
    public bool isNearStation = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Main method to attempt crafting an item
    public void CraftItem(RecipeData recipe)
    {
        // Check if  have enough materials
        if (InventoryManager.Instance.HasIngredients(recipe.ingredients))
        {
            // Check if the recipe requires a station and if   near one
            if (recipe.requiresStation && !isNearStation)
            {
                Debug.Log("Requires a Crafting Station!");
                return;
            }

            // Remove ingredients from inventory
            foreach (var ingredient in recipe.ingredients)
            {
                InventoryManager.Instance.RemoveItem(ingredient.item, ingredient.amount);
            }

            // Add the result item to inventory
            InventoryManager.Instance.AddItem(recipe.resultItem, 1);
            
            Debug.Log("Crafted: " + recipe.resultItem.itemName);
            
            // Refresh UI
            GameEvents.TriggerInventoryUpdated();
        }
    }
}