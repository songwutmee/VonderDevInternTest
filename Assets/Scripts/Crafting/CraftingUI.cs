using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public GameObject recipePrefab;
    public Transform recipeContainer;
    
    private List<CraftingRecipeUI> spawnedRecipes = new List<CraftingRecipeUI>();

    private void OnEnable()
    {
        GameEvents.OnInventoryUpdated += RefreshList;
    }

    private void OnDisable()
    {
        GameEvents.OnInventoryUpdated -= RefreshList;
    }

    private void Start()
    {
        InitializeList();
    }

    private void InitializeList()
    {
        // Clear old ones
        foreach (Transform child in recipeContainer) Destroy(child.gameObject);
        spawnedRecipes.Clear();

        // Create UI for each recipe
        foreach (RecipeData recipe in CraftingManager.Instance.allRecipes)
        {
            GameObject obj = Instantiate(recipePrefab, recipeContainer);
            CraftingRecipeUI ui = obj.GetComponent<CraftingRecipeUI>();
            ui.Setup(recipe);
            spawnedRecipes.Add(ui);
        }
    }

    private void RefreshList()
    {
        foreach (var recipeUI in spawnedRecipes)
        {
            recipeUI.RefreshState();
        }
    }
}