using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingRecipeUI : MonoBehaviour
{
    public RecipeData recipe;
    public Image resultIcon;
    public TextMeshProUGUI recipeNameText;
    public Button craftButton;

    public void Setup(RecipeData data)
    {
        recipe = data;
        resultIcon.sprite = data.resultItem.icon;
        recipeNameText.text = data.resultItem.itemName;
        
        RefreshState();
    }

    public void RefreshState()
    {
        // Highlight if materials are sufficient
        bool canCraft = InventoryManager.Instance.HasIngredients(recipe.ingredients);
        
        // If near station
        if (recipe.requiresStation && !CraftingManager.Instance.isNearStation) canCraft = false;

        craftButton.interactable = canCraft;
        
        // Change color if materials are ready
        craftButton.image.color = canCraft ? Color.white : new Color(1, 1, 1, 0.5f);
    }

    public void OnCraftButtonClicked()
    {
        CraftingManager.Instance.CraftItem(recipe);
    }
}
