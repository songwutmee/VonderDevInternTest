using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public GameObject recipePrefab;
    public Transform container;
    
    [Header("Filter Settings")]
    [Tooltip("If true, shows all recipes. If false, only shows instant recipes.")]
    public bool isStationMenu = false; 

    private List<CraftingRecipeUI> spawnedUIs = new List<CraftingRecipeUI>();

    private void OnEnable()
    {
        GameEvents.OnInventoryUpdated += RefreshUI;
        BuildRecipeList(); 
    }

    private void OnDisable()
    {
        GameEvents.OnInventoryUpdated -= RefreshUI;
    }

    private void BuildRecipeList()
    {
        // Clear old list items
        foreach (Transform child in container) Destroy(child.gameObject);
        spawnedUIs.Clear();

        if (CraftingManager.Instance == null || CraftingManager.Instance.recipes == null) return;

        foreach (var recipe in CraftingManager.Instance.recipes)
        {
            bool shouldShow = false;

            // If this is Station UI show everything
            // If this is Pocket UI show only recipes that dont need  station
            if (isStationMenu) 
            {
                shouldShow = true; 
            }
            else if (!recipe.requiresStation) 
            {
                shouldShow = true;
            }

            if (shouldShow)
            {
                GameObject go = Instantiate(recipePrefab, container);
                CraftingRecipeUI ui = go.GetComponent<CraftingRecipeUI>();
                if (ui != null)
                {
                    ui.Setup(recipe);
                    spawnedUIs.Add(ui);
                }
            }
        }
    }

    public void RefreshUI()
    {
        foreach (var ui in spawnedUIs)
        {
            if (ui != null) ui.Refresh();
        }
    }
}