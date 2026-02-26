using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    [Header("Master Data")]
    public List<RecipeData> recipes; 

    [HideInInspector] 
    public bool isNearStation = false; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Craft(RecipeData recipe)
    {
        // Check materials
        if (!InventoryManager.Instance.HasIngredients(recipe.ingredients)) return;

        // Check station requirement
        if (recipe.requiresStation && !isNearStation) 
        {
            Debug.Log("Need a Crafting Station for this item!");
            return;
        }

        // Consume materials
        foreach (var ing in recipe.ingredients)
        {
            InventoryManager.Instance.RemoveItem(ing.item, ing.amount);
        }

        // Add result
        InventoryManager.Instance.AddItem(recipe.resultItem, 1);
        GameEvents.TriggerInventoryUpdated();
    }
}